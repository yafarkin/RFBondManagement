using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class DatabaseLayer : IDatabaseLayer
    {
        protected ILiteDatabase _database;

        protected ILiteCollection<Settings> _settingsSet;
        protected ILiteCollection<BaseStockPaper> _papersList;
        protected ILiteCollection<BaseBondPaperInPortfolio> _bonds;

        public DatabaseLayer()
        {
            _database = new LiteDatabase("bondmanagement.db");
            _settingsSet = _database.GetCollection<Settings>("settings");
            _papersList = _database.GetCollection<BaseStockPaper>("papersList");
            _bonds = _database.GetCollection<BaseBondPaperInPortfolio>("bondsInPortfolio");
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

        public IEnumerable<BaseStockPaperInPortfolio<BaseBondPaper>> GetPapersInPortfolio()
        {
            return _bonds.FindAll();
        }

        public IEnumerable<BaseStockPaper> SelectPapers()
        {
            return _papersList.FindAll();
        }

        public BaseStockPaper InsertPaper(BaseStockPaper paper)
        {
            paper.Id = Guid.NewGuid();
            _papersList.Insert(paper);
            return paper;
        }

        public void UpdatePaper(BaseStockPaper paper)
        {
            _papersList.Update(paper);
        }

        public void DeletePaper(Guid id)
        {
            _papersList.DeleteMany(x => x.Id == id);
        }

        public void Dispose()
        {
            _database?.Dispose();
            _database = null;
        }
    }
}