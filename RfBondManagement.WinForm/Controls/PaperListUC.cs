using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperListUC : UserControl
    {
        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

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
            dgvPapers.DataSource = PaperRepository.Get().ToList();
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
    }
}
