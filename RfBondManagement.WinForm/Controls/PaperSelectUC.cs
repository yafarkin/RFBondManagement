using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectUC : UserControl
    {
        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

        protected IEnumerable<AbstractPaper> _papers;

        protected const int TAKE_COUNT = 25;

        public Func<AbstractPaper, bool> WhereFilter;

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user select stock paper")]
        public event Action<BondPaper> OnSelectPaper;

        public PaperSelectUC()
        {
            InitializeComponent();
        }

        private void PaperSelectUC_Load(object sender, EventArgs e)
        {
            if (null == PaperRepository)
            {
                return;
            }

            cbbPaper.MaxDropDownItems = TAKE_COUNT;

            cbbPaper.DisplayMember = "Name";
            cbbPaper.ValueMember = "Code";

            _papers = PaperRepository.Get();
            if (WhereFilter != null)
            {
                _papers = _papers.Where(WhereFilter);
            }

            cbbPaper.DataSource = _papers.Take(TAKE_COUNT).ToList();
        }

        private void cbPaper_TextChanged(object sender, EventArgs e)
        {
            if (null == PaperRepository)
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
                OnSelectPaper?.Invoke(cbbPaper.SelectedItem as BondPaper);
            }
        }
    }
}
