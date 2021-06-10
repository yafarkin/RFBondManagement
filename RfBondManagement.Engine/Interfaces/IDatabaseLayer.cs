using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IDatabaseLayer : IDisposable
    {
        Settings LoadSettings();
        void SaveSettings(Settings settings);

        [Obsolete]
        IEnumerable<BaseStockPaperInPortfolio<BaseStockPaper>> GetPapersInPortfolio();

        IEnumerable<BaseStockPaper> SelectPapers();
        BaseStockPaper InsertPaper(BaseStockPaper paper);
        void UpdatePaper(BaseStockPaper paper);
        void DeletePaper(Guid id);

    }
}