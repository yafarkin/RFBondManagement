using System;
using System.Collections.Generic;
using LiteDB;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IDatabaseLayer : IDisposable
    {
        public ILiteDatabase Database { get; }

        Settings LoadSettings();
        void SaveSettings(Settings settings);

        [Obsolete]
        IEnumerable<BaseStockPaperInPortfolio<BaseStockPaper>> GetPapersInPortfolio();
    }
}