using System;
using System.Linq;
using System.Reflection.Metadata;

namespace RfBondManagement.Engine.Calculations
{
    public class BondCalculator : IBondCalculator
    {
        public BondCoupon NearestCoupon(BaseBondPaper bond, DateTime onDate)
        {
            var coupons = bond.Coupons.OrderBy(c => c.Date).ToList();
            var futureCoupons = coupons.Where(c => c.Date > onDate).ToList();

            if (0 == futureCoupons.Count)
            {
                return coupons.Last();
            }

            return futureCoupons.First();
        }

        public void StartCalculateIncome(BaseBondPaper bond, BondBuyAction buyAction, Settings settings, DateTime onDate, out decimal balance, out decimal incomePercent)
        {
            decimal buyPrice = (bond.BondPar * (buyAction.Price / 100) + buyAction.NKD) *
                               (1 + settings.Comissions / 100);

            decimal income = -buyPrice;

            balance = income + bond.BondPar;

            ContinueCalculateIncome(bond, settings, buyAction.Date, buyAction.Date, onDate, out balance, out incomePercent);
        }

        public void ContinueCalculateIncome(BaseBondPaper bond, Settings settings, DateTime buyDate , DateTime fromDate, DateTime onDate, out decimal balance, out decimal incomePercent)
        {
            var nextCoupon = NearestCoupon(bond, fromDate);

            decimal nkd;
            if (nextCoupon.Date < = onDate)
            {
                nkd = nextCoupon.Value;
            }
            else
            {
                
            }

            var nextDate = nextCoupon.Date > onDate ? onDate : nextCoupon.Date;

            var diffDays = (nextDate - fromDate).Days;

            var totalDays = (onDate - buyDate).Days;

            nkd *= settings.Tax / 100;
            balance += nkd;
            incomePercent = balance/bond.BondPar*100/((totalDays/365)/100)''
        }
    }
}