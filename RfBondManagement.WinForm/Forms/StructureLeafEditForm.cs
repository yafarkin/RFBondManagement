using RfFondPortfolio.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class StructureLeafEditForm : Form
    {
        protected readonly IUnityContainer _container;

        public PortfolioStructureLeaf Leaf { get; set; }

        public PortfolioStructureLeafPaper SelectedPaper =>
            lvPapers.SelectedItems.Count != 1
                ? null
                : lvPapers.SelectedItems[0].Tag as PortfolioStructureLeafPaper;

        public StructureLeafEditForm(IUnityContainer container)
        {
            _container = container;

            InitializeComponent();
        }

        private ListViewItem CreatePaperToListView(PortfolioStructureLeafPaper leafPaper, decimal percent)
        {
            var lvi = new ListViewItem(new[]
            {
                percent.ToString("P"),
                leafPaper.Volume.ToString(),
                leafPaper.Paper.SecId,
                leafPaper.Paper.ShortName
            })
            {
                Tag = leafPaper
            };
            
            return lvi;
        }

        private void StructureLeafEditForm_Load(object sender, EventArgs e)
        {
            if (null == Leaf)
            {
                return;
            }

            tbName.Text = Leaf.Name;
            tbVolume.Text = Leaf.Volume.ToString();

            if (Leaf.Papers != null)
            {
                var totalVolume = Leaf.Papers.Sum(x => x.Volume);
                foreach (var leafPaper in Leaf.Papers)
                {
                    var lvi = CreatePaperToListView(leafPaper, leafPaper.Volume / totalVolume);
                    lvPapers.Items.Add(lvi);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Leaf ??= new PortfolioStructureLeaf();
            Leaf.Papers ??= new List<PortfolioStructureLeafPaper>();
            Leaf.Name = tbName.Text.Trim();
            Leaf.Volume = Convert.ToDecimal(tbVolume.Text.Trim());

            var cloneList = new List<PortfolioStructureLeafPaper>(Leaf.Papers);
            foreach (var leafPaper in cloneList)
            {
                var lvi = lvPapers.FindItemWithText(leafPaper.Paper.SecId);
                if (null == lvi)
                {
                    Leaf.Papers.Remove(leafPaper);
                }
                else
                {
                    leafPaper.Volume = Convert.ToDecimal(lvi.SubItems[1].Text);
                }
            }

            foreach (ListViewItem lvi in lvPapers.Items)
            {
                var leafPaper = Leaf.Papers.FirstOrDefault(x => x.Paper.SecId == lvi.Text);
                if (null == leafPaper)
                {
                    Leaf.Papers.Add(new PortfolioStructureLeafPaper
                    {
                        Paper = lvi.Tag as AbstractPaper,
                        Volume = Convert.ToDecimal(lvi.SubItems[1].Text)
                    });
                }
                else
                {
                    leafPaper.Volume = Convert.ToDecimal(lvi.SubItems[1].Text);
                }
            }
        }

        private void btnAddPaper_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<StructureLeafPaperEditForm>())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Leaf.Papers.Add(f.SelectedLeafPaper);

                var totalVolume = Leaf.Papers.Sum(x => x.Volume);
                var lvi = CreatePaperToListView(f.SelectedLeafPaper, f.SelectedLeafPaper.Volume / totalVolume);
                lvPapers.Items.Add(lvi);
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
                f.SelectedLeafPaper = SelectedPaper;

                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var totalVolume = Leaf.Papers.Sum(x => x.Volume);

                var lvi = lvPapers.SelectedItems[0];
                lvi.Tag = f.SelectedLeafPaper.Paper;
                lvi.SubItems[0].Text = (f.SelectedLeafPaper.Volume / totalVolume).ToString("P");
                lvi.SubItems[1].Text = f.SelectedLeafPaper.Volume.ToString();
                lvi.SubItems[2].Text = f.SelectedLeafPaper.Paper.SecId;
                lvi.SubItems[3].Text = f.SelectedLeafPaper.Paper.ShortName;
            }
        }

        private void btnDeletePaper_Click(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }

        }
    }
}
