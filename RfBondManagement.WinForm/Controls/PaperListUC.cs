using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperListUC : UserControl
    {
        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

        public string Filter { get; set; }

        public AbstractPaper SelectedPaper
        {
            get
            {
                if (dgvPapers.SelectedRows.Count != 1)
                {
                    return null;
                }

                return dgvPapers.SelectedRows[0].DataBoundItem as AbstractPaper;
            }
        }

        public void DataBind()
        {
            var papers = PaperRepository.Get();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                Filter = Filter.ToLower();
                papers = papers.Where(x =>
                    x.SecId.ToLower().Contains(Filter) ||
                    x.Isin.ToLower().Contains(Filter) ||
                    x.Name.ToLower().Contains(Filter) ||
                    x.ShortName.ToLower().Contains(Filter));
            }

            dgvPapers.DataSource = papers.ToList();
        }

        public PaperListUC()
        {
            InitializeComponent();
        }

        private void PaperListUC_Load(object sender, EventArgs e)
        {
            if (null == PaperRepository)
            {
                return;
            }

            DataBind();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Filter = tbSearch.Text.Trim();
            tbSearch.Focus();
            DataBind();
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
    }
}
