using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IExternalImport
    {
        Task<AbstractPaper> ImportPaper(ILogger logger, string secId);

        Task<PaperPrice> LastPrice(ILogger logger, AbstractPaper paper);

        Task<IEnumerable<HistoryPrice>> HistoryPrice(ILogger logger, AbstractPaper paper, DateTime? startDate = null, DateTime? endDate = null);

        Task<IEnumerable<string>> ListPapers(ILogger logger);
    }
}