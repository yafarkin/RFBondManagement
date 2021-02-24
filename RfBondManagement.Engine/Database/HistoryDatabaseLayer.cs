using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LiteDB;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class HistoryDatabaseLayer : IHistoryDatabaseLayer
    {
        protected ILiteDatabase _database;

        protected ILiteCollection<BaseStockPaper> _papers;
        protected ILiteCollection<HistoryPrice> _prices;

        public HistoryDatabaseLayer()
        {
            _database = new LiteDatabase("history.db");
            _papers = _database.GetCollection<BaseStockPaper>("papers");
            _prices = _database.GetCollection<HistoryPrice>("prices");

            _papers.EnsureIndex(p => p.Code);
            _prices.EnsureIndex(p => p.IndexCode);
        }

        public IList<BaseStockPaper> GetHistoryPapers()
        {
            return _papers.FindAll().ToList();
        }

        public IList<HistoryPrice> GetHistoryPrice(string paperCode)
        {

            return _prices.Find(p => p.PaperCode == paperCode).ToList();
        }

        public HistoryPrice GetHistoryPriceOnDate(string paperCode, DateTime date)
        {
            var indexCode = $"{paperCode}{date:yyyyMMdd}";
            return _prices.FindOne(p => p.IndexCode == indexCode);
        }

        public void AddPaper(BaseStockPaper paper)
        {
            if (null == _papers.FindOne(p => p.Code == paper.Code))
            {
                _papers.Insert(paper);
            }
        }

        public bool AddHistoryPrice(HistoryPrice historyPrice)
        {
            var hp = _prices.FindOne(p => p.IndexCode == historyPrice.IndexCode);
            if (null == hp)
            {
                _prices.Insert(historyPrice);
                return true;
            }
            else
            {
                hp.ClosePrice = historyPrice.ClosePrice;
                hp.HighPrice = historyPrice.HighPrice;
                hp.LowPrice = historyPrice.LowPrice;
                hp.OpenPrice = historyPrice.OpenPrice;
                hp.Volume = historyPrice.Volume;
                hp.Price = historyPrice.Price;
                _prices.Update(hp);
                return false;
            }
        }
    }
}