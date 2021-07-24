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
            var yAxis = gp.YAxis;
            var xAxis = gp.XAxis;

            gp.Title.Text = secId;
            gp.AddBar(
                string.Empty,
                Enumerable.Range(0, historyPrices.Count).Select(x => (double) x).ToArray(),
                historyPrices.Select(x => (double) x.ClosePrice).ToArray(),
                Color.Red);

            yAxis.Title.Text = "Y";
            yAxis.Scale.MinAuto = true;
            yAxis.Scale.MaxAuto = true;

            xAxis.Type = AxisType.Text;
            xAxis.Scale.TextLabels = historyPrices.Select(x => x.When.ToShortDateString()).ToArray();

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
                paper.IsFavorite = true;
                PaperRepository.Insert(paper);

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
