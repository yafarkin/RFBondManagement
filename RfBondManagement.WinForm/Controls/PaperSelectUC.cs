using RfBondManagement.WinForm.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectUC : UserControl
    {
        [Dependency]
        public IUnityContainer DiContainer { get; set; }

        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user select stock paper")]
        public event Action<string> OnSelectPaper;

        protected string _secId;

        public string SecId
        {
            get => _secId;
            set
            {
                _secId = value;
                var paper = PaperRepository.Get(_secId);
                tbSelectedPaper.Text = null == value ? string.Empty : $"{value} ({paper.ShortName})";
            }
        }

        public PaperSelectUC()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (var f = DiContainer.Resolve<PaperListForm>())
            {
                f.AllowSelectPaper = true;
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SecId = f.SelectedPaper.SecId;
                OnSelectPaper?.Invoke(SecId);
            }
        }

        private void PaperSelectUC_Load(object sender, EventArgs e)
        {
            if (null == SecId)
            {
                return;
            }
        }
    }
}
