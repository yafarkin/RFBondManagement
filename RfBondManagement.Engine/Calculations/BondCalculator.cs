using System;
using System.Linq;

namespace RfBondManagement.Engine.Calculations
{
    public class BondCalculator : IBondCalculator
    {
        public void StartCalculateIncome(BondIncomeInfo bondIncomeInfo, Settings settings, DateTime toDate)
        {
            decimal buyPrice = (bondIncomeInfo.BuyAction.BondPaper.BondPar * (bondIncomeInfo.BuyAction.Price / 100) + bondIncomeInfo.BuyAction.NKD) *
                               (1 + (settings?.Comissions / 100 ?? 0));

            decimal balance = -buyPrice + bondIncomeInfo.BuyAction.BondPaper.BondPar;

            bondIncomeInfo.BalanceOnBuy = buyPrice;

            ContinueCalculateIncome(bondIncomeInfo, settings, bondIncomeInfo.BuyAction.Date, bondIncomeInfo.BuyAction.Date, toDate, ref balance);
        }

        protected void ContinueCalculateIncome(BondIncomeInfo bondIncomeInfo, Settings settings, DateTime buyDate, DateTime fromDate, DateTime toDate, ref decimal balance)
        {
            if (fromDate >= toDate)
            {
                var totalDays = (toDate - buyDate).Days;
                bondIncomeInfo.RealIncomePercent =  balance / bondIncomeInfo.BuyAction.BondPaper.BondPar * 100 / (totalDays / 365m);

                if (fromDate == bondIncomeInfo.BuyAction.BondPaper.MaturityDate)
                {
                    bondIncomeInfo.SellPrice = bondIncomeInfo.BuyAction.BondPaper.BondPar;
                    bondIncomeInfo.CloseByMaturityDate = true;

                    balance += bondIncomeInfo.BuyAction.BondPaper.BondPar;
                }
                else
                {
                    var sellPrice = bondIncomeInfo.BuyAction.BondPaper.BondPar * bondIncomeInfo.SellPrice / 100;
                    if (bondIncomeInfo.SellPrice > bondIncomeInfo.BuyAction.Price)
                    {
                        var sellTax = (bondIncomeInfo.SellPrice - bondIncomeInfo.BuyAction.Price) *
                                   bondIncomeInfo.BuyAction.BondPaper.BondPar *
                                   (settings?.Tax / 100 ?? 1);
                        sellPrice -= sellTax;
                        bondIncomeInfo.SellTax = sellTax;
                    }

                    bondIncomeInfo.SellPrice = sellPrice;
                    balance += sellPrice;
                }

                return;
            }

            var nextCoupon = NextNearestCoupon(bondIncomeInfo.BuyAction.BondPaper, fromDate);

            decimal nkd;
            if (nextCoupon.Date <= toDate)
            {
                nkd = nextCoupon.Value;
            }
            else
            {
                var nextFutureCoupon = NextFutureCoupon(bondIncomeInfo.BuyAction.BondPaper, toDate);
                var daysBetweenCoupons = (nextFutureCoupon.Date - nextCoupon.Date).Days;
                var diffDays = (nextFutureCoupon.Date - toDate).Days;
                nkd = (decimal)diffDays / daysBetweenCoupons * nextFutureCoupon.Value;
            }

            var nextDate = nextCoupon.Date;

            var tax = nkd * (settings?.Tax / 100m ?? 0);
            var couponSum = nkd - tax;
            balance += couponSum;

            bondIncomeInfo.IncomeByCoupons += couponSum;

            ContinueCalculateIncome(bondIncomeInfo, settings, buyDate, nextDate, toDate, ref balance);
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