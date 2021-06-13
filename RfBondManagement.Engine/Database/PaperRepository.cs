using System;
using System.Collections.Generic;
using LiteDB;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class PaperRepository : IPaperRepository
    {
        protected IDatabaseLayer _db;
        protected ILiteCollection<AbstractPaper> _papers;

        public PaperRepository(IDatabaseLayer db)
        {
            _db = db;
            _papers = _db.Database.GetCollection<AbstractPaper>("papers");
            _papers.EnsureIndex(p => p.Id);
            _papers.EnsureIndex(p => p.SecId);
        }

        public IEnumerable<AbstractPaper> Get()
        {
            return _papers.FindAll();
        }

        public AbstractPaper Insert(AbstractPaper entity)
        {
            entity.Id = Guid.NewGuid();
            _papers.Insert(entity);
            
            return entity;
        }

        public void Update(AbstractPaper entity)
        {
            _papers.Update(entity);
        }

        public void Delete(AbstractPaper entity)
        {
            _papers.DeleteMany(e => e.Id == entity.Id);
        }
    }
}