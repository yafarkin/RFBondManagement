using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.WinForm.Forms;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Unity;
using Unity.Injection;
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

        protected SynchronizationContext _syncContext;

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

                var lastPriceDate = HistoryEngine.GetLastHistoryDate(favoritePaper.SecId);
                if (null == lastPriceDate || lastPriceDate > DateTime.Today.AddDays(-3))
                {
                    HistoryEngine.ImportHistory(favoritePaper.SecId).ContinueWith(t =>
                    {
                        _historyPrices.TryAdd(favoritePaper.SecId,
                            HistoryEngine.GetHistoryPrices(favoritePaper.SecId).OrderBy(x => x.When).ToList());

                        _syncContext.Send(s =>
                        {
                            FillMinMax(s + string.Empty);
                        }, favoritePaper.SecId);
                    });
                }
                else
                {
                    _historyPrices.TryAdd(favoritePaper.SecId,
                        HistoryEngine.GetHistoryPrices(favoritePaper.SecId).OrderBy(x => x.When).ToList());
                    FillMinMax(favoritePaper.SecId);
                }

                ExternalImport.LastPrice(Logger, favoritePaper)
                    .ContinueWith(pp =>
                    {
                        _syncContext.Send(paperPriceObj =>
                        {
                            var paperPrice = paperPriceObj as PaperPrice;

                            var secId = paperPrice.SecId;
                            var lastPrice = paperPrice.Price;
                            var lvi = lvPapers.FindItemWithText(secId);
                            if (lvi != null)
                            {
                                lvi.SubItems[1].Text = lastPrice.ToString("N2");
                                lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                            }
                            else
                            {
                                
                            }
                        }, pp.Result);
                    });
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void DrawGraph(string secId)
        {
            if (string.IsNullOrWhiteSpace(secId) || !_historyPrices.ContainsKey(secId))
            {
                pnlGraph.Controls.Clear();
                return;
            }

            var historyPrices = _historyPrices[secId];

            var chart = new ZedGraphControl
            {
                Dock = DockStyle.Fill
            };

            var gp = chart.GraphPane;
            gp.Title.Text = secId;

            var yAxis = gp.YAxis;
            var xAxis = gp.XAxis;

            var list = new StockPointList();
            for (var i = 0; i < historyPrices.Count; i++)
            {
                var d = new XDate(historyPrices[i].When);
                var open = (double)historyPrices[i].OpenPrice;
                var close = (double) historyPrices[i].ClosePrice;
                var high = (double) historyPrices[i].HighPrice;
                var low = (double) historyPrices[i].LowPrice;
                var vol = (double) historyPrices[i].Volume;

                var p = new StockPt(d.XLDate, high, low, open, close, vol);

                list.Add(p);
            }

            var days = rbDay.Checked ? 1 : rbWeek.Checked ? 7 : 30;

            var lastPriceValues = historyPrices.TakeLast(days).ToList();
            var fromPrice = lastPriceValues.First();
            var toPrice = lastPriceValues.Last();

            var curve = gp.AddJapaneseCandleStick(string.Empty, list);
            curve.Stick.RisingFill = new Fill(Color.Green, Color.LightGreen);
            curve.Stick.FallingFill = new Fill(Color.Red, Color.IndianRed);

            yAxis.Title.Text = "Цена";
            yAxis.Scale.Min = (double) lastPriceValues.Min(x => x.LowPrice) * 0.95;
            yAxis.Scale.Max = (double) lastPriceValues.Max(x => x.HighPrice) * 1.05;

            xAxis.Title.Text = "Дата";
            xAxis.Type = AxisType.Date;
            xAxis.Scale.TextLabels = historyPrices.Select(x => x.When.ToShortDateString()).ToArray();
            xAxis.Scale.Min = fromPrice.When.ToOADate();
            xAxis.Scale.Max = toPrice.When.ToOADate();

            chart.IsAntiAlias = true;
            chart.IsShowPointValues = true;
            chart.AxisChange();

            pnlGraph.Controls.Clear();
            pnlGraph.Controls.Add(chart);
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
            _syncContext = SynchronizationContext.Current;

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
    }
}
