using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;

namespace RfBondManagement.WinForm.Controls
{
    public partial class DisplayPaperListUC : UserControl
    {
        [Dependency]
        protected IBondCalculator _bondCalculator;

        [Dependency]
        protected PortfolioEngine _engine;

        public IList<PaperType> PaperTypes { get; set; }
        public Portfolio Portfolio { get; set; }

        public DisplayPaperListUC()
        {
            InitializeComponent();
        }

        protected void BuildHeaders()
        {
            var headers = new List<ColumnHeader>
            {
                new() {Name = "SecId"},
                new() {Name = "Название"},
                new() {Name = "Цена покупки"},
                new() {Name = "Цена рын."},
                new() {Name = "% изменения"},
                new() {Name = "Кол-во"},
                new() {Name = "Сумма"},
                new() {Name = "Доля, %"}
            };

            if (PaperTypes.Any(p => p == PaperType.Bond))
            {
                headers.Add(new ColumnHeader { Name = "НКД (сумма)" });
                headers.Add(new ColumnHeader { Name = "След. купон" });
                headers.Add(new ColumnHeader { Name = "% дох. в портф." });
                //headers.Add(new ColumnHeader { Name = "% дох. рыночн." });
            }

            lvPapers.Columns.Clear();
            lvPapers.Columns.AddRange(headers.ToArray());
        }

        protected async Task BindData()
        {
            var content = _engine.Build();
            content.Papers = new ReadOnlyCollection<IPaperInPortfolio<AbstractPaper>>(
                content.Papers.Where(p => PaperTypes.Contains(p.Paper.PaperType)).ToList());
            await _engine.FillPrice(content);

            var totalAvgSum = content.Papers.Sum(p => p.AveragePrice * p.Count);
            var totalMarketSum = content.Papers.Sum(p => p.MarketPrice * p.Count);
            foreach (var paperInPortfolio in content.Papers)
            {
                var paper = paperInPortfolio.Paper;

                var percentChange = paperInPortfolio.MarketPrice / paperInPortfolio.AveragePrice - 1;
                var percentInPortfolio = (paperInPortfolio.MarketPrice * paperInPortfolio.Count) / totalMarketSum;

                var itemText = new List<string>
                {
                    paper.SecId,
                    paper.Name,
                    paperInPortfolio.AveragePrice.ToString("N4"),
                    paperInPortfolio.MarketPrice.ToString("N4"),
                    percentChange.ToString("P"),
                    paperInPortfolio.Count.ToString(),
                    (paperInPortfolio.Count * paperInPortfolio.MarketPrice).ToString("N4"),
                    percentInPortfolio.ToString("P")
                };

                if (PaperTypes.Any(p => p == PaperType.Bond))
                {
                    var bondPaper = paper as BondPaper;

                    var aci = _bondCalculator.CalculateAci(bondPaper, DateTime.Today) * paperInPortfolio.Count;
                    var nextCoupon = _bondCalculator.NearFutureCoupon(bondPaper, DateTime.Today);

                    var bii = new BondIncomeInfo
                    {
                        BondInPortfolio = new BondInPortfolio(bondPaper)
                        {
                            Actions = paperInPortfolio.Actions
                        }
                    };
                    _bondCalculator.CalculateIncome(bii, Portfolio, DateTime.Today);

                    itemText.Add(aci.ToString("N2"));
                    itemText.Add(nextCoupon.CouponDate.ToShortDateString());
                    itemText.Add(bii.RealIncomePercent.ToString("P"));
                }

                var lvi = new ListViewItem(itemText.ToArray()) {Tag = paperInPortfolio};
                lvPapers.Items.Add(lvi);
            }

            lblPrice.Text = $"{totalMarketSum:N4} ({(totalMarketSum/totalAvgSum-1):P})";
        }

        private async void DisplayPaperListUC_Load(object sender, EventArgs e)
        {
            if (null == Portfolio)
            {
                return;
            }

            BuildHeaders();
            await BindData();
        }
    }
}
