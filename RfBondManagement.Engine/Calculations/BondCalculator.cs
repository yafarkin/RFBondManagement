using System;
using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Calculations
{
    public class BondCalculator : IBondCalculator
    {
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
                if (bondIncomeInfo.ExpectedPositiveDate < bii.ExpectedPositiveDate)
                {
                    bondIncomeInfo.ExpectedPositiveDate = bii.ExpectedPositiveDate;
                }
                realIncomePercent.Add(bii.RealIncomePercent);
                avgSellPrice.Add(bii.SellPrice * buyAction.Count);
            }

            bondIncomeInfo.RealIncomePercent = realIncomePercent.Average();
            bondIncomeInfo.SellPrice = avgSellPrice.Average();
        }

        public void StartCalculateIncome(BondIncomeInfo bondIncomeInfo, BondBuyAction buyAction, Settings settings, DateTime toDate)
        {
            var nkd = buyAction.NKD;

            decimal buyPrice = (bondIncomeInfo.PaperInPortfolio.BondPaper.BondPar * (bondIncomeInfo.PaperInPortfolio.AvgBuySum / 100) + nkd) *
                               (1 + (settings?.Comissions / 100 ?? 0));

            decimal balance = -buyPrice + bondIncomeInfo.PaperInPortfolio.BondPaper.BondPar;

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

                bondIncomeInfo.RealIncomePercent =  balance / buyAction.BondPaper.BondPar * 100 / (totalDays / 365m);

                if (fromDate == buyAction.BondPaper.MaturityDate)
                {
                    bondIncomeInfo.SellPrice = buyAction.BondPaper.BondPar;
                    bondIncomeInfo.CloseByMaturityDate = true;

                    balance += buyAction.BondPaper.BondPar;
                }
                else
                {
                    var sellPrice = buyAction.BondPaper.BondPar * bondIncomeInfo.SellPrice / 100;
                    if (bondIncomeInfo.SellPrice > buyAction.Price)
                    {
                        var sellTax = (bondIncomeInfo.SellPrice - buyAction.Price) *
                                   buyAction.BondPaper.BondPar *
                                   (settings?.Tax / 100 ?? 1);
                        sellPrice -= sellTax;
                        bondIncomeInfo.SellTax = sellTax;
                    }

                    bondIncomeInfo.SellPrice = sellPrice;
                    balance += sellPrice;
                }

                return;
            }

            var nextCoupon = NextNearestCoupon(buyAction.BondPaper, fromDate);

            decimal nkd;
            if (nextCoupon.Date <= toDate)
            {
                nkd = nextCoupon.Value;
            }
            else
            {
                var nextFutureCoupon = NextFutureCoupon(buyAction.BondPaper, toDate);
                var daysBetweenCoupons = (nextFutureCoupon.Date - nextCoupon.Date).Days;
                var diffDays = (nextFutureCoupon.Date - toDate).Days;
                nkd = (decimal)diffDays / daysBetweenCoupons * nextFutureCoupon.Value;
            }

            var nextDate = nextCoupon.Date;

            var tax = nkd * (settings?.Tax / 100m ?? 0);
            var couponSum = nkd - tax;
            balance += couponSum;

            if (balance > 0 && bondIncomeInfo.ExpectedPositiveDate == default(DateTime))
            {
                bondIncomeInfo.ExpectedPositiveDate = fromDate;
            }

            bondIncomeInfo.IncomeByCoupons += couponSum;

            ContinueCalculateIncome(bondIncomeInfo, buyAction, settings, nextDate, toDate, ref balance);
        }

        protected BondCoupon NextNearestCoupon(BaseBondPaper bond, DateTime toDate)
        {
            var coupons = bond.Coupons.Where(c => c.Date > toDate).ToList();

            return 0 == coupons.Count ? bond.Coupons.Last() : coupons.First();
        }

        protected BondCoupon NextFutureCoupon(BaseBondPaper bond, DateTime toDate)
        {
            var coupons = bond.Coupons.Where(c => c.Date <= toDate).ToList();

            return 0 == coupons.Count ? bond.Coupons.Last() : coupons.Last();
        }
    }
}