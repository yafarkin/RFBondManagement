using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    internal class DGVDisplayEntity
    {
        public string GlobalType { get; set; }
        public string LocalType { get; set; }
        public DateTime When { get; set; }
        public string Comment { get; set; }
        public string Sum { get; set; }
        public string Count { get; set; }
        public string Price { get; set; }
        public string SecId { get; set; }
    }

    public partial class PortfolioActionsUC : UserControl
    {
        [Dependency]
        public IPortfolioMoneyActionRepository MoneyActionRepository { get; set; }

        [Dependency]
        public IPortfolioPaperActionRepository PaperActionRepository { get; set; }

        public Portfolio Portfolio { get; set; }

        public PortfolioActionsUC()
        {
            InitializeComponent();
        }

        public void DataBind()
        {
            var actions = new List<DGVDisplayEntity>();

            foreach (var moneyAction in MoneyActionRepository.Get())
            {
                actions.Add(new DGVDisplayEntity
                {
                    GlobalType = "Деньги",
                    LocalType = moneyAction.MoneyAction.ToString(),
                    When = moneyAction.When,
                    Comment = moneyAction.Comment,
                    Sum = moneyAction.Sum.ToString("N2"),
                });
            }

            foreach (var paperAction in PaperActionRepository.Get())
            {
                actions.Add(new DGVDisplayEntity
                {
                    GlobalType = "Бумага",
                    LocalType = paperAction.PaperAction.ToString(),
                    When = paperAction.When,
                    Comment = paperAction.Comment,
                    Sum = paperAction.Sum.ToString("N2"),
                    SecId = paperAction.SecId,
                    Count = paperAction.Count.ToString("N"),
                    Price = paperAction.Value.ToString("N2")
                });
            }

            actions = actions.OrderByDescending(x => x.When).ToList();

            dgvActions.AutoGenerateColumns = false;
            dgvActions.DataSource = new SortableBindingList<DGVDisplayEntity>(actions);
        }

        private void PortfolioActionsUC_Load(object sender, EventArgs e)
        {
            if (null == MoneyActionRepository || null == PaperActionRepository || null == Portfolio)
            {
                return;
            }

            MoneyActionRepository.Setup(Portfolio.Id);
            PaperActionRepository.Setup(Portfolio.Id);

            DataBind();
        }
    }
}
