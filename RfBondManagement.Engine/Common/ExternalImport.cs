using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Common
{
    public class ExternalImport : IExternalImport
    {
        protected static StaticCache<AbstractPaper> _papers = new StaticCache<AbstractPaper>(60 * 60 * 1000);
        protected static StaticCache<PaperPrice> _prices = new StaticCache<PaperPrice>(20 * 1000);
        protected static StaticCache<IEnumerable<string>> _paperList = new StaticCache<IEnumerable<string>>(24 * 60 * 60 * 1000);

        protected IExternalImport _impl { get; set; }

        internal ExternalImport(IExternalImport impl)
        {
            _impl = impl;
        }

        public async Task<AbstractPaper> ImportPaper(ILogger logger, string secId)
        {
            return await _papers.GetOrRetrieve(secId,
                () => _impl.ImportPaper(logger, secId));
        }

        public async Task<PaperPrice> LastPrice(ILogger logger, AbstractPaper paper)
        {
            return await _prices.GetOrRetrieve(paper.SecId,
                () => _impl.LastPrice(logger, paper));
        }

        public Task<IEnumerable<HistoryPrice>> HistoryPrice(ILogger logger, AbstractPaper paper, DateTime? startDate = null, DateTime? endDate = null)
        {
            return _impl.HistoryPrice(logger, paper, startDate, endDate);
        }

        public async Task<IEnumerable<string>> ListPapers(ILogger logger)
        {
            return await _paperList.GetOrRetrieve("_", () => _impl.ListPapers(logger));
        }
    }
}