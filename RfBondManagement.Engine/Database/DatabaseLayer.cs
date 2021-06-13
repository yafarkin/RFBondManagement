using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class DatabaseLayer : IDatabaseLayer
    {
        public ILiteDatabase Database { get; protected set; }

        protected ILiteCollection<Settings> _settingsSet;

        public DatabaseLayer()
        {
            Database = new LiteDatabase("bondmanagement.db");
            _settingsSet = Database.GetCollection<Settings>("settings");
        }

        public Settings LoadSettings()
        {
            var result = _settingsSet.FindAll().FirstOrDefault();
            if (null == result)
            {
                result = new Settings();
                _settingsSet.Insert(result);
            }

            return result;
        }

        public void SaveSettings(Settings settings)
        {
            _settingsSet.DeleteAll();
            _settingsSet.Insert(settings);
        }

        public IEnumerable<BaseStockPaperInPortfolio<BaseStockPaper>> GetPapersInPortfolio()
        {
            return new BaseStockPaperInPortfolio<BaseStockPaper>[0];
        }

        public void Dispose()
        {
            Database?.Dispose();
            Database = null;
        }
    }
}