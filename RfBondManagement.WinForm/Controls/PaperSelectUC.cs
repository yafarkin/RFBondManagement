using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectUC : UserControl
    {
        protected IPaperRepository _paperRepository;
        protected IEnumerable<AbstractPaper> _papers;

        protected const int TAKE_COUNT = 25;

        public Func<AbstractPaper, bool> WhereFilter;

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user select stock paper")]
        public event Action<BaseStockPaper> OnSelectStockPaper;

        public PaperSelectUC()
        {
            InitializeComponent();
        }

        public void InitDI(IPaperRepository paperRepository)
        {
            _paperRepository = paperRepository;
        }

        private void PaperSelectUC_Load(object sender, EventArgs e)
        {
            if (null == _paperRepository)
            {
                return;
            }

            cbbPaper.MaxDropDownItems = TAKE_COUNT;

            cbbPaper.DisplayMember = "Name";
            cbbPaper.ValueMember = "Code";

            _papers = _paperRepository.Get();
            if (WhereFilter != null)
            {
                _papers = _papers.Where(WhereFilter);
            }

            cbbPaper.DataSource = _papers.Take(TAKE_COUNT).ToList();
        }

        private void cbPaper_TextChanged(object sender, EventArgs e)
        {
            if (null == _paperRepository)
            {
                return;
            }

            var text = cbbPaper.Text.Trim().ToLower();
            var query = _papers.Where(p => string.IsNullOrEmpty(text) ||
                                           p.Name.ToLower().Contains(text) || p.SecId.ToLower().Contains(text) ||
                                           p.Isin.ToLower().Contains(text));
            cbbPaper.DataSource =query.Take(TAKE_COUNT).ToList();
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
