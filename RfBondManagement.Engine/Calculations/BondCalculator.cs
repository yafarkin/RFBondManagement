using System;
using System.Collections.Generic;
using System.Linq;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;

namespace RfBondManagement.Engine.Calculations
{
    public class BondCalculator : IBondCalculator
    {
        public decimal CalculateAci(BondPaper paper, DateTime toDate)
        {
            if (null == paper.Coupons || 0 == paper.Coupons.Count)
            {
                return 0;
            }

            if (toDate < paper.IssueDate || toDate > paper.MatDate)
            {
                return 0;
            }

            var nextCoupon = NearFutureCoupon(paper, toDate);
            if (nextCoupon.CouponDate == toDate)
            {
                return 0;
            }

            var prevCoupons = paper.Coupons.Where(c => c.CouponDate < toDate).ToList();
            var prevCoupon = 0 == prevCoupons.Count ? null : prevCoupons.Last();
            if (null == prevCoupon)
            {
                if (null == paper.IssueDate)
                {
                    throw new InvalidOperationException("Не возможно вычислить НКД, т.к. дата находится ранее самого первого купона");
                }

                if (paper.IssueDate == toDate)
                {
                    return 0;
                }

                prevCoupon = new BondCoupon
                {
                    CouponDate = paper.IssueDate.GetValueOrDefault()
                };
            }

            var daysBetweenCoupons = (nextCoupon.CouponDate - prevCoupon.CouponDate).Days;
            var diffDays = (toDate - prevCoupon.CouponDate).Days;
            var aci =Math.Round((decimal)diffDays / daysBetweenCoupons * nextCoupon.Value, 2);
            return aci;
        }

        public void CalculateIncome(BondIncomeInfo bondIncomeInfo, Portfolio portfolio, DateTime toDate)
        {
            var buyActions = bondIncomeInfo.BondInPortfolio.Actions.Where(a => a.PaperAction == PaperActionType.Buy);
            var realIncomePercent = new List<decimal>();
            var avgSellPrice = new List<decimal>();
            foreach (var buyAction in buyActions)
            {
                var bii = new BondIncomeInfo
                {
                    BondInPortfolio = bondIncomeInfo.BondInPortfolio,
                    SellPrice = bondIncomeInfo.SellPrice
                };

                StartCalculateIncome(bii, buyAction, portfolio, toDate);

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

        public void StartCalculateIncome(BondIncomeInfo bondIncomeInfo, PortfolioPaperAction buyAction, Portfolio portfolio, DateTime toDate)
        {
            var paper = bondIncomeInfo.BondInPortfolio.Paper;
            var aci = CalculateAci(paper, buyAction.When);

            var buyPrice = (paper.FaceValue * (bondIncomeInfo.BondInPortfolio.AveragePrice / 100) + aci) *
                           (1 + (portfolio?.Commissions / 100 ?? 0));

            var balance = -buyPrice + bondIncomeInfo.BondInPortfolio.Paper.FaceValue;

            bondIncomeInfo.BalanceOnBuy = buyPrice;

            ContinueCalculateIncome(bondIncomeInfo, buyAction, portfolio, buyAction.When, toDate, ref balance);
        }

        protected void ContinueCalculateIncome(BondIncomeInfo bondIncomeInfo, PortfolioPaperAction buyAction, Portfolio portfolio, DateTime fromDate, DateTime toDate, ref decimal balance)
        {
            var paper = bondIncomeInfo.BondInPortfolio.Paper;

            if (fromDate >= toDate)
            {
                var totalDays = (toDate - buyAction.When).Days;
                if (0 == totalDays)
                {
                    totalDays = 1;
                }

                bondIncomeInfo.RealIncomePercent =  balance / paper.FaceValue * 100 / (totalDays / 365m);

                if (fromDate == paper.MatDate)
                {
                    bondIncomeInfo.SellPrice = paper.FaceValue;
                    bondIncomeInfo.CloseByMaturityDate = true;

                    balance += paper.FaceValue;
                }
                else
                {
                    var sellPrice = paper.FaceValue * bondIncomeInfo.SellPrice / 100;
                    if (bondIncomeInfo.SellPrice > buyAction.Value)
                    {
                        var sellTax = (bondIncomeInfo.SellPrice - buyAction.Value) *
                                   paper.FaceValue *
                                   (portfolio?.Tax / 100 ?? 1);
                        sellPrice -= sellTax;
                        bondIncomeInfo.SellTax = sellTax;
                    }

                    bondIncomeInfo.SellPrice = sellPrice;
                    balance += sellPrice;
                }

                return;
            }

            var nextCoupon = NearFutureCoupon(paper, fromDate);

            var aci = nextCoupon.CouponDate<= toDate ? nextCoupon.Value : CalculateAci(paper, toDate);

            var nextDate = nextCoupon.CouponDate;

            var tax = aci * (portfolio?.Tax / 100m ?? 0);
            var couponSum = aci - tax;
            balance += couponSum;

            if (balance > 0 && bondIncomeInfo.BreakevenDate == default(DateTime))
            {
                bondIncomeInfo.BreakevenDate = fromDate;
            }

            bondIncomeInfo.IncomeByCoupons += couponSum;

            ContinueCalculateIncome(bondIncomeInfo, buyAction, portfolio, nextDate, toDate, ref balance);
        }

        protected BondCoupon NearFutureCoupon(BondPaper bond, DateTime toDate)
        {
            var coupons = bond.Coupons.Where(c => c.CouponDate > toDate).ToList();

            return 0 == coupons.Count ? bond.Coupons.Last() : coupons.First();
        }
    }
}