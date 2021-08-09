using RfFondPortfolio.Common.Dtos;
using System;
using System.Windows.Forms;

namespace RfBondManagement.WinForm.Forms
{
    public partial class StructureLeafPaperEditForm : Form
    {
        public PortfolioStructureLeafPaper SelectedLeafPaper { get; set; }

        public StructureLeafPaperEditForm()
        {
            InitializeComponent();
        }

        private void StructureLeafPaperEditForm_Load(object sender, EventArgs e)
        {
            if (null == SelectedLeafPaper)
            {
                return;
            }

            paperSelect.SelectedPaper = SelectedLeafPaper.Paper;
            tbVolume.Text = SelectedLeafPaper.Volume.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SelectedLeafPaper ??= new PortfolioStructureLeafPaper();

            SelectedLeafPaper.Paper = paperSelect.SelectedPaper;
            SelectedLeafPaper.Volume = Convert.ToDecimal(tbVolume.Text);
        }
    }
}
