using RfFondPortfolio.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class StructureLeafEditForm : Form
    {
        protected readonly IUnityContainer _container;

        protected readonly IPaperRepository _paperRepository;

        public PortfolioStructureLeaf RootLeaf { protected get; set; }
        public PortfolioStructureLeaf Leaf { get; set; }

        protected PortfolioStructureLeafPaper SelectedPaper =>
            lvPapers.SelectedItems.Count != 1
                ? null
                : lvPapers.SelectedItems[0].Tag as PortfolioStructureLeafPaper;

        public StructureLeafEditForm(IUnityContainer container)
        {
            _container = container;
            _paperRepository = _container.Resolve<IPaperRepository>();

            InitializeComponent();
        }

        private ListViewItem CreatePaperToListView(PortfolioStructureLeafPaper leafPaper, decimal percent)
        {
            var paper = _paperRepository.Get(leafPaper.SecId);

            var lvi = new ListViewItem(new[]
            {
                percent.ToString("P"),
                leafPaper.Volume.ToString(),
                leafPaper.SecId,
                paper.ShortName
            })
            {
                Tag = leafPaper
            };
            
            return lvi;
        }

        public void DataBind(bool fullRefresh)
        {
            Leaf ??= new PortfolioStructureLeaf();
            Leaf.Papers ??= new List<PortfolioStructureLeafPaper>();

            var selectedPaper = SelectedPaper;
            lvPapers.Items.Clear();

            if (fullRefresh)
            {
                tbName.Text = Leaf.Name;
                tbVolume.Text = Leaf.Volume.ToString();
            }

            var totalVolume = Leaf.Papers.Sum(x => x.Volume);
            foreach (var leafPaper in Leaf.Papers)
            {
                var lvi = CreatePaperToListView(leafPaper, leafPaper.Volume / totalVolume);
                lvPapers.Items.Add(lvi);

                if (leafPaper == selectedPaper)
                {
                    lvi.Selected = true;
                }
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void StructureLeafEditForm_Load(object sender, EventArgs e)
        {
            DataBind(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Leaf.Name = tbName.Text.Trim();
            Leaf.Volume = Convert.ToDecimal(tbVolume.Text.Trim());
            Leaf.Children ??= new List<PortfolioStructureLeaf>();

            Leaf.Papers = new List<PortfolioStructureLeafPaper>();
            foreach (ListViewItem lvi in lvPapers.Items)
            {
                Leaf.Papers.Add(new PortfolioStructureLeafPaper
                {
                    SecId = (lvi.Tag as PortfolioStructureLeafPaper).SecId,
                    Volume = Convert.ToDecimal(lvi.SubItems[1].Text)
                });
            }
        }

        private void btnAddPaper_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<StructureLeafPaperEditForm>())
            {
                f.RootLeaf = RootLeaf;
                f.CurrentLeaf = Leaf;

                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Leaf.Papers.Add(f.SelectedLeafPaper);
                DataBind(false);
            }
        }

        private void btnEditPaper_Click(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }

            using (var f = _container.Resolve<StructureLeafPaperEditForm>())
            {
                f.RootLeaf = RootLeaf;
                f.CurrentLeaf = Leaf;
                f.SelectedLeafPaper = SelectedPaper;

                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                DataBind(false);
            }
        }

        private void btnDeletePaper_Click(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }

            if (MessageBox.Show("Подтвердите удаление записи.", "Удаление записи по бумаге", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            Leaf.Papers.Remove(SelectedPaper);
            DataBind(false);
        }
    }
}
