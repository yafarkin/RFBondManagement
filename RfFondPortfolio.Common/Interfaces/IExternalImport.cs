using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IExternalImport
    {
        Task<AbstractPaper> ImportPaper(string secId);

        Task<PaperPrice> LastPrice(AbstractPaper paper);

        Task<IEnumerable<HistoryPrice>> HistoryPrice(AbstractPaper paper, DateTime? startDate = null, DateTime? endDate = null);

        Task<IEnumerable<string>> ListPapers();
    }
}