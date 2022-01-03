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

        protected Portfolio _portfolio;

        public Portfolio Portfolio => _portfolio;

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

        protected void CheckIsConfigured()
        {
            if (null == _portfolio)
            {
                throw new Exception("Portfolio service is not configured");
            }
        }

        public void Configure(Portfolio portfolio, ExternalImportType importType)
        {
            _portfolio = portfolio;
            _import = _importFactory.GetImpl(importType);

            _paperActionRepository.Setup(portfolio.Id);
            _moneyActionRepository.Setup(portfolio.Id);
        }

        public void ApplyActions(PortfolioAction action)
        {
            CheckIsConfigured();

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
                throw new NotSupportedException($"Неизвестный тип действия: {action.GetType()}");
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
            CheckIsConfigured();

            foreach (var paper in portfolioAggregatedContent.Papers)
            {
                var marketPrice = await GetPrice(paper.Paper, onDate);
                paper.MarketPrice = marketPrice;
            }
        }

        public async Task<decimal> GetPrice(AbstractPaper paper, DateTime? onDate = null)
        {
            CheckIsConfigured();

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