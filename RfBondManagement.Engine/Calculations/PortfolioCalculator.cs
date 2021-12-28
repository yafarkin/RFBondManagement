using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class PortfolioCalculator : IPortfolioCalculator
    {
        protected readonly IPaperRepository _paperRepository;
        protected readonly ISplitRepository _splitRepository;
        protected readonly IBondCalculator _bondCalculator;
        protected readonly IPortfolioBuilder _portfolioBuilder;
        protected readonly IPortfolioActions _portfolioActions;

        protected readonly ILogger _logger;

        protected Portfolio _portfolio;
        public Portfolio Portfolio => _portfolio;

        public PortfolioCalculator(
            IPortfolioBuilder portfolioBuilder,
            IPortfolioActions portfolioActions,
            IPaperRepository paperRepository,
            ISplitRepository splitRepository,
            IBondCalculator bondCalculator,
            ILogger logger)
        {
            _logger = logger;
            _portfolioBuilder = portfolioBuilder;
            _portfolioActions = portfolioActions;
            _paperRepository = paperRepository;
            _splitRepository = splitRepository;
            _bondCalculator = bondCalculator;
        }

        public void Configure(Portfolio portfolio)
        {
            _portfolio = portfolio;
        }

        protected void CheckIsConfigured()
        {
            if (null == _portfolio)
            {
                throw new Exception("Portfolio calculator is not configured");
            }
        }

        public IEnumerable<PortfolioAction> PayTaxByDraftProfit(decimal draftSum, string comment = null, DateTime when = default(DateTime))
        {
            CheckIsConfigured();

            if (draftSum <= 0)
            {
                yield break;
            }

            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var tax = _portfolio.Tax;
            var taxSum = draftSum * tax / 100;

            comment ??= $"Оплата налога с суммы {draftSum:N4}, ставка {tax}%, сумма налога: {taxSum:N4}";

            var actions = MoveMoney(taxSum, MoneyActionType.OutcomeTax, comment, null, when);
            foreach (var action in actions)
            {
                yield return action;
            }

            actions = MoveMoney(-draftSum, MoneyActionType.DraftProfit, "Уменьшение налогооблагаемой суммы, т.к. налог по ней выплачен", null, when);
            foreach (var action in actions)
            {
                yield return action;
            }
        }

        public IEnumerable<PortfolioAction> MoveMoney(decimal sum, MoneyActionType moneyActionType, string comment, string secId = null, DateTime when = default(DateTime))
        {
            if (0 == sum)
            {
                yield break;
            }

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

            yield return moneyAction;
        }

        public IEnumerable<PortfolioAction> BuyPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime))
        {
            CheckIsConfigured();

            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var sum = paper.PaperType == PaperType.Bond ? price / 100 * paper.FaceValue * count : price * count;

            var actions = MoveMoney(sum, MoneyActionType.OutcomeBuyOnMarket, $"Покупка бумаги {paper.SecId}, количество {count}, цена {price}", paper.SecId, when);
            foreach (var action in actions)
            {
                yield return action;
            }

            if (paper.PaperType == PaperType.Bond)
            {
                var aci = _bondCalculator.CalculateAci(paper as BondPaper, when);
                var aciSum = aci * count;
                sum += aciSum;

                actions = MoveMoney(aciSum, MoneyActionType.OutcomeAci, $"НКД {aci}, сумма НКД {aciSum}", paper.SecId, when);
                foreach (var action in actions)
                {
                    yield return action;
                }
            }

            var commission = sum * _portfolio.Commissions / 100;
            actions = MoveMoney(commission, MoneyActionType.OutcomeCommission, $"Списание комиссии, ставка {_portfolio.Commissions/100:P}", paper.SecId, when);
            foreach (var action in actions)
            {
                yield return action;
            }

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
            
            yield return paperAction;
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

        public IEnumerable<PortfolioAction> SellPaper(AbstractPaper paper, long count, decimal price, DateTime when = default(DateTime))
        {
            CheckIsConfigured();

            return SellPaper(paper, count, price, when, PaperActionType.Sell);
        }

        protected IEnumerable<PortfolioAction> SellPaper(AbstractPaper paper, long count, decimal price, DateTime when, PaperActionType sellActionType)
        {
            if (when == default(DateTime))
            {
                when = DateTime.UtcNow;
            }

            var paperInPortfolio = _portfolioBuilder.BuildPaperInPortfolio(paper, _portfolioActions.PaperActions(_portfolio.Id), when);

            if (-1 == count)
            {
                // закрываем позицию
                count = paperInPortfolio.Count;
            }
            else if (paperInPortfolio.Count < count)
            {
                // может быть позднее, когда будет делаться реализация функционала шорта
                throw new InvalidOperationException("Нельзя продать большее количество, чем есть в портфеле");
            }

            var sum = paper.PaperType == PaperType.Bond ? price / 100 * paper.FaceValue * count : price * count;

            var actions = MoveMoney(sum, MoneyActionType.IncomeSellOnMarket, $"Продажа бумаги {paper.SecId}, количество {count}, цена {price}", paper.SecId, when);
            foreach (var action in actions)
            {
                yield return action;
            }

            if (paper.PaperType == PaperType.Bond)
            {
                var aci = _bondCalculator.CalculateAci(paper as BondPaper, when);
                var aciSum = aci * count;

                sum += aciSum;

                actions = MoveMoney(aciSum, MoneyActionType.IncomeAci, $"НКД {aci}, сумма НКД {aciSum}", paper.SecId, when);
                foreach (var action in actions)
                {
                    yield return action;
                }
            }

            var commission = sum * _portfolio.Commissions / 100;
            actions = MoveMoney(commission, MoneyActionType.OutcomeCommission, $"Списание комиссии, ставка {_portfolio.Commissions/100:P}", paper.SecId, when);
            foreach (var action in actions)
            {
                yield return action;
            }

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

            actions = MoveMoney(profit, MoneyActionType.DraftProfit, "Прибыль/убыток по сделке", paper.SecId, when);
            foreach (var action in actions)
            {
                yield return action;
            }

            var paperAction = new PortfolioPaperAction
            {
                Count = count,
                PaperAction = sellActionType,
                PortfolioId = _portfolio.Id,
                SecId = paper.SecId,
                Value = price,
                When = when
            };

            yield return paperAction;
        }

        public IEnumerable<PortfolioAction> Automate(DateTime onDate)
        {
            var result = new List<PortfolioAction>();

            var paperActionSecIds = _portfolioActions.PaperActions(_portfolio.Id).Select(p => p.SecId);
            var moneyActionSecIds = _portfolioActions.MoneyActions(_portfolio.Id).Select(p => p.SecId);
            var paperSecIds = paperActionSecIds.Concat(moneyActionSecIds).Distinct().ToList();

            result.AddRange(AutomateSplit(onDate, paperSecIds));
            result.AddRange(AutomateDividend(onDate, paperSecIds));
            result.AddRange(AutomateCoupons(onDate, paperSecIds));
            result.AddRange(AutomateBondCloseDate(onDate, paperSecIds));

            return result;
        }

        public IEnumerable<PortfolioPaperAction> AutomateSplit(DateTime onDate, IList<string> paperSecIds)
        {
            CheckIsConfigured();

            var splits = _splitRepository.Get().Where(s => s.Date.Date == onDate.Date).ToList();
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
                var multiplier = splits.Single(s => s.Date.Date == onDate.Date && s.SecId == secId).Multiplier;

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
            CheckIsConfigured();

            var papers = _paperRepository.Get()
                .Where(p => paperSecIds.Contains(p.SecId) && p.PaperType == PaperType.Share)
                .OfType<SharePaper>()
                .Where(p => p.Dividends?.Count > 0 && p.Dividends.Any(d => d.RegistryCloseDate == onDate))
                .ToList();

            var paperActions = new Lazy<List<PortfolioPaperAction>>(() => _portfolioActions.PaperActions(_portfolio.Id).ToList());

            foreach (var sharePaper in papers)
            {
                var dividendRecord = sharePaper.Dividends.Single(d => d.RegistryCloseDate.Date == onDate.Date);

                var paperInPortfolio = _portfolioBuilder.BuildPaperInPortfolio(sharePaper, paperActions.Value, onDate);

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

                var actions = MoveMoney(totalSum, MoneyActionType.IncomeDividend, $"Дивиденды по {sharePaper.SecId}, дивиденд: {dividendSum:N4}, налог: {taxSum:N2}, итого на бумагу: {(dividendSum - taxSum):N2}");
                foreach (var action in actions)
                {
                    yield return action;
                }

                actions = MoveMoney(totalTaxSum, MoneyActionType.OutcomeTax, $"Итого налог по дивидендам");
                foreach (var action in actions)
                {
                    yield return action;
                }
            }
        }

        public IEnumerable<PortfolioAction> AutomateCoupons(DateTime onDate, IList<string> paperSecIds)
        {
            CheckIsConfigured();

            var papers = _paperRepository.Get()
                .Where(p => paperSecIds.Contains(p.SecId) && p.PaperType == PaperType.Bond)
                .OfType<BondPaper>()
                .Where(p => p.Coupons?.Count > 0 && p.Coupons.Any(c => c.CouponDate.Date == onDate.Date))
                .ToList();

            var paperActions = new Lazy<List<PortfolioPaperAction>>(() => _portfolioActions.PaperActions(_portfolio.Id).ToList());

            foreach (var bondPaper in papers)
            {
                var paperInPortfolio = _portfolioBuilder.BuildPaperInPortfolio(bondPaper, paperActions.Value, onDate);
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

                var actions = MoveMoney(totalSum, MoneyActionType.IncomeDividend, $"Купоны по {bondPaper.SecId}, размер: {couponSum:N4}, налог: {taxSum:N2}, итого на бумагу: {(couponSum - taxSum):N2}");
                foreach (var action in actions)
                {
                    yield return action;
                }

                actions = MoveMoney(totalTaxSum, MoneyActionType.OutcomeTax, $"Итого налог по купонам");
                foreach (var action in actions)
                {
                    yield return action;
                }
            }
        }

        public IEnumerable<PortfolioAction> AutomateBondCloseDate(DateTime onDate, IList<string> paperSecIds)
        {
            CheckIsConfigured();

            var papers = _paperRepository.Get()
                .Where(p => paperSecIds.Contains(p.SecId) && p.PaperType == PaperType.Bond)
                .OfType<BondPaper>()
                .Where(p => p.MatDate.Date == onDate.Date)
                .ToList();

            foreach (var bondPaper in papers)
            {
                var actions = SellPaper(bondPaper, -1, 100, onDate, PaperActionType.Close);
                foreach (var action in actions)
                {
                    yield return action;
                }
            }
        }
    }
}