using RfFondPortfolio.Common.Dtos;
using System;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class StructureLeafPaperEditForm : Form
    {
        public PortfolioStructureLeaf RootLeaf { protected get; set; }
        public PortfolioStructureLeaf CurrentLeaf { protected get; set; }
        public PortfolioStructureLeafPaper SelectedLeafPaper { get; set; }

        protected IUnityContainer _container;

        public StructureLeafPaperEditForm(IUnityContainer container)
        {
            InitializeComponent();

            _container = container;
            _container.BuildUp(paperSelect);
        }

        private void StructureLeafPaperEditForm_Load(object sender, EventArgs e)
        {
            if (null == SelectedLeafPaper)
            {
                return;
            }

            paperSelect.SecId = SelectedLeafPaper.SecId;
            tbVolume.Text = SelectedLeafPaper.Volume.ToString();
        }

        protected bool IsPaperAlreadyExist(PortfolioStructureLeaf leaf, string secId)
        {
            if (leaf == CurrentLeaf)
            {
                return false;
            }

            if (leaf.Papers.Any(x => x.SecId == secId))
            {
                return true;
            }

            foreach (var child in leaf.Children)
            {
                if (IsPaperAlreadyExist(child, secId))
                {
                    return true;
                }
            }

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPaperAlreadyExist(RootLeaf, paperSelect.SecId))
            {
                DialogResult = DialogResult.None;
                paperSelect.Focus();
                MessageBox.Show("Данная бумага уже включена в структуру", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedLeafPaper ??= new PortfolioStructureLeafPaper();

            SelectedLeafPaper.SecId = paperSelect.SecId;
            SelectedLeafPaper.Volume = Convert.ToDecimal(tbVolume.Text);
        }
    }
}
