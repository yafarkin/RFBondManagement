﻿using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PaperRepository : BaseSecRepository<AbstractPaper>, IPaperRepository
    {
        protected override string _collectionName => "papers";

        public PaperRepository(IDatabaseLayer db)
            : base(db)
        {
        }
    }
}