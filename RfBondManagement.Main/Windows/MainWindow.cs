using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Database;
using Terminal.Gui;

namespace RfBondManagement.Main.Windows
{
    public class MainWindow : Window
    {
        protected readonly DatabaseLayer _db;

        public MainWindow(DatabaseLayer db)
        {
            _db = db;

            InitControls();
            InitStyle();
        }

        protected void InitStyle()
        {
            Title = "Портфель облигаций";

            X = 0;
            Y = 1;

            Width = Dim.Fill();
            Height = Dim.Fill();
        }

        protected void InitControls()
        {
            var listPapers = new ListPaperView {PaperSource = () =>
            {
                //_db.GetPapers().ToList()
                var result = new List<BaseStockPaper>()
                {
                    new BaseBondPaper {BondPar = 1000, Currency = "RUR", Name = "test1"},
                    new BaseBondPaper {BondPar = 1000, Currency = "RUR", Name = "test2"},
                };

                IListDataSource

                return result;
            }
            };

            Add(listPapers);

        }
    }
}