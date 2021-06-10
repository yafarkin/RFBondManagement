using System;
using System.Collections.Generic;
using RfFondPortfolio.Common.Dtos;

namespace RfFondPortfolio.Common.Interfaces
{
    public interface IExternalImport
    {
        AbstractPaper ImportPaper(string secId);

        PaperPrice LastPrice(string secId);

        IEnumerable<PaperPrice> HistoryPrice(string secId, DateTime? startDate, DateTime? endDate);

        IEnumerable<string> ListPapers();
    }
}