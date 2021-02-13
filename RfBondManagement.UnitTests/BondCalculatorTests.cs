using System;
using System.Collections.Generic;
using NUnit.Framework;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class BondCalculatorTests
    {
        public BondCalculator Calculator;
        public BaseBondPaper BondPaper;
        public Settings Settings;

        [SetUp]
        public void Setup()
        {
            Settings = new Settings
            {
                Comissions = 0.061m,
                Tax = 13.0m
            };

            Calculator = new BondCalculator();

            BondPaper = new BaseBondPaper
            {
                BondPar = 1000,
                Name = "Test Bond",
                Currency = "RUR",
                MaturityDate = new DateTime(2023, 8, 30),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 3, 3),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 9, 1),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 3, 2),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 8, 31),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 3, 1),
                        Value = 50
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 8, 30),
                        Value = 50
                    }
                }
            };
        }

        [Test]
        public void TenPercents_Clear()
        {
            var buyAction = new BondBuyAction
            {
                Count = 1,
                Date = new DateTime(2020, 9, 2),
                NKD = 0,
                Paper = BondPaper,
                Price = 100
            };

            var bondIncomeInfo = new BondIncomeInfo
            {
                PaperInPortfolio = new BaseBondPaperInPortfolio
                {
                    Actions = new List<BaseAction> {buyAction},
                    BondPaper = BondPaper
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, null, BondPaper.MaturityDate);
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
            var buyAction = new BondBuyAction
            {
                Count = 1,
                Date = new DateTime(2020, 9, 2),
                NKD = 0,
                Paper = BondPaper,
                Price = 100
            };

            var bondIncomeInfo = new BondIncomeInfo
            {
                PaperInPortfolio = new BaseBondPaperInPortfolio
                {
                    Actions = new List<BaseAction> {buyAction},
                    BondPaper = BondPaper
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, Settings, BondPaper.MaturityDate);
            Assert.IsTrue(bondIncomeInfo.CloseByMaturityDate);
            Assert.AreEqual(1000.61m, bondIncomeInfo.BalanceOnBuy);
            Assert.AreEqual(1261m, bondIncomeInfo.BalanceOnSell);
            Assert.AreEqual(261m, bondIncomeInfo.IncomeByCoupons);
            Assert.AreEqual(260.39m, bondIncomeInfo.ExpectedIncome);
            Assert.AreEqual(8.7m, Math.Round(bondIncomeInfo.RealIncomePercent, 2));
        }

        [Test]
        public void RealBondPaper()
        {
            var realPaper = new BaseBondPaper
            {
                Name = "ОФЗ 26223",
                BondPar = 1000,
                MaturityDate = new DateTime(2024, 2, 28),
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 3, 3),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 9, 1),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 3, 2),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 8, 31),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 3, 1),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 8, 30),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2024, 2, 28),
                        Value = 32.41m
                    },
                }
            };

            var buyAction = new BondBuyAction
            {
                Count = 1,
                Date = new DateTime(2021, 2, 4),
                NKD = 27.6m,
                Paper = realPaper,
                Price = 103.75m
            };

            var bondIncomeInfo = new BondIncomeInfo
            {
                PaperInPortfolio = new BaseBondPaperInPortfolio
                {
                    Actions = new List<BaseAction> { buyAction },
                    BondPaper = BondPaper
                }
            };

            Calculator.StartCalculateIncome(bondIncomeInfo, buyAction, Settings, realPaper.MaturityDate);

            Assert.IsTrue(bondIncomeInfo.CloseByMaturityDate);
            Assert.AreEqual(1065.75m, Math.Round(bondIncomeInfo.BalanceOnBuy, 2));
            Assert.AreEqual(1197.38m, Math.Round(bondIncomeInfo.BalanceOnSell, 2));
            Assert.AreEqual(197.38m, Math.Round(bondIncomeInfo.IncomeByCoupons, 2));
            Assert.AreEqual(131.63m, Math.Round(bondIncomeInfo.ExpectedIncome, 2));
            Assert.AreEqual(4.29m, Math.Round(bondIncomeInfo.RealIncomePercent, 2));
        }
    }
}
