using System;
using System.Collections;
using Terminal.Gui;

namespace RfBondManagement.Main.Windows
{
    public class ListPaperView : View
    {
        public Func<IListDataSource> PaperSource;

        protected ListView _allPapersView;

        public ListPaperView()
        {
            InitControls();
            InitStyle();
        }

        private void InitStyle()
        {
            X = 0;
            Y = 0;

            Width = Dim.Fill();
            Height = Dim.Fill();
        }

        public void DataBind()
        {
            _allPapersView.Source = PaperSource();
        }

        public void InitControls()
        {

            var headerLabel = new Label(0, 0, "Список бумаг:");
            Add(headerLabel);

            //var allPaperListView = new ListView(new Rect(4, 8, top.Frame.Width, 200), MovieDataSource.GetList(forKidsOnly.Checked, 0).ToList());
            _allPapersView = new ListView(new Rect(0, 1, 10, 10), (IListDataSource)null);
            Add(_allPapersView);
        }
    }
}