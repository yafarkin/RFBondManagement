using System;
using System.Collections.Generic;
using LiteDB;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PortfolioActions : IPortfolioActions
    {
        protected IDatabaseLayer _db;
        protected ILiteCollection<PortfolioMoneyAction> _moneyEntities;
        protected ILiteCollection<PortfolioPaperAction> _paperEntities;

        public PortfolioActions(IDatabaseLayer db)
        {
            _db = db;
            
            _moneyEntities = _db.Database.GetCollection<PortfolioMoneyAction>("moneyActions");
            _moneyEntities.EnsureIndex(p => p.Id);

            _paperEntities = _db.Database.GetCollection<PortfolioPaperAction>("paperActions");
            _paperEntities.EnsureIndex(p => p.Id);
        }

        public IEnumerable<PortfolioMoneyAction> MoneyActions(Guid portfolioId)
        {
            return _moneyEntities.Find(x => x.PortfolioId == portfolioId);
        }

        public IEnumerable<PortfolioPaperAction> PaperActions(Guid portfolioId)
        {
            return _paperEntities.Find(x => x.PortfolioId == portfolioId);
        }
    }
}