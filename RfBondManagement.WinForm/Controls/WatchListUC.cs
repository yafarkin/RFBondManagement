using NLog;
using RfBondManagement.Engine;
using RfBondManagement.WinForm.Forms;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using ZedGraph;

namespace RfBondManagement.WinForm.Controls
{
    public partial class WatchListUC : UserControl
    {
        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

        [Dependency]
        public HistoryEngine HistoryEngine { get; set; }

        [Dependency]
        public ILogger Logger { get; set; }

        [Dependency]
        public IExternalImport ExternalImport { get; set; }

        [Dependency]
        public IUnityContainer Container { get; set; }

        public string SelectedPaper
        {
            get
            {
                if (0 == lvPapers.SelectedItems.Count)
                {
                    return string.Empty;
                }

                return lvPapers.SelectedItems[0].Text;
            }
        }

        protected IDictionary<string, IList<HistoryPrice>> _historyPrices = new ConcurrentDictionary<string, IList<HistoryPrice>>();

        public WatchListUC()
        {
            InitializeComponent();
        }

        public void FillMinMax(string secId)
        {
            var lvi = lvPapers.FindItemWithText(secId);
            if (null == lvi)
            {
                return;
            }

            if (!_historyPrices.ContainsKey(secId))
            {
                lvi.SubItems[2].Text = "---";
                lvi.SubItems[3].Text = "---";
            }
            else
            {
                var to = DateTime.Today;
                var from = rbDay.Checked ? to.AddDays(-1) : rbWeek.Checked ? to.AddDays(-7) : to.AddMonths(-1);

                var pricePeriod = _historyPrices[secId].Where(p => p.When >= from && p.When <= to);
                var minPrice = pricePeriod.DefaultIfEmpty().Min(p => p?.ClosePrice);
                var maxPrice = pricePeriod.DefaultIfEmpty().Max(p => p?.ClosePrice);

                lvi.SubItems[2].Text = minPrice?.ToString("N2") ?? "---";
                lvi.SubItems[3].Text = maxPrice?.ToString("N2") ?? "---";
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public async Task UpdateHistoryPrices(string secId)
        {
            var lastPriceDate = HistoryEngine.GetLastHistoryDate(secId);
            if (null == lastPriceDate || lastPriceDate < DateTime.Today.AddDays(-3))
            {
                await HistoryEngine.ImportHistory(secId);

                _historyPrices.TryAdd(secId, HistoryEngine.GetHistoryPrices(secId).OrderBy(x => x.When).ToList());

                FillMinMax(secId);
            }
            else
            {
                _historyPrices.TryAdd(secId, HistoryEngine.GetHistoryPrices(secId).OrderBy(x => x.When).ToList());
                FillMinMax(secId);
            }
        }

        public async Task UpdateListPrice(AbstractPaper paper)
        {
            var paperPrice = await ExternalImport.LastPrice(Logger, paper);

            var secId = paperPrice.SecId;
            var lastPrice = paperPrice.Price;
            var lvi = lvPapers.FindItemWithText(secId);
            if (lvi != null)
            {
                lvi.SubItems[1].Text = lastPrice.ToString("N2");
                lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        public void DataBind()
        {
            if (null == PaperRepository)
            {
                return;
            }

            var favoritePapers = PaperRepository.Get().Where(p => p.IsFavorite).ToList();

            var selectedSecId = 0 == lvPapers.SelectedItems.Count ? string.Empty : lvPapers.SelectedItems[0].Text;
            lvPapers.Items.Clear();

            foreach (var favoritePaper in favoritePapers)
            {
                var lvi = new ListViewItem(new[]
                {
                    favoritePaper.SecId,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "Удалить"
                })
                {
                    Tag = favoritePaper
                };

                lvPapers.Items.Add(lvi);
                if (favoritePaper.SecId == selectedSecId)
                {
                    lvi.Selected = true;
                }

#pragma warning disable 4014
                UpdateHistoryPrices(favoritePaper.SecId);
                UpdateListPrice(favoritePaper);
#pragma warning restore 4014
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void DrawGraph(string secId)
        {
            if (string.IsNullOrWhiteSpace(secId) || !_historyPrices.ContainsKey(secId))
            {
                pnlGraphData.Controls.Clear();
                return;
            }

            var historyPrices = _historyPrices[secId];

            var chart = new ZedGraphControl
            {
                Dock = DockStyle.Fill
            };

            var gp = chart.GraphPane;
            gp.Title.Text = secId;

            var days = rbDay.Checked ? 1 : rbWeek.Checked ? 7 : 30;
            var lastPriceValues = historyPrices.TakeLast(days).ToList();
            var fromPrice = lastPriceValues.First();
            var toPrice = lastPriceValues.Last();

            var yAxis = gp.YAxis;
            var xAxis = gp.XAxis;

            yAxis.Title.Text = "Цена";
            yAxis.Scale.Min = (double)lastPriceValues.Min(x => x.LowPrice) * 0.95;
            yAxis.Scale.Max = (double)lastPriceValues.Max(x => x.HighPrice) * 1.05;

            xAxis.Title.Text = "Дата";
            xAxis.Type = AxisType.Date;
            xAxis.Scale.TextLabels = historyPrices.Select(x => x.When.ToShortDateString()).ToArray();

            IPointList list;

            if (rbGraphCandle.Checked)
            {
                list = new StockPointList();

                for (var i = 0; i < historyPrices.Count; i++)
                {
                    var d = new XDate(historyPrices[i].When);
                    var open = (double) historyPrices[i].OpenPrice;
                    var close = (double) historyPrices[i].ClosePrice;
                    var high = (double) historyPrices[i].HighPrice;
                    var low = (double) historyPrices[i].LowPrice;
                    var vol = (double) historyPrices[i].Volume;

                    var p = new StockPt(d.XLDate, high, low, open, close, vol);

                    (list as StockPointList).Add(p);
                }

                var curve = gp.AddJapaneseCandleStick(string.Empty, list);
                curve.Stick.RisingFill = new Fill(Color.Green, Color.LightGreen);
                curve.Stick.FallingFill = new Fill(Color.Red, Color.IndianRed);

                xAxis.Scale.Min = fromPrice.When.ToOADate();
                xAxis.Scale.Max = toPrice.When.ToOADate();
            }
            else
            {
                list = new PointPairList();

                var fromValue = 0;
                var toValue = 0;

                for (var i = 0; i < historyPrices.Count; i++)
                {
                    if (historyPrices[i].When == fromPrice.When)
                    {
                        fromValue = i;
                    }

                    if (historyPrices[i].When == toPrice.When)
                    {
                        toValue = i;
                    }

                    (list as PointPairList).Add((double)i, (double)historyPrices[i].ClosePrice);
                }

                gp.AddCurve(string.Empty, list, Color.Blue, SymbolType.None);

                xAxis.Scale.Min = fromValue;
                xAxis.Scale.Max = toValue;
            }

            chart.IsAntiAlias = true;
            chart.IsShowPointValues = true;
            chart.AxisChange();

            pnlGraphData.Controls.Clear();
            pnlGraphData.Controls.Add(chart);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<PaperForm>())
            {
                if (f.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var paper = f.Paper;

                var existPaper = PaperRepository.Get(paper.SecId);
                if (existPaper != null)
                {
                    if (!existPaper.IsFavorite)
                    {
                        existPaper.IsFavorite = true;
                        PaperRepository.Update(existPaper);
                    }
                }
                else
                {
                    paper.IsFavorite = true;
                    PaperRepository.Insert(paper);
                }

                DataBind();
            }
        }

        private void lvPapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = 0 == lvPapers.SelectedItems.Count ? string.Empty : lvPapers.SelectedItems[0].Text;
            DrawGraph(selectedItem);
        }

        private void WatchListUC_Load(object sender, EventArgs e)
        {
            DataBind();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            DataBind();
        }

        private void rbPeriod_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var kv in _historyPrices)
            {
                FillMinMax(kv.Key);
            }

            DrawGraph(SelectedPaper);
        }

        private void rbGraph_CheckedChanged(object sender, EventArgs e)
        {
            DrawGraph(SelectedPaper);
        }
    }
}
