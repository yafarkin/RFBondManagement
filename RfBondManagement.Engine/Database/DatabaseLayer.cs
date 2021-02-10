using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace RfBondManagement.Engine.Database
{
    public class DatabaseLayer
    {
        protected LiteDatabase _database;

        protected ILiteCollection<Settings> _settingsSet;
        protected ILiteCollection<BaseStockPaper> _papers;

        public DatabaseLayer()
        {
            _database = new LiteDatabase("bondmanagement.db");
            _settingsSet = _database.GetCollection<Settings>("settings");
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

        public IEnumerable<BaseStockPaper> GetPapers()
        {
            return _papers.FindAll();
        }
    }
}