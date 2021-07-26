using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using RfBondManagement.Engine;
using RfBondManagement.WinForm.Forms;
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

        public WatchListUC()
        {
            InitializeComponent();
        }

        private void WatchListUC_Load(object sender, EventArgs e)
        {
            if (null == PaperRepository)
            {
                return;
            }

            DataBind();
        }

        public void DataBind()
        {
            var favoritePapers = PaperRepository.Get().Where(p => p.IsFavorite).ToList();

            var selectedSecId = 0 == lvPapers.SelectedItems.Count ? string.Empty : lvPapers.SelectedItems[0].Text;
            lvPapers.Items.Clear();

            foreach (var favoritePaper in favoritePapers)
            {
                var lastPriceDate = HistoryEngine.GetLastHistoryDate(favoritePaper.SecId);
                if (null == lastPriceDate || lastPriceDate > DateTime.Today.AddDays(-3))
                {
                    var t = HistoryEngine.ImportHistory(favoritePaper.SecId);
                }

                ExternalImport.LastPrice(Logger, favoritePaper);
                    //.ContinueWith(pp =>
                    //{
                    //    var secId = pp.Result.SecId;
                    //    var lastPrice = pp.Result.Price;
                    //    var i = lvPapers.Items.Find(secId, false);
                    //    if (1 == i.Length)
                    //    {
                    //        i[0].SubItems[0].Text = lastPrice.ToString("N2");
                    //    }
                    //});

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
            }

            lvPapers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void DrawGraph(string secId)
        {
            if (string.IsNullOrWhiteSpace(secId))
            {
                pnlGraph.Controls.Clear();
                return;
            }

            var historyPrices = HistoryEngine.GetHistoryPrices(secId).OrderBy(x => x.When).ToList();

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

            var lastPriceValues = historyPrices.TakeLast(60).ToList();
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
    }
}
