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

            if (!_historyPrices.ContainsKey(secId) || null == _historyPrices[secId] || 0 == _historyPrices[secId].Count)
            {
                lvi.SubItems[2].Text = "---";
                lvi.SubItems[3].Text = "---";
            }
            else
            {
                var historyPrices = _historyPrices[secId];
                var lastPriceDate = historyPrices.Last().When;
                var fromPriceDate = FindFromPriceDate(lastPriceDate) ?? historyPrices.First().When;

                var lastPriceValues = historyPrices
                    .Where(x => (x.When - fromPriceDate).TotalDays >= 0)
                    .OrderBy(x => x.When).ToList();

                fromPriceDate = lastPriceValues.First().When;
                var toPriceDate = lastPriceValues.Last().When;

                var pricePeriod = _historyPrices[secId].Where(p => p.When >= fromPriceDate && p.When <= toPriceDate);
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
            if (null == paperPrice)
            {
                return;
            }

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
                    string.Empty
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

        private DateTime? FindFromPriceDate(DateTime lastPriceDate)
        {
            DateTime? fromPriceDate;

            if (rbWeek.Checked)
            {
                fromPriceDate = lastPriceDate.AddDays(-7);
            }
            else if (rbMonth.Checked)
            {
                fromPriceDate = lastPriceDate.AddMonths(-1);
            }
            else if (rb3Months.Checked)
            {
                fromPriceDate = lastPriceDate.AddMonths(-3);
            }
            else if (rb6Months.Checked)
            {
                fromPriceDate = lastPriceDate.AddMonths(-6);
            }
            else if (rbYear.Checked)
            {
                fromPriceDate = lastPriceDate.AddYears(-1);
            }
            else if (rbYTD.Checked)
            {
                fromPriceDate = new DateTime(lastPriceDate.Year, 1, 1);
            }
            else if (rb5Years.Checked)
            {
                fromPriceDate = lastPriceDate.AddYears(-5);
            }
            else
            {
                fromPriceDate = null;
            }

            return fromPriceDate;
        }

        private void DrawGraph(string secId)
        {
            if (string.IsNullOrWhiteSpace(secId) || !_historyPrices.ContainsKey(secId))
            {
                pnlGraphData.Controls.Clear();
                return;
            }

            var historyPrices = _historyPrices[secId];
            if (null == historyPrices || 0 == historyPrices.Count)
            {
                pnlGraphData.Controls.Clear();
                return;
            }

            var chart = new ZedGraphControl
            {
                Dock = DockStyle.Fill
            };

            var gp = chart.GraphPane;
            gp.Title.Text = secId;

            var lastPriceDate = historyPrices.Last().When;

            var fromPriceDate = FindFromPriceDate(lastPriceDate) ?? historyPrices.First().When;

            var lastPriceValues = historyPrices
                .Where(x => (x.When - fromPriceDate).TotalDays >= 0)
                .OrderBy(x => x.When).ToList();

            fromPriceDate = lastPriceValues.First().When;

            var yAxis = gp.YAxis;
            var xAxis = gp.XAxis;

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

                xAxis.Scale.Min = fromPriceDate.ToOADate();
                xAxis.Scale.Max = lastPriceDate.ToOADate();
            }
            else
            {
                list = new PointPairList();

                double fromValue = 0;
                double toValue = 0;

                for (var i = 0; i < historyPrices.Count; i++)
                {
                    var whenXDate = new XDate(historyPrices[i].When);

                    if (historyPrices[i].When == fromPriceDate)
                    {
                        fromValue = whenXDate.XLDate;
                    }

                    if (historyPrices[i].When == lastPriceDate)
                    {
                        toValue = whenXDate.XLDate;
                    }

                    (list as PointPairList).Add(whenXDate, (double)historyPrices[i].ClosePrice);
                }

                var curve = gp.AddCurve(string.Empty, list, Color.Blue, SymbolType.Default);
                curve.Line.Fill = new Fill(Color.FromArgb(153, 212, 255));
                curve.Line.IsSmooth = true;
                curve.Line.Width = 4;
                curve.Color = Color.FromArgb(0, 148, 255);
                curve.Symbol.Size = 4;

                xAxis.Scale.Min = fromValue;
                xAxis.Scale.Max = toValue;
            }

            yAxis.Title.Text = "Цена";
            yAxis.Scale.Min = (double)lastPriceValues.Min(x => x.LowPrice) * 0.95;
            yAxis.Scale.Max = (double)lastPriceValues.Max(x => x.HighPrice) * 1.05;
            yAxis.MajorGrid.IsVisible = true;
            yAxis.MinorGrid.IsVisible = true;

            xAxis.Title.Text = "Дата";
            xAxis.Type = AxisType.Date;
            xAxis.Scale.TextLabels = historyPrices.Select(x => x.When.ToShortDateString()).ToArray();
            xAxis.MajorGrid.IsVisible = true;
            xAxis.MinorGrid.IsVisible = true;

            chart.IsAntiAlias = true;
            chart.IsShowPointValues = true;
            chart.AxisChange();

            pnlGraphData.Controls.Clear();
            pnlGraphData.Controls.Add(chart);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = Container.Resolve<PaperListForm>())
            {
                f.AllowSelectPaper = true;

                if (f.ShowDialog() != DialogResult.OK)
                {
                    DataBind();
                    return;
                }

                var paper = f.SelectedPaper;
                if (null == paper)
                {
                    DataBind();
                    return;
                }

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
            if (sender is RadioButton rb && !rb.Checked)
            {
                return;
            }

            foreach (var kv in _historyPrices)
            {
                FillMinMax(kv.Key);
            }

            DrawGraph(SelectedPaper);
        }

        private void rbGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && !rb.Checked)
            {
                return;
            }

            DrawGraph(SelectedPaper);
        }
    }
}
