using System.Collections;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Common;
using Terminal.Gui;

namespace RfBondManagement.Main.UI
{
    public class BaseBondPaperItemView : BaseBondPaper, IListDataSource
    {
        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width)
        {
            throw new System.NotImplementedException();
        }

        public bool IsMarked(int item)
        {
            throw new System.NotImplementedException();
        }

        public void SetMark(int item, bool value)
        {
            throw new System.NotImplementedException();
        }

        public IList ToList()
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; }
    }
}