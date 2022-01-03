using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace BackTesting.Strategies
{
    public class BuyAndHoldStrategy : BaseEmptyStrategy
    {
        protected bool _reinvestIncome;
        protected decimal _initialSum;
        protected decimal _monthlyIncome;

        /// <summary>
        /// Use Value Averaging (VA) method instead Dollar Cost Avergaing (DCA)
        /// </summary>
        /// <remarks>see https://investprofit.info/value-averaging/</remarks>
        protected bool _useVaMethod;

        protected DateTime _nextMonthlyIncome;

        protected Dictionary<string, PortfolioStructureLeafPaper> _flattenPapers;
        protected Dictionary<string, AbstractPaper> _papers;

        protected readonly IPaperRepository _paperRepository;
        protected readonly IPortfolioCalculator _portfolioCalculator;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IAdviser _adviser;

        protected IPortfolioService _portfolioService;

        public BuyAndHoldStrategy(
            ILogger logger,
            IHistoryRepository historyRepository,
            IBondCalculator bondCalculator,
            IPortfolioCalculator portfolioCalculator,
            IPortfolioBuilder portfolioBuilder,
            IPaperRepository paperRepository,
            IAdviserFactory adviserFactory
            )
            : base(logger, historyRepository, bondCalculator)
        {
            _paperRepository = paperRepository;
            
            _portfolioCalculator = portfolioCalculator;
            _portfolioBuilder = portfolioBuilder;

            _adviser = adviserFactory.GetAdviser(_useVaMethod
                ? Constants.Adviser.BuyAndHoldWithVA.Name
                : Constants.Adviser.BuyAndHold.Name);
        }

        protected IList<PortfolioStructureLeafPaper> FlattenPaper(PortfolioStructureLeaf leaf)
        {
            var result = leaf.Papers != null && leaf.Papers.Count > 0
                ? new List<PortfolioStructureLeafPaper>(leaf.Papers)
                : new List<PortfolioStructureLeafPaper>();

            if (leaf.Children != null)
            {
                foreach (var child in leaf.Children)
                {
                    result.AddRange(FlattenPaper(child));
                }
            }

            return result;
        }

        public Portfolio CreateTestPortfolio(bool useVaMethod, bool reinvestIncome, decimal initialSum, decimal monthlyIncome, decimal tax, decimal commission, PortfolioStructureLeaf rootLeaf)
        {
            _useVaMethod = useVaMethod;
            _reinvestIncome = reinvestIncome;
            _initialSum = initialSum;
            _monthlyIncome = monthlyIncome;

            var portfolio = new Portfolio
            {
                Id = Guid.NewGuid(),
                Tax = tax,
                Commissions = commission,
                LongTermBenefit = true,
                Actual = true,
                Name = $"BackTesting {DateTime.Now}",
                RootLeaf = rootLeaf
            };
            
            var secIds = _historyRepository.Get().Select(x => x.SecId).Distinct().ToHashSet();
            _papers = _paperRepository.Get().Where(p => secIds.Contains(p.SecId)).ToDictionary(p => p.SecId);

            _flattenPapers = FlattenPaper(rootLeaf).ToDictionary(x => x.SecId);

            return portfolio;
        }

        public override IEnumerable<string> Papers => _flattenPapers.Select(x => x.Key);

        public override string Description => "Buy and hold" + (_reinvestIncome ? " (with reinvest)" : string.Empty);

        public override void Init(IPortfolioService portfolioService, DateTime date)
        {
            _nextMonthlyIncome = date.AddMonths(1);

            _portfolioService = portfolioService;
            _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_portfolioService.Portfolio, _initialSum, MoneyActionType.IncomeExternal, "Начальная сумма", null, date));
        }

        public override async Task<bool> Process(DateTime date)
        {
            if (_monthlyIncome > 0 && date >= _nextMonthlyIncome)
            {
                _nextMonthlyIncome = _nextMonthlyIncome.AddMonths(1);

                _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_portfolioService.Portfolio, _monthlyIncome, MoneyActionType.IncomeExternal, "Ежемесячное пополнение", null, date));
                _logger.Info($"Monthly income, {_monthlyIncome:C}");
            }

            var content = _portfolioBuilder.Build(_portfolioService.Portfolio.Id, date);

            if (_reinvestIncome || 0 == content.Papers.Count)
            {
                var p = new Dictionary<string, string>();

                if (_useVaMethod)
                {
                    p.Add(Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume, content.TotalIncome.ToString());
                }
                else
                {
                    p.Add(Constants.Adviser.BuyAndHold.P_AvailSum, content.AvailSum.ToString());
                }

                p.Add(Constants.Adviser.P_OnDate, date.ToString());

                var actions = await _adviser.Advise(_portfolioService, p);
                _portfolioService.ApplyActions(actions);
            }

            var statistic = _portfolioBuilder.FillStatistic(_portfolioService.Portfolio.Id, date);
            var portfolioCost = statistic.PortfolioCost;
            _logger.Info($"Portfolio cost on {date} is {portfolioCost:C}; free sum: {content.AvailSum:C}");

            return true;
        }
    }
}