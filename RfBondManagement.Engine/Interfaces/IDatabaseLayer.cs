using System;
using System.Collections.Generic;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IDatabaseLayer : IDisposable
    {
        Settings LoadSettings();
        void SaveSettings(Settings settings);

        IEnumerable<BaseBondPaperInPortfolio> GetBondsInPortfolio();
        IEnumerable<BaseStockPaper> GetPapers();
    }
}