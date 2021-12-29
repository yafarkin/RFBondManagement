using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioService : IPortfolioService
    {
        protected readonly IPortfolioPaperActionRepository _paperActionRepository;
        protected readonly IPortfolioMoneyActionRepository _moneyActionRepository;
        protected readonly IExternalImportFactory _importFactory;
        protected readonly IHistoryRepository _historyRepository;
        protected readonly ILogger _logger;

        protected IExternalImport _import;

        public PortfolioService(
            ILogger logger,
            IExternalImportFactory importFactory,
            IPortfolioMoneyActionRepository moneyActionRepository,
            IPortfolioPaperActionRepository paperActionRepository,
            IHistoryRepository historyRepository
            )
        {
            _logger = logger;
            _importFactory = importFactory;
            _moneyActionRepository = moneyActionRepository;
            _paperActionRepository = paperActionRepository;
            _historyRepository = historyRepository;
        }

        public void Configure(Portfolio portfolio, ExternalImportType importType)
        {
            _import = _importFactory.GetImpl(importType);

            _paperActionRepository.Setup(portfolio.Id);
            _moneyActionRepository.Setup(portfolio.Id);
        }

        public void ApplyActions(PortfolioAction action)
        {
            if (action is PortfolioMoneyAction moneyAction)
            {
                _moneyActionRepository.Insert(moneyAction);
            }
            else if (action is PortfolioPaperAction paperAction)
            {
                _paperActionRepository.Insert(paperAction);
            }
            else
            {
                throw new NotSupportedException($"Не известный тип действия: {action.GetType()}");
            }
        }

        public void ApplyActions(IEnumerable<PortfolioAction> actions)
        {
            foreach (var action in actions)
            {
                ApplyActions(action);
            }
        }

        public async Task GetPrice(PortfolioAggregatedContent portfolioAggregatedContent, DateTime? onDate = null)
        {
            foreach (var paper in portfolioAggregatedContent.Papers)
            {
                var marketPrice = await GetPrice(paper.Paper, onDate);
                paper.MarketPrice = marketPrice;
            }
        }

        public async Task<decimal> GetPrice(AbstractPaper paper, DateTime? onDate = null)
        {
            decimal result;

            if (onDate.HasValue)
            {
                var historyPrice = _historyRepository.GetNearHistoryPriceOnDate(paper.SecId, onDate.Value);

                if (null == historyPrice)
                {
                    var importHistoryPrice = await _import.HistoryPrice(_logger, paper, onDate, onDate);
                    result = importHistoryPrice.FirstOrDefault()?.LegalClosePrice ?? 0;
                }
                else
                {
                    result = historyPrice.LegalClosePrice;
                }
            }
            else
            {
                var lastPrice = await _import.LastPrice(_logger, paper);
                result = lastPrice.Price;
            }

            return result;
        }
    }
}