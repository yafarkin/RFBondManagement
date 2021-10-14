using RfBondManagement.WinForm.Forms;
using RfFondPortfolio.Common.Dtos;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class PaperSelectUC : UserControl
    {
        [Dependency]
        public IUnityContainer DiContainer { get; set; }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user select stock paper")]
        public event Action<AbstractPaper> OnSelectPaper;

        protected AbstractPaper _selectedPaper;

        public AbstractPaper SelectedPaper
        {
            get => _selectedPaper;
            set
            {
                _selectedPaper = value;
                tbSelectedPaper.Text = null == value ? string.Empty : $"{value.SecId} ({value.ShortName})";
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

                SelectedPaper = f.SelectedPaper;
                OnSelectPaper?.Invoke(SelectedPaper);
            }
        }

        private void PaperSelectUC_Load(object sender, EventArgs e)
        {
            if (null == SelectedPaper)
            {
                return;
            }
        }
    }
}
