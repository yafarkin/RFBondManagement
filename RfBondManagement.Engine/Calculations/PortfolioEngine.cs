using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioEngine
    {
        protected readonly Portfolio _portfolio;
        protected readonly IPaperRepository _paperRepository;
        protected readonly IPortfolioPaperActionRepository _paperActionRepository;
        protected readonly IPortfolioMoneyActionRepository _moneyActionRepository;

        protected readonly IExternalImport _import;
        protected readonly IBondCalculator _bondCalculator;

        protected readonly ILogger _logger;

        public PortfolioEngine(Portfolio portfolio, IExternalImport import, IPaperRepository paperRepository, IPortfolioMoneyActionRepository moneyActionRepository, IPortfolioPaperActionRepository paperActionRepository, IBondCalculator bondCalculator, ILogger logger)
        {
            _logger = logger;
            _portfolio = portfolio;
            _import = import;
            _paperRepository = paperRepository;
            _moneyActionRepository = moneyActionRepository;
            _paperActionRepository = paperActionRepository;
            _bondCalculator = bondCalculator;
        }

        public IPaperInPortfolio<AbstractPaper> BuildPaperInPortfolio(AbstractPaper paper, IEnumerable<PortfolioAction> allActions, DateTime? onDate = null)
        {
            long count;

            IPaperInPortfolio<AbstractPaper> paperInPortfolio;
            if (paper.PaperType == PaperType.Bond)
            {
                var bondInPortfolio = new BondInPortfolio(paper as BondPaper);
                bondInPortfolio.Aci = _bondCalculator.CalculateAci(bondInPortfolio.Paper, onDate ?? DateTime.Today);
                paperInPortfolio = bondInPortfolio;
            }
            else
            {
                paperInPortfolio = new ShareInPortfolio(paper as SharePaper);
            }

            paperInPortfolio.OnDate = onDate;

            var fifo = new List<Tuple<PortfolioPaperAction, PortfolioPaperAction, long>>();

            var paperActions = allActions.OfType<PortfolioPaperAction>().Where(a => a.SecId == paper.SecId);
            if (onDate.HasValue)
            {
                paperActions = paperActions.Where(a => a.When <= onDate);
            }

            paperInPortfolio.Actions = paperActions.ToList();
            foreach (var paperAction in paperInPortfolio.Actions)
            {
                var isBuy = paperAction.PaperAction == PaperActionType.Buy;
                count = paperAction.Count;

                if (isBuy)
                {
                    fifo.Add(new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(paperAction, null, paperAction.Count));
                }
                else
                {
                    for (var i = 0; i < fifo.Count; i++)
                    {
                        var t = fifo[i];
                        if (0 == t.Item3)
                        {
                            // all already sold, skip
                            continue;
                        }

                        if (t.Item3 >= count)
                        {
                            t = new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(t.Item1, paperAction, t.Item3 - count);
                            fifo[i] = t;
                            break;
                        }

                        count -= t.Item3;
                        t = new Tuple<PortfolioPaperAction, PortfolioPaperAction, long>(t.Item1, paperAction, 0);
                        fifo[i] = t;
                    }
                }
            }

            var sum = 0m;
            count = 0;
            foreach (var t in fifo)
            {
                if (0 == t.Item3)
                {
                    continue;
                }

                count += t.Item3;
                sum += t.Item3 * t.Item1.Value;
            }

            paperInPortfolio.FifoActions = fifo;
            paperInPortfolio.Count = count;
            paperInPortfolio.AveragePrice = 0 == count ? 0 : sum / count;


            return paperInPortfolio;
        }

        public async Task FillPrice(PortfolioAggregatedContent portfolioAggregatedContent, DateTime? onDate = null)
        {
            foreach (var paper in portfolioAggregatedContent.Papers)
            {
                decimal marketPrice;
                if (onDate.HasValue)
                {
                    var historyPrice = await _import.HistoryPrice(_logger, paper.Paper, onDate, onDate);
                    marketPrice = historyPrice.FirstOrDefault()?.LegalClosePrice ?? 0;
                }
                else
                {
                    var lastPrice = await _import.LastPrice(_logger, paper.Paper);
                    marketPrice = lastPrice.Price;
                }

                paper.MarketPrice = marketPrice;
            }
        }

        public PortfolioAggregatedContent Build(DateTime? onDate = null)
        {
            var sums = new Dictionary<MoneyActionType, decimal>();
            var moneyPortfolioActions = _moneyActionRepository.Get();
            if (onDate.HasValue)
            {
                moneyPortfolioActions = moneyPortfolioActions.Where(a => a.When <= onDate);
            }

            foreach (var moneyAction in moneyPortfolioActions)
            {
                if (!sums.ContainsKey(moneyAction.MoneyAction))
                {
                    sums.Add(moneyAction.MoneyAction, moneyAction.Sum);
                }
                else
                {
                    sums[moneyAction.MoneyAction] += moneyAction.Sum;
                }
            }

            var paperPortfolioActions = _paperActionRepository.Get().ToList();
            if (onDate.HasValue)
            {
                paperPortfolioActions = paperPortfolioActions.Where(a => a.When <= onDate).ToList();
            }

            var papers = new Dictionary<string, IPaperInPortfolio<AbstractPaper>>();
            var uniqueSecIds = paperPortfolioActions.Select(a => a.SecId).Distinct();
            foreach (var secId in uniqueSecIds)
            {
                var paperDefinition = _paperRepository.Get().Single(p => p.SecId == secId);
                var paperInPortfolio = BuildPaperInPortfolio(paperDefinition, paperPortfolioActions, onDate);

                papers.Add(secId, paperInPortfolio);
            }

            var content = new PortfolioAggregatedContent
            {
                Papers = new ReadOnlyCollection<IPaperInPortfolio<AbstractPaper>>(papers.Select(x => x.Value).ToList()),
                Sums = new ReadOnlyDictionary<MoneyActionType, decimal>(sums)
            };

            return content;
        }

        public PortfolioMoneyAction MoveMoney(decimal sum, MoneyActionType moneyActionType, string comment, string secId = null, DateTime when = default(DateTime))
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var moneyAction = new PortfolioMoneyAction
            {
                MoneyAction = moneyActionType,
                PortfolioId = _portfolio.Id,
                When = when,
                SecId = secId,
                Sum = sum,
                Comment = comment
            };
            _moneyActionRepository.Insert(moneyAction);
            return moneyAction;
        }

        public PortfolioPaperAction BuyPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime))
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var sum = paper.PaperType == PaperType.Bond ? price / 100 * paper.FaceValue * count : price * count;

            MoveMoney(sum, MoneyActionType.OutcomeBuyOnMarket, $"Покупка бумаги {paper.SecId}, количество {count}, цена {price}", paper.SecId, when);

            if (paper.PaperType == PaperType.Bond)
            {
                var aci = _bondCalculator.CalculateAci(paper as BondPaper, when);
                var aciSum = aci * count;
                sum += aciSum;

                MoveMoney(aciSum, MoneyActionType.OutcomeAci, $"НКД {aci}, сумма НКД {aciSum}", paper.SecId, when);
            }

            var commission = sum * _portfolio.Commissions / 100;
            MoveMoney(commission, MoneyActionType.OutcomeCommission, $"Списание комиссии, ставка {_portfolio.Commissions:P}", paper.SecId, when);

            var paperAction = new PortfolioPaperAction
            {
                Count = count,
                PaperAction = PaperActionType.Buy,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                Sum = sum,
                Value = price,
                When = when
            };
            _paperActionRepository.Insert(paperAction);
            return paperAction;
        }

        protected static int CalcFullYears(DateTime dt1, DateTime dt2)
        {
            if (dt2.Year <= dt1.Year)
            {
                return 0;
            }

            var n = dt2.Year - dt1.Year;
            if (dt1.DayOfYear > dt2.DayOfYear)
            {
                --n;
            }

            return n;
        }

        public PortfolioPaperAction SellPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime))
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var paperInPortfolio = BuildPaperInPortfolio(paper, _paperActionRepository.Get(), when);
            if (paperInPortfolio.Count < count)
            {
                // may be later, I will implement 'short' functionality
                throw new InvalidOperationException("Нельзя продать большее количество, чем есть в портфеле");
            }

            var sum = paper.PaperType == PaperType.Bond ? price / 100 * paper.FaceValue * count : price * count;

            MoveMoney(sum, MoneyActionType.IncomeSellOnMarket, $"Продажа бумаги {paper.SecId}, количество {count}, цена {price}", paper.SecId, when);

            if (paper.PaperType == PaperType.Bond)
            {
                var aci = _bondCalculator.CalculateAci(paper as BondPaper, when);
                var aciSum = aci * count;

                sum += aciSum;

                MoveMoney(aciSum, MoneyActionType.IncomeAci, $"НКД {aci}, сумма НКД {aciSum}", paper.SecId, when);
            }

            var commission = sum * _portfolio.Commissions / 100;
            MoveMoney(commission, MoneyActionType.OutcomeCommission, $"Списание комиссии, ставка {_portfolio.Commissions:P}", paper.SecId, when);

            var today = DateTime.Today;
            var threeYears = new DateTime(2014, 1, 1);
            var fiveYears = new DateTime(2011, 1, 1);

            var profit = 0m;
            var needCount = count;
            foreach (var fifoAction in paperInPortfolio.FifoActions.Where(a => a.Item3 > 0))
            {
                var fifoActionProfit = 0m;

                if (fifoAction.Item3 >= needCount)
                {
                    fifoActionProfit = needCount * (price - fifoAction.Item1.Value);
                    needCount = 0;
                }
                else
                {
                    fifoActionProfit = fifoAction.Item3 * (price - fifoAction.Item1.Value);
                    needCount -= fifoAction.Item3;
                }

                // реализация логики на долгосрочное владение
                // https://bcs.ru/blog/lgoty-dlya-investorov
                if (_portfolio.LongTermBenefit && fifoActionProfit > 0)
                {
                    if (fifoAction.Item1.When >= fiveYears && fifoAction.Item1.When < threeYears)
                    {
                        var minusFive = today.AddYears(-5);
                        if (fifoAction.Item1.When <= minusFive)
                        {
                            fifoActionProfit = 0;
                        }
                    }
                    else if (fifoAction.Item1.When >= threeYears)
                    {
                        var minusThree = today.AddYears(-3);
                        if (fifoAction.Item1.When <= minusThree)
                        {
                            var totalFullYears = CalcFullYears(today, fifoAction.Item1.When);
                            var maxProfit = totalFullYears * 3_000_000m;
                            if (fifoActionProfit > maxProfit)
                            {
                                fifoActionProfit -= maxProfit;
                            }
                            else
                            {
                                fifoActionProfit = 0;
                            }
                        }
                    }
                }

                profit += fifoActionProfit;

                if (0 == needCount)
                {
                    break;
                }
            }

            if (paper.PaperType == PaperType.Bond)
            {
                profit = profit / 100 * paper.FaceValue;
            }

            if (profit > 0)
            {
                var delayTaxSum = profit * _portfolio.Tax / 100;
                MoveMoney(delayTaxSum, MoneyActionType.OutcomeDelayTax,
                    $"Отложенный налог, ставка {_portfolio.Tax:P}; разница покупка/продажа: {profit}", paper.SecId,
                    when);
            }

            var paperAction = new PortfolioPaperAction
                {
                    Count = count,
                    PaperAction = PaperActionType.Sell,
                    PortfolioId = _portfolio.Id,
                    SecId = paper.SecId,
                    Sum = sum,
                    Value = price,
                    When = when
                };
                _paperActionRepository.Insert(paperAction);

            return paperAction;
        }
    }
}