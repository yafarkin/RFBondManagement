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
        protected readonly ISplitRepository _splitRepository;

        protected readonly IExternalImport _import;
        protected readonly IBondCalculator _bondCalculator;

        protected readonly ILogger _logger;

        public PortfolioEngine(
            Portfolio portfolio,
            IExternalImport import,
            IPaperRepository paperRepository,
            IPortfolioMoneyActionRepository moneyActionRepository,
            IPortfolioPaperActionRepository paperActionRepository,
            ISplitRepository splitRepository,
            IBondCalculator bondCalculator, ILogger logger)
        {
            _logger = logger;
            _portfolio = portfolio;
            _import = import;
            _paperRepository = paperRepository;
            _moneyActionRepository = moneyActionRepository;
            _paperActionRepository = paperActionRepository;
            _splitRepository = splitRepository;
            _bondCalculator = bondCalculator;

            _paperActionRepository.Setup(_portfolio.Id);
            _moneyActionRepository.Setup(_portfolio.Id);
        }

        protected void PerformFifoSplit(decimal multiplier, IList<FifoAction> fifo)
        {
            if (1 == multiplier || 0 == fifo.Count)
            {
                return;
            }

            var lastFifo = fifo.Last();

            var buyPaperAction = new PortfolioPaperAction
            {
                Comment = lastFifo.BuyAction.Comment,
                Id = Guid.Empty,
                PaperAction = lastFifo.BuyAction.PaperAction,
                PortfolioId = lastFifo.BuyAction.PortfolioId,
                SecId = lastFifo.BuyAction.SecId,

                Count = Convert.ToInt64(Math.Floor(lastFifo.BuyAction.Count * multiplier)),
                Value = lastFifo.BuyAction.Value / multiplier,

                When = lastFifo.BuyAction.When
            };

            var newCount = Convert.ToInt64(Math.Floor(lastFifo.Count * multiplier));

            var updatedFifo = new FifoAction(buyPaperAction, lastFifo.SellAction, newCount);
            fifo.Remove(lastFifo);
            fifo.Add(updatedFifo);
        }

        /// <summary>
        /// Возвращает информацию о бумаге в портфеле
        /// </summary>
        /// <param name="paper">Бумага</param>
        /// <param name="allPaperActions">Все действия с бумагами в портфеле</param>
        /// <param name="onDate">Дата, на которую надо построить информацию, null - на текущий момент</param>
        /// <returns>Информация по бумаге</returns>
        public IPaperInPortfolio<AbstractPaper> BuildPaperInPortfolio(AbstractPaper paper, IEnumerable<PortfolioPaperAction> allPaperActions, DateTime? onDate = null)
        {
            long count;

            IPaperInPortfolio<AbstractPaper> paperInPortfolio;
            if (paper.PaperType == PaperType.Bond)
            {
                var bondInPortfolio = new BondInPortfolio(paper as BondPaper);
                bondInPortfolio.Aci = _bondCalculator.CalculateAci(bondInPortfolio.Paper, onDate ?? DateTime.UtcNow.Date);
                paperInPortfolio = bondInPortfolio;
            }
            else
            {
                paperInPortfolio = new ShareInPortfolio(paper as SharePaper);
            }

            paperInPortfolio.OnDate = onDate;

            var fifo = new List<FifoAction>();

            var paperActions = allPaperActions.Where(a => a.SecId == paper.SecId);
            if (onDate.HasValue)
            {
                paperActions = paperActions.Where(a => a.When <= onDate);
            }

            paperInPortfolio.Actions = paperActions.ToList();
            foreach (var paperAction in paperInPortfolio.Actions)
            {
                if (paperAction.PaperAction == PaperActionType.Split)
                {
                    PerformFifoSplit(paperAction.Value, fifo);
                    continue;
                }

                if (paperAction.PaperAction == PaperActionType.Coupon ||
                    paperAction.PaperAction == PaperActionType.Dividend)
                {
                    continue;
                }

                var isBuy = paperAction.PaperAction == PaperActionType.Buy;
                count = paperAction.Count;

                if (isBuy)
                {
                    fifo.Add(new FifoAction(paperAction, null, paperAction.Count));
                }
                else
                {
                    for (var i = 0; i < fifo.Count; i++)
                    {
                        var t = fifo[i];
                        if (0 == t.Count)
                        {
                            // all already sold, skip
                            continue;
                        }

                        if (t.Count >= count)
                        {
                            t = new FifoAction(t.BuyAction, paperAction, t.Count - count);
                            fifo[i] = t;
                            break;
                        }

                        count -= t.Count;
                        t = new FifoAction(t.BuyAction, paperAction, 0);
                        fifo[i] = t;
                    }
                }
            }

            var sum = 0m;
            count = 0;
            foreach (var t in fifo)
            {
                if (0 == t.Count)
                {
                    continue;
                }

                count += t.Count;
                sum += t.Count * t.BuyAction.Value;
            }

            paperInPortfolio.FifoActions = fifo;
            paperInPortfolio.Count = count;
            paperInPortfolio.AveragePrice = 0 == count ? 0 : Math.Round(sum / count, 2);

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

            var today = DateTime.UtcNow.Date;
            var threeYears = new DateTime(2014, 1, 1);
            var fiveYears = new DateTime(2011, 1, 1);

            var profit = 0m;
            var needCount = count;
            foreach (var fifoAction in paperInPortfolio.FifoActions.Where(a => a.Count > 0))
            {
                var fifoActionProfit = 0m;

                if (fifoAction.Count >= needCount)
                {
                    fifoActionProfit = needCount * (price - fifoAction.BuyAction.Value);
                    needCount = 0;
                }
                else
                {
                    fifoActionProfit = fifoAction.Count * (price - fifoAction.BuyAction.Value);
                    needCount -= fifoAction.Count;
                }

                // реализация логики на долгосрочное владение
                // https://bcs.ru/blog/lgoty-dlya-investorov
                if (_portfolio.LongTermBenefit && fifoActionProfit > 0)
                {
                    var buyWhen = fifoAction.BuyAction.When;

                    if (buyWhen >= fiveYears && buyWhen < threeYears)
                    {
                        var minusFive = today.AddYears(-5);
                        if (buyWhen <= minusFive)
                        {
                            fifoActionProfit = 0;
                        }
                    }
                    else if (buyWhen >= threeYears)
                    {
                        var minusThree = today.AddYears(-3);
                        if (buyWhen <= minusThree)
                        {
                            var totalFullYears = CalcFullYears(today, buyWhen);
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

        public IEnumerable<PortfolioAction> Automate(DateTime onDate)
        {
            var result = new List<PortfolioAction>();

            var paperSecIds = _paperActionRepository.Get().Select(p => p.SecId).Distinct().ToList();
            result.AddRange(AutomateSplit(onDate, paperSecIds));
            result.AddRange(AutomateDividend(onDate, paperSecIds));
            result.AddRange(AutomateCoupons(onDate, paperSecIds));

            return result;
        }

        public IEnumerable<PortfolioPaperAction> AutomateSplit(DateTime onDate, IList<string> paperSecIds)
        {
            var splits = _splitRepository.Get().Where(s => s.Date == onDate).ToList();
            if (!splits.Any())
            {
                yield break;
            }

            var secIds = paperSecIds.Intersect(splits.Select(s => s.SecId)).ToList();
            if (!secIds.Any())
            {
                yield break;
            }

            foreach (var secId in secIds)
            {
                var multiplier = splits.Single(s => s.Date == onDate && s.SecId == secId).Multiplier;

                _logger.Info($"Perform split for {secId}, multiplier {multiplier:N4}");

                var paperAction = new PortfolioPaperAction
                {
                    PaperAction = PaperActionType.Split,
                    PortfolioId = _portfolio.Id,
                    SecId = secId,
                    When = onDate.Date,
                    Value = multiplier,
                };

                yield return paperAction;
            }
        }

        public IEnumerable<PortfolioAction> AutomateDividend(DateTime onDate, IList<string> paperSecIds)
        {
            var papers = _paperRepository.Get()
                .Where(p => paperSecIds.Contains(p.SecId) && p.PaperType == PaperType.Share)
                .OfType<SharePaper>()
                .Where(p => p.Dividends?.Count > 0 && p.Dividends.Any(d => d.RegistryCloseDate == onDate))
                .ToList();

            var paperActions = new Lazy<List<PortfolioPaperAction>>(() => _paperActionRepository.Get().ToList());

            foreach (var sharePaper in papers)
            {
                var dividendRecord = sharePaper.Dividends.Single(d => d.RegistryCloseDate == onDate);

                var paperInPortfolio = BuildPaperInPortfolio(sharePaper, paperActions.Value, onDate);

                var dividendSum = dividendRecord.Value;
                var taxSum = dividendSum * _portfolio.Tax / 100;

                var totalTaxSum = taxSum * paperInPortfolio.Count;
                var totalSum = dividendSum * paperInPortfolio.Count;
                var incomeClearSum = totalSum - totalTaxSum;

                _logger.Info($"Income dividends from {sharePaper.SecId}, value {dividendSum:N4}, tax {taxSum:N2}, total clear sum: {incomeClearSum:N2}");

                var paperAction = new PortfolioPaperAction
                {
                    Count = paperInPortfolio.Count,
                    PaperAction = PaperActionType.Dividend,
                    PortfolioId = _portfolio.Id,
                    SecId = sharePaper.SecId,
                    When = onDate.Date,
                    Value = dividendSum,
                };

                yield return paperAction;

                yield return MoveMoney(totalSum, MoneyActionType.IncomeDividend, $"Дивиденды по {sharePaper.SecId}, дивиденд: {dividendSum:N4}, налог: {taxSum:N2}, итого на бумагу: {(dividendSum - taxSum):N2}");
                yield return MoveMoney(totalTaxSum, MoneyActionType.OutcomeTax, $"Итого налог по дивидендам");
            }
        }

        public IEnumerable<PortfolioAction> AutomateCoupons(DateTime onDate, IList<string> paperSecIds)
        {
            var papers = _paperRepository.Get()
                .Where(p => paperSecIds.Contains(p.SecId) && p.PaperType == PaperType.Bond)
                .OfType<BondPaper>()
                .Where(p => p.Coupons?.Count > 0 && p.Coupons.Any(c => c.CouponDate == onDate))
                .ToList();

            var paperActions = new Lazy<List<PortfolioPaperAction>>(() => _paperActionRepository.Get().ToList());

            foreach (var bondPaper in papers)
            {
                var paperInPortfolio = BuildPaperInPortfolio(bondPaper, paperActions.Value, onDate);
                var couponRecord = bondPaper.Coupons.Single(c => c.CouponDate == onDate);

                var couponSum = couponRecord.Value;

                var taxSum = couponSum * _portfolio.Tax / 100;

                var totalTaxSum = taxSum * paperInPortfolio.Count;
                var totalSum = couponSum * paperInPortfolio.Count;
                var incomeClearSum = totalSum - totalTaxSum;

                _logger.Info($"Income coupons from {bondPaper.SecId}, value {couponSum:N4}, tax {taxSum:N2}, total clear sum: {incomeClearSum:N2}");

                var paperAction = new PortfolioPaperAction
                {
                    Count = paperInPortfolio.Count,
                    PaperAction = PaperActionType.Coupon,
                    PortfolioId = _portfolio.Id,
                    SecId = bondPaper.SecId,
                    When = onDate.Date,
                    Value = couponSum,
                };

                yield return paperAction;

                yield return MoveMoney(totalSum, MoneyActionType.IncomeDividend, $"Купоны по {bondPaper.SecId}, размер: {couponSum:N4}, налог: {taxSum:N2}, итого на бумагу: {(couponSum - taxSum):N2}");
                yield return MoveMoney(totalTaxSum, MoneyActionType.OutcomeTax, $"Итого налог по купонам");
            }
        }
    }
}