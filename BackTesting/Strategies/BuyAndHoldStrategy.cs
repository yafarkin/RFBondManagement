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
        protected readonly IPortfolioService _portfolioService;
        protected readonly IPortfolioCalculator _portfolioCalculator;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IAdviser _adviser;
        protected ExternalImportType _importType;

        public BuyAndHoldStrategy(
            ILogger logger,
            IHistoryRepository historyRepository,
            IBondCalculator bondCalculator,
            IPortfolioService portfolioService,
            IPortfolioCalculator portfolioCalculator,
            IPortfolioBuilder portfolioBuilder,
            IPaperRepository paperRepository,
            IAdviserFactory adviserFactory
            )
            : base(logger, historyRepository, bondCalculator)
        {
            _paperRepository = paperRepository;
            
            _portfolioService = portfolioService;
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

        public Portfolio Configure(bool useVaMethod, bool reinvestIncome, decimal initialSum, decimal monthlyIncome, decimal tax, decimal commission, PortfolioStructureLeaf rootLeaf, ExternalImportType importType)
        {
            _useVaMethod = useVaMethod;
            _reinvestIncome = reinvestIncome;
            _initialSum = initialSum;
            _monthlyIncome = monthlyIncome;
            _importType = importType;

            _portfolio = new Portfolio
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

            _flattenPapers = FlattenPaper(rootLeaf).ToDictionary(x => x.Paper.SecId);

            return _portfolio;
        }

        public override IEnumerable<string> Papers => _flattenPapers.Select(x => x.Key);

        public override string Description => "Buy and hold" + (_reinvestIncome ? " (with reinvest)" : string.Empty);

        public override void Init(Portfolio portfolio, DateTime date)
        {
            _portfolio = portfolio;
            _nextMonthlyIncome = date.AddMonths(1);

            _portfolioService.Configure(portfolio, _importType);
            _portfolioCalculator.Configure(portfolio);

            _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_initialSum, MoneyActionType.IncomeExternal, "Начальная сумма", null, date));
        }

        public override async Task<bool> Process(DateTime date)
        {
            if (_monthlyIncome > 0 && date >= _nextMonthlyIncome)
            {
                _nextMonthlyIncome = _nextMonthlyIncome.AddMonths(1);

                _portfolioService.ApplyActions(_portfolioCalculator.MoveMoney(_monthlyIncome, MoneyActionType.IncomeExternal, "Ежемесячное пополнение", null, date));
                _logger.Info($"Monthly income, {_monthlyIncome:C}");
            }

            var content = _portfolioBuilder.Build(_portfolio.Id, date);

            if (_reinvestIncome || 0 == content.Papers.Count)
            {
                var p = new Dictionary<string, string>();

                if (_useVaMethod)
                {
                    var incomeTotal = content.TotalIncome;
                    p.Add(Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume, incomeTotal.ToString());
                }
                else
                {
                    p.Add(Constants.Adviser.BuyAndHold.P_AvailSum, content.AvailSum.ToString());
                }

                var actions = await _adviser.Advise(_portfolio, _importType, p);
                _portfolioService.ApplyActions(actions);
            }

            var statistic = _portfolioBuilder.FillStatistic(_portfolio.Id, date);
            var portfolioCost = statistic.PortfolioCost;
            _logger.Info($"Portfolio cost on {date} is {portfolioCost:C}; free sum: {content.AvailSum:C}");

            return true;
        }
    }
}