using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class PaperListForm : Form
    {
        protected ILogger _logger;
        protected readonly IDatabaseLayer _db;
        protected readonly IList<BaseStockPaper> _papers;
        protected IUnityContainer _container;

        public PaperListForm(ILogger logger, IDatabaseLayer db, IUnityContainer container)
        {
            _logger = logger;
            _db = db;
            _container = container;

            _papers = _db.GetPapers().ToList();

            InitializeComponent();
        }

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PaperListForm_Load(object sender, EventArgs e)
        {
            tbSearch.Focus();

            foreach (var paper in _papers)
            {
                var itemText = new[] {paper.Isin, paper.Type, paper.Name};
                var lvi = new ListViewItem(itemText) {Tag = paper};
                lvPapers.Items.Add(lvi);
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<PaperForm>())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                throw new NotImplementedException();
            }
        }

        private void menuItemEdit_Click(object sender, EventArgs e)
        {
            if (lvPapers.SelectedItems.Count != 1)
            {
                return;
            }

            using (var f = _container.Resolve<PaperForm>())
            {
                f.Paper = lvPapers.SelectedItems[0].Tag as BaseStockPaper;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            if (lvPapers.SelectedItems.Count != 1)
            {
                return;
            }

            if (MessageBox.Show("Подтвердите удаление бумаги", "Удаление бумаги", MessageBoxButtons.OKCancel) !=
                DialogResult.OK)
            {
                return;
            }

            throw new NotImplementedException();
        }
    }
}
