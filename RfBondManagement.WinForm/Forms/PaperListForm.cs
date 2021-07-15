using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class PaperListForm : Form
    {
        protected ILogger _logger;
        protected readonly IList<AbstractPaper> _papers;
        protected IUnityContainer _container;
        protected IPaperRepository _paperRepository;

        public PaperListForm(ILogger logger, IPaperRepository paperRepository, IUnityContainer container)
        {
            _logger = logger;
            _paperRepository = paperRepository;
            _container = container;

            _papers = _paperRepository.Get().ToList();

            InitializeComponent();
        }

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PaperListForm_Load(object sender, EventArgs e)
        {
            tbSearch.Focus();

            DataBind();
        }

        private void DataBind()
        {
            var selectedPaper = 0 == lvPapers.SelectedItems.Count ? null : lvPapers.SelectedItems[0].Tag as AbstractPaper;

            lvPapers.Items.Clear();

            foreach (var paper in _papers)
            {
                var itemText = new[] {paper.SecId, paper.Isin, paper.Type, paper.Name};
                var lvi = new ListViewItem(itemText) {Tag = paper};
                lvPapers.Items.Add(lvi);

                if (paper.Id == selectedPaper?.Id)
                {
                    lvi.Selected = true;
                }
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            using (var f = _container.Resolve<PaperForm>())
            {
                while (true)
                {
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var paper = f.Paper;
                    if (_papers.Any(p => p.SecId == paper.SecId))
                    {
                        MessageBox.Show("Бумага с таким SECID уже есть в системе", "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        continue;

                    }

                    paper = _paperRepository.Insert(paper);
                    _papers.Add(paper);

                    DataBind();

                    break;
                }
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
                var originalPaper = lvPapers.SelectedItems[0].Tag as AbstractPaper;
                f.Paper = originalPaper;

                while (true)
                {
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var paper = f.Paper;
                    if (_papers.Any(p => p.SecId == paper.SecId && p.Id != paper.Id))
                    {
                        MessageBox.Show("Бумага с таким SECID уже есть в системе", "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        continue;

                    }

                    _paperRepository.Update(paper);

                    _papers.Remove(originalPaper);
                    _papers.Add(paper);

                    DataBind();

                    break;
                }
            }
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            if (lvPapers.SelectedItems.Count != 1)
            {
                return;
            }

            if (MessageBox.Show("Подтвердите удаление бумаги", "Удаление бумаги", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            var paper = lvPapers.SelectedItems[0].Tag as AbstractPaper;
            _paperRepository.Delete(paper);
            _papers.Remove(paper);
            DataBind();
        }
    }
}
