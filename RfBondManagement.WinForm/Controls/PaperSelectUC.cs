using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectUC : UserControl
    {
        protected IDatabaseLayer _db;
        protected IEnumerable<BaseStockPaper> _papers;

        protected const int TAKE_COUNT = 25;


        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user select stock paper")]
        public event Action<BaseStockPaper> OnSelectStockPaper;

        public PaperSelectUC()
        {
            InitializeComponent();
        }

        public void InitDI(IDatabaseLayer db)
        {
            _db = db;
        }

        private void PaperSelectUC_Load(object sender, EventArgs e)
        {
            if (null == _db)
            {
                return;
            }

            cbbPaper.MaxDropDownItems = TAKE_COUNT;

            cbbPaper.DisplayMember = "Name";
            cbbPaper.ValueMember = "Code";

            _papers = _db.SelectPapers();
            cbbPaper.DataSource = _papers.Take(TAKE_COUNT).ToList();
        }

        private void cbPaper_TextChanged(object sender, EventArgs e)
        {
            if (null == _db)
            {
                return;
            }

            var text = cbbPaper.Text.Trim().ToLower();
            cbbPaper.DataSource = _papers.Where(p => string.IsNullOrEmpty(text) ||
                    p.Name.ToLower().Contains(text) || p.SecId.ToLower().Contains(text) ||
                    p.Isin.ToLower().Contains(text))
                .Take(TAKE_COUNT)
                .ToList();
        }

        private void cbPaper_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbbPaper.SelectedItem != null)
            {
                OnSelectStockPaper?.Invoke(cbbPaper.SelectedItem as BaseStockPaper);
            }
        }
    }
}
