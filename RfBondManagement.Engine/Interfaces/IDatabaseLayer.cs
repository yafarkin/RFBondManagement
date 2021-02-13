using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IDatabaseLayer
    {
        Settings LoadSettings();
        void SaveSettings(Settings settings);

        IEnumerable<BaseStockPaperInPortfolio> GetPapersInPortfolio();
        IEnumerable<BaseStockPaper> GetPapers();
    }
}