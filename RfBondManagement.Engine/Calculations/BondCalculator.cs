using System;
using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class BondCalculator : IBondCalculator
    {
        public decimal CalculateNkd(BaseBondPaper paper, DateTime toDate)
        {
            var nextCoupon = NearFutureCoupon(paper, toDate);
            if (nextCoupon.Date == toDate)
            {
                return 0;
            }

            var prevCoupon = NearPastCoupon(paper, toDate);
            var daysBetweenCoupons = (nextCoupon.Date - prevCoupon.Date).Days;
            var diffDays = (toDate - prevCoupon.Date).Days;
            var nkd = (decimal)diffDays / daysBetweenCoupons * nextCoupon.Value;
            return nkd;
        }

        public void CalculateIncome(BondIncomeInfo bondIncomeInfo, Settings settings, DateTime toDate)
        {
            var buyActions = bondIncomeInfo.PaperInPortfolio.Actions.OfType<BondBuyAction>();
            var realIncomePercent = new List<decimal>();
            var avgSellPrice = new List<decimal>();
            foreach (var buyAction in buyActions)
            {
                var bii = new BondIncomeInfo
                {
                    PaperInPortfolio = bondIncomeInfo.PaperInPortfolio,
                    SellPrice = bondIncomeInfo.SellPrice
                };

                StartCalculateIncome(bii, buyAction, settings, toDate);

                bondIncomeInfo.CloseByMaturityDate = bii.CloseByMaturityDate;
                bondIncomeInfo.BalanceOnBuy += bii.BalanceOnBuy * buyAction.Count;
                bondIncomeInfo.IncomeByCoupons += bii.IncomeByCoupons * buyAction.Count;
                bondIncomeInfo.SellTax += bii.SellTax * buyAction.Count;
                if (bondIncomeInfo.BreakevenDate < bii.BreakevenDate)
                {
                    bondIncomeInfo.BreakevenDate = bii.BreakevenDate;
                }
                realIncomePercent.Add(bii.RealIncomePercent);
                avgSellPrice.Add(bii.SellPrice * buyAction.Count);
            }

            bondIncomeInfo.RealIncomePercent = realIncomePercent.Average();
            bondIncomeInfo.SellPrice = avgSellPrice.Average();
        }

        public void StartCalculateIncome(BondIncomeInfo bondIncomeInfo, BondBuyAction buyAction, Settings settings, DateTime toDate)
        {
            var nkd = buyAction.Nkd;

            decimal buyPrice = (bondIncomeInfo.PaperInPortfolio.Paper.BondPar * (bondIncomeInfo.PaperInPortfolio.AvgBuySum / 100) + nkd) *
                               (1 + (settings?.Commissions / 100 ?? 0));

            decimal balance = -buyPrice + bondIncomeInfo.PaperInPortfolio.Paper.BondPar;

            bondIncomeInfo.BalanceOnBuy = buyPrice;

            ContinueCalculateIncome(bondIncomeInfo, buyAction, settings, buyAction.Date, toDate, ref balance);
        }

        protected void ContinueCalculateIncome(BondIncomeInfo bondIncomeInfo, BondBuyAction buyAction, Settings settings, DateTime fromDate, DateTime toDate, ref decimal balance)
        {
            if (fromDate >= toDate)
            {
                var totalDays = (toDate - buyAction.Date).Days;
                if (0 == totalDays)
                {
                    totalDays = 1;
                }

                bondIncomeInfo.RealIncomePercent =  balance / buyAction.Paper.BondPar * 100 / (totalDays / 365m);

                if (fromDate == buyAction.Paper.MaturityDate)
                {
                    bondIncomeInfo.SellPrice = buyAction.Paper.BondPar;
                    bondIncomeInfo.CloseByMaturityDate = true;

                    balance += buyAction.Paper.BondPar;
                }
                else
                {
                    var sellPrice = buyAction.Paper.BondPar * bondIncomeInfo.SellPrice / 100;
                    if (bondIncomeInfo.SellPrice > buyAction.Price)
                    {
                        var sellTax = (bondIncomeInfo.SellPrice - buyAction.Price) *
                                   buyAction.Paper.BondPar *
                                   (settings?.Tax / 100 ?? 1);
                        sellPrice -= sellTax;
                        bondIncomeInfo.SellTax = sellTax;
                    }

                    bondIncomeInfo.SellPrice = sellPrice;
                    balance += sellPrice;
                }

                return;
            }

            var nextCoupon = NearFutureCoupon(buyAction.Paper, fromDate);

            var nkd = nextCoupon.Date <= toDate ? nextCoupon.Value : CalculateNkd(buyAction.Paper, toDate);

            var nextDate = nextCoupon.Date;

            var tax = nkd * (settings?.Tax / 100m ?? 0);
            var couponSum = nkd - tax;
            balance += couponSum;

            if (balance > 0 && bondIncomeInfo.BreakevenDate == default(DateTime))
            {
                bondIncomeInfo.BreakevenDate = fromDate;
            }

            bondIncomeInfo.IncomeByCoupons += couponSum;

            ContinueCalculateIncome(bondIncomeInfo, buyAction, settings, nextDate, toDate, ref balance);
        }

        protected BondCoupon NearFutureCoupon(BaseBondPaper bond, DateTime toDate)
        {
            var coupons = bond.Coupons.Where(c => c.Date > toDate).ToList();

            return 0 == coupons.Count ? bond.Coupons.Last() : coupons.First();
        }

        protected BondCoupon NearPastCoupon(BaseBondPaper bond, DateTime toDate)
        {
            var coupons = bond.Coupons.Where(c => c.Date < toDate).ToList();

            return 0 == coupons.Count ? bond.Coupons.Last() : coupons.Last();
        }
    }
}