using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectControl : UserControl
    {
        protected IDatabaseLayer _db;
        protected List<BaseStockPaper> _papers;

        protected const int TAKE_COUNT = 25;

        public PaperSelectControl()
        {
        }

        public PaperSelectControl(IDatabaseLayer db)
        {
            _db = db;
            InitializeComponent();
        }

        private void PaperSelectControl_Load(object sender, EventArgs e)
        {
            _papers = _db.GetPapers().ToList();
            cbPaper.DataSource = _papers.Take(TAKE_COUNT);
        }

        private void cbPaper_TextChanged(object sender, EventArgs e)
        {
            var text = cbPaper.Text.Trim().ToLower();
            cbPaper.DataSource = _papers.Where(p => string.IsNullOrEmpty(text) ||
                    p.Name.ToLower().Contains(text) || p.Code.ToLower().Contains(text) ||
                    p.ISIN.ToLower().Contains(text))
                .Take(TAKE_COUNT);
            cbPaper.DisplayMember = "Name";
        }
    }
}
