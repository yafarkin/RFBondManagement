using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using RfFondPortfolio.Integration.Moex.Requests;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfFondPortfolio.Integration.Moex
{
    public class MoexImport : IExternalImport
    {
        public async Task<AbstractPaper> ImportPaper(string secId)
        {
            var request = new MoexPaperDefinitionRequest(secId);
            var response = await request.Read();

            var paperType = StockPaperConverter.GetPaperType(response);
            if (paperType == PaperType.Unknown)
            {
                throw new InvalidOperationException($"Неизвестный тип бумаги: {secId}");
            }

            AbstractPaper result;
            switch (paperType)
            {
                case PaperType.Share:
                    var divRequest = new MoexDividendsRequest(secId);
                    var divResponse = await divRequest.Read();
                    result = StockPaperConverter.MapShare(response, divResponse);
                    break;
                case PaperType.Bond:
                    var couponRequest = new MoexBondCouponsRequest(secId);
                    var couponResponse = await couponRequest.Read();
                    result = StockPaperConverter.MapBond(response, couponResponse);
                    break;
                default:
                    throw new InvalidOperationException($"Нет конвертора {paperType} для бумаги {secId}");
            }

            return result;

        }

        public async Task<PaperPrice> LastPrice(AbstractPaper paper)
        {
            var request = new MoexLastPriceRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            var response = await request.Read();

            if (null == response.Securities || 0 == response.Securities.Data.Count)
            {
                return null;
            }

            var result = new PaperPrice();

            result.Id = Guid.NewGuid();
            result.SecId = paper.SecId;
            result.When = DateTime.UtcNow;
            result.LotSize = response.Securities.GetDataForLong("SECID", paper.SecId, "LOTSIZE").GetValueOrDefault();
            result.Price = response.Securities.GetDataForDecimal("SECID", paper.SecId, "PREVADMITTEDQUOTE").GetValueOrDefault();

            return result;
        }

        public async Task<IEnumerable<HistoryPrice>> HistoryPrice(AbstractPaper paper, DateTime? startDate, DateTime? endDate)
        {
            var request = new MoexSecurityHistoryRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            var response = await request.CursorRead();
            if (null == response || 0 == response.Data.Count)
            {
                return null;
            }

            var result = new List<HistoryPrice>();
            foreach (var historyPrice in response.Data)
            {
                var history = new HistoryPrice();

                history.Id = Guid.NewGuid();
                history.SecId = historyPrice["SECID"];
                history.When = Convert.ToDateTime(historyPrice["TRADEDATE"]);
                history.NumTrades = Convert.ToInt64(historyPrice["NUMTRADES"], CultureInfo.InvariantCulture);
                history.Value = Convert.ToDecimal(historyPrice["VALUE"], CultureInfo.InvariantCulture);
                history.Volume = Convert.ToDecimal(historyPrice["VOLUME"], CultureInfo.InvariantCulture);
                history.OpenPrice = Convert.ToDecimal(historyPrice["OPEN"], CultureInfo.InvariantCulture);
                history.LowPrice = Convert.ToDecimal(historyPrice["LOW"], CultureInfo.InvariantCulture);
                history.HighPrice = Convert.ToDecimal(historyPrice["HIGH"], CultureInfo.InvariantCulture);
                history.ClosePrice = Convert.ToDecimal(historyPrice["CLOSE"], CultureInfo.InvariantCulture);
                history.LegalClosePrice = Convert.ToDecimal(historyPrice["LEGALCLOSEPRICE"], CultureInfo.InvariantCulture);

                result.Add(history);
            }

            return result;
        }

        public async Task<IEnumerable<string>> ListPapers()
        {
            throw new NotImplementedException();
        }
    }
}