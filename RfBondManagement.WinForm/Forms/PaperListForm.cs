using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Forms
{
    public partial class PaperListForm : Form
    {
        protected ILogger _logger;
        protected IUnityContainer _container;
        protected IPaperRepository _paperRepository;

        public AbstractPaper SelectedPaper => paperList.SelectedPaper;

        public bool AllowSelectPaper { get; set; }

        public PaperListForm(ILogger logger, IPaperRepository paperRepository, IUnityContainer container)
        {
            _logger = logger;
            _paperRepository = paperRepository;
            _container = container;

            InitializeComponent();

            _container.BuildUp(paperList);
        }

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            Close();
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
                    if (_paperRepository.Get(paper.SecId) != null)
                    {
                        MessageBox.Show("Бумага с таким SECID уже есть в системе", "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        continue;

                    }

                    _paperRepository.Insert(paper);
                    paperList.DataBind();

                    break;
                }
            }
        }

        private void menuItemEdit_Click(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }

            using (var f = _container.Resolve<PaperForm>())
            {
                var originalPaper = SelectedPaper;
                f.Paper = originalPaper;

                while (true)
                {
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var paper = f.Paper;
                    var repoPaper = _paperRepository.Get(paper.SecId);
                    if (repoPaper !=null && repoPaper.Id != paper.Id)
                    {
                        MessageBox.Show("Бумага с таким SECID уже есть в системе", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;

                    }

                    _paperRepository.Update(paper);
                    paperList.DataBind();

                    break;
                }
            }
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }

            if (MessageBox.Show("Подтвердите удаление бумаги. Может привести к несогласованности данных.", "Удаление бумаги", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            _paperRepository.Delete(SelectedPaper);
            paperList.DataBind();
        }

        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void PaperListForm_Load(object sender, EventArgs e)
        {
            menuItemSelect.Visible = AllowSelectPaper;
        }
    }
}
