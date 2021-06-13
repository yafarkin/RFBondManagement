﻿using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PortfolioPaperActionRepository : BasePortfolioActionRepository<PortfolioPaperAction>, IPortfolioPaperActionRepository
    {
        public PortfolioPaperActionRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}