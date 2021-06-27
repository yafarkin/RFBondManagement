using System;
using System.Collections.Generic;
using NUnit.Framework;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class BondCalculatorTests
    {
        public BondCalculator Calculator;
        public BondPaper BondPaper;
        public Portfolio Portfolio;

        [SetUp]
        public void Setup()
        {
            Portfolio = new Portfolio
            {
                Commissions = 0.061m,
                Tax = 13.0m
            };

            Calculator = new BondCalculator();

            BondPaper = new BondPaper
            {
                FaceValue = 1000,
                Name = "Test Bond",
                MatDate = new DateTime(2023, 8, 30),
                IssueDate = new DateTime(2020, 9, 2),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon {CouponDate = new DateTime(2021, 3, 3), Value = 50},
                    new BondCoupon {CouponDate = new DateTime(2021, 9, 1), Value = 50},
                    new BondCoupon {CouponDate = new DateTime(2022, 3, 2), Value = 50},
                    new BondCoupon {CouponDate = new DateTime(2022, 8, 31), Value = 50},
                    new BondCoupon {CouponDate = new DateTime(2023, 3, 1), Value = 50},
                    new BondCoupon {CouponDate = new DateTime(2023, 8, 30), Value = 50}
                }
            };
        }

        [Test]
        public void CouponOnCurrentDate_Test()
        {
            var today = DateTime.UtcNow.Date;

            BondPaper = new BondPaper
            {
                FaceValue = 1000,
                Name = "Test Bond",
                MatDate = today.AddDays(180),
                IssueDate = today,
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon {CouponDate = today, Value = 50},
                    new BondCoupon {CouponDate = today.AddDays(180), Value = 50},
                }
            };

            var aci = Calculator.CalculateAci(BondPaper, today);
            aci.ShouldBe(0);
        }

        [Test]
        public void TenPercents_Clear_Test()
        {
            var buyAction = new PortfolioPaperAction
            {
                Count = 1,
                When = new DateTime(2020, 9, 2),
                Value = 100,
                PaperAction = PaperActionType.Buy
            };

            var bondIncomeInfo = new BondIncomeInfo
            {
                BondInPortfolio = new BondInPortfolio(BondPaper)
                {
                    Actions = new List<PortfolioPaperAction> {buyAction},
                    AveragePrice = buyAction.Value
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, null, BondPaper.MatDate);
            Assert.IsTrue(bondIncomeInfo.CloseByMaturityDate);
            Assert.AreEqual(1000m, bondIncomeInfo.BalanceOnBuy);
            Assert.AreEqual(1300m, bondIncomeInfo.BalanceOnSell);
            Assert.AreEqual(300m, bondIncomeInfo.IncomeByCoupons);
            Assert.AreEqual(300m, bondIncomeInfo.ExpectedIncome);
            Assert.AreEqual(10.03m, Math.Round(bondIncomeInfo.RealIncomePercent, 2));
        }

        [Test]
        public void TenPercents_withSettings()
        {
            var buyAction = new PortfolioPaperAction
            {
                Count = 1,
                When = new DateTime(2020, 9, 2),
                Value = 100,
                PaperAction = PaperActionType.Buy
            };

            var bondIncomeInfo = new BondIncomeInfo
            {
                BondInPortfolio = new BondInPortfolio(BondPaper)
                {
                    Actions = new List<PortfolioPaperAction> {buyAction},
                    AveragePrice = buyAction.Value
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, Portfolio, BondPaper.MatDate);
            Assert.IsTrue(bondIncomeInfo.CloseByMaturityDate);
            Assert.AreEqual(1000.61m, bondIncomeInfo.BalanceOnBuy);
            Assert.AreEqual(1261m, bondIncomeInfo.BalanceOnSell);
            Assert.AreEqual(261m, bondIncomeInfo.IncomeByCoupons);
            Assert.AreEqual(260.39m, bondIncomeInfo.ExpectedIncome);
            Assert.AreEqual(8.7m, Math.Round(bondIncomeInfo.RealIncomePercent, 2));
        }

        [Test]
        public void CalculateAci_NoCoupons()
        {
            var paper = new BondPaper
            {
                Name = "ОФЗ 26223",
                FaceValue = 1000,
                MatDate = new DateTime(2024, 2, 28),
                Coupons = new List<BondCoupon>()
            };

            var aci = Calculator.CalculateAci(paper, DateTime.Now);
            aci.ShouldBe(0);
        }

        [Test]
        public void CalculateAci_AskDate_BeforeIssueDate()
        {
            var paper = new BondPaper
            {
                Name = "ОФЗ 26223",
                FaceValue = 1000,
                IssueDate = DateTime.UtcNow.Date,
                MatDate = new DateTime(2024, 2, 28),
                Coupons = new List<BondCoupon>()
            };

            var aci = Calculator.CalculateAci(paper, DateTime.UtcNow.Date.AddDays(-1));
            aci.ShouldBe(0);
        }

        [Test]
        public void CalculateAci_AskDate_AfterMatDate()
        {
            var paper = new BondPaper
            {
                Name = "ОФЗ 26223",
                FaceValue = 1000,
                MatDate = new DateTime(2024, 2, 28),
                Coupons = new List<BondCoupon>()
            };

            var aci = Calculator.CalculateAci(paper, paper.MatDate.AddDays(1));
            aci.ShouldBe(0);
        }

        [Test]
        public void RealBondPaper()
        {
            var realPaper = new BondPaper
            {
                Name = "ОФЗ 26223",
                FaceValue = 1000,
                MatDate = new DateTime(2024, 2, 28),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon {CouponDate = new DateTime(2018, 9, 5), Value = 34.9m},
                    new BondCoupon {CouponDate = new DateTime(2019, 3, 6), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2019, 9, 4), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2020, 3, 4), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2020, 9, 2), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2021, 3, 3), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2021, 9, 1), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2022, 3, 2), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2022, 8, 31), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2023, 3, 1), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2023, 8, 30), Value = 32.41m},
                    new BondCoupon {CouponDate = new DateTime(2024, 2, 28), Value = 32.41m}
                }
            };

            var buyAction = new PortfolioPaperAction
            {
                Count = 1,
                When = new DateTime(2021, 2, 4),
                Value = 103.75m,
                PaperAction = PaperActionType.Buy
            };

            var aci = Calculator.CalculateAci(realPaper, buyAction.When);
            aci.ShouldBe(27.6m);

            var bondIncomeInfo = new BondIncomeInfo
            {
                BondInPortfolio = new BondInPortfolio(realPaper)
                {
                    Actions = new List<PortfolioPaperAction> {buyAction},
                    AveragePrice = buyAction.Value
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, Portfolio, realPaper.MatDate);

            Assert.IsTrue(bondIncomeInfo.CloseByMaturityDate);
            Assert.AreEqual(1065.75m, Math.Round(bondIncomeInfo.BalanceOnBuy, 2));
            Assert.AreEqual(1197.38m, Math.Round(bondIncomeInfo.BalanceOnSell, 2));
            Assert.AreEqual(197.38m, Math.Round(bondIncomeInfo.IncomeByCoupons, 2));
            Assert.AreEqual(131.63m, Math.Round(bondIncomeInfo.ExpectedIncome, 2));
            Assert.AreEqual(4.29m, Math.Round(bondIncomeInfo.RealIncomePercent, 2));
        }
    }
}
