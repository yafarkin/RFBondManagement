using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfFondPortfolio.Common.Dtos;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestClass]
    public class BuyAdviserTests
    {
        [TestInitialize]
        public void Setup()
        {
            TestsHelper.Reset();
        }
        
        public Portfolio BuildSamplePortfolio()
        {
            var portfolio = new Portfolio
            {
                Id = Guid.NewGuid(),
                Commissions = 1,

                RootLeaf = new PortfolioStructureLeaf
                {
                    Children = new List<PortfolioStructureLeaf>
                    {
                        new PortfolioStructureLeaf
                        {
                            Name = "ChildLeaf1",
                            Volume = 100,
                            Papers = new List<PortfolioStructureLeafPaper>
                            {
                                new PortfolioStructureLeafPaper
                                {
                                    Paper = new SharePaper { SecId = "Paper1" },
                                    Volume = 100
                                },
                                new PortfolioStructureLeafPaper
                                {
                                    Paper = new SharePaper { SecId = "Paper2" },
                                    Volume = 200
                                }
                            }
                        },
                        new PortfolioStructureLeaf
                        {
                            Name = "ChildLeaf2",
                            Volume = 100,
                            Papers = new List<PortfolioStructureLeafPaper>
                            {
                                new PortfolioStructureLeafPaper
                                {
                                    Paper = new SharePaper { SecId = "Paper3" },
                                    Volume = 30
                                },
                                new PortfolioStructureLeafPaper
                                {
                                    Paper = new SharePaper { SecId = "Paper4" },
                                    Volume = 20
                                }
                            },
                            Children = new List<PortfolioStructureLeaf>
                            {
                                new PortfolioStructureLeaf
                                {
                                    Name = "ChildLeaf3",
                                    Volume = 50,
                                    Papers = new List<PortfolioStructureLeafPaper>
                                    {
                                        new PortfolioStructureLeafPaper
                                        {
                                            Paper = new SharePaper { SecId = "Paper5" },
                                            Volume = 100
                                        },
                                        new PortfolioStructureLeafPaper
                                        {
                                            Paper = new SharePaper { SecId = "Paper6" },
                                            Volume = 50
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return portfolio;
        }

        public Portfolio BuildSimplePortfolioAndBuy(long count1, long count2)
        {
            var portfolio = new Portfolio
            {
                Id = Guid.NewGuid(),
                Commissions = 0,
                RootLeaf = new PortfolioStructureLeaf
                {
                    Papers = new List<PortfolioStructureLeafPaper>
                    {
                        new PortfolioStructureLeafPaper
                        {
                            Paper = new SharePaper {SecId = "Paper1"},
                            Volume = 0.8m
                        },
                        new PortfolioStructureLeafPaper
                        {
                            Paper = new SharePaper {SecId = "Paper2"},
                            Volume = 0.2m
                        },
                    }
                }
            };

            for (var i = 0; i < 2; i++)
            {
                TestsHelper.Papers.Add(new SharePaper { SecId = $"Paper{i + 1}" });
            }

            if (!TestsHelper.LastPrices.ContainsKey("Paper1"))
            {
                TestsHelper.LastPrices.Add("Paper1", 20);
            }

            if (!TestsHelper.LastPrices.ContainsKey("Paper2"))
            {
                TestsHelper.LastPrices.Add("Paper2", 10);
            }

            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            if (count1 > 0)
            {
                service.ApplyActions(calculator.BuyPaper(new SharePaper { SecId = "Paper1" }, count1, TestsHelper.LastPrices["Paper1"]));
            }

            if (count2 > 0)
            {
                service.ApplyActions(calculator.BuyPaper(new SharePaper { SecId = "Paper2" }, count2, TestsHelper.LastPrices["Paper2"]));
            }

            return portfolio;
        }

        [TestMethod]
        public void FlattenStructure_CorrectPercentCalc_Test()
        {
            // проверяем, что структура схлопнется, и правильно рассчитаются необходимые веса по каждой бумаге
            var flattenList = BuyAdviser.FlattenPaperStructure(BuildSamplePortfolio().RootLeaf, 1);
            flattenList.Count.ShouldBe(6);

            flattenList[0].Paper.SecId.ShouldBe("Paper1");
            flattenList[0].Volume.ShouldBe(0.1666m, 0.0001m);

            flattenList[1].Paper.SecId.ShouldBe("Paper2");
            flattenList[1].Volume.ShouldBe(0.3333m, 0.0001m);

            flattenList[2].Paper.SecId.ShouldBe("Paper3");
            flattenList[2].Volume.ShouldBe(0.15m);

            flattenList[3].Paper.SecId.ShouldBe("Paper4");
            flattenList[3].Volume.ShouldBe(0.1m);

            flattenList[4].Paper.SecId.ShouldBe("Paper5");
            flattenList[4].Volume.ShouldBe(0.1666m, 0.0001m);

            flattenList[5].Paper.SecId.ShouldBe("Paper6");
            flattenList[5].Volume.ShouldBe(0.0833m, 0.0001m);
        }

        [TestMethod]
        public async Task Advice_BuyOnly_Test()
        {
            // проверяем, что пустой портфель правильно заполнится - в зависимости от веса бумаги в структуре и её цены
            var portfolio = BuildSamplePortfolio();

            for (var i = 0; i < 6; i++)
            {
                TestsHelper.Papers.Add(new SharePaper { SecId = $"Paper{i + 1}" });
            }

            TestsHelper.LastPrices.Add("Paper1", 100);
            TestsHelper.LastPrices.Add("Paper2", 200);
            TestsHelper.LastPrices.Add("Paper3", 30);
            TestsHelper.LastPrices.Add("Paper4", 20);
            TestsHelper.LastPrices.Add("Paper5", 100);
            TestsHelper.LastPrices.Add("Paper6", 50);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            var actions = await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum.ToString()}
            });
            actions.ShouldNotBeEmpty();

            service.ApplyActions(actions);

            var sum = actions.OfType<PortfolioMoneyAction>().Sum(x => x.Sum);
            sum.ShouldBe(99990);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(6);
            
            // т.к. при покупке ещё списывается комиссия, да и у бумаг разная цена, то в целом мы ожидаем
            // что вес бумаг в денежном выражении по отношению к общей сумме в целом будет соответствовать
            // тому весу, который мы видели в предыдущем тесте
            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / availSum).ShouldBe(0.16m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / availSum).ShouldBe(0.33m, 0.01m);
            (papers["Paper3"].Count * TestsHelper.LastPrices["Paper3"] / availSum).ShouldBe(0.15m, 0.01m);
            (papers["Paper4"].Count * TestsHelper.LastPrices["Paper4"] / availSum).ShouldBe(0.1m, 0.01m);
            (papers["Paper5"].Count * TestsHelper.LastPrices["Paper5"] / availSum).ShouldBe(0.165m, 0.01m);
            (papers["Paper6"].Count * TestsHelper.LastPrices["Paper6"] / availSum).ShouldBe(0.083m, 0.01m);
        }

        [TestMethod]
        public async Task Buy_NoSell_Test()
        {
            // в портфеле заведен баланс на одну бумагу, что она занимает весь портфель
            // но мы уже до этого успели купить другую бумагу. тест проверяет, что 
            // мы на всю переданную сумму купим указанную в структуре бумагу, не продавая ранее купленную.
            TestsHelper.LastPrices.Add("Paper1", 100);
            TestsHelper.LastPrices.Add("Paper2", 100);

            var portfolio = BuildSimplePortfolioAndBuy(0, 1000);
            portfolio.RootLeaf = new PortfolioStructureLeaf
            {
                Papers = new List<PortfolioStructureLeafPaper>
                {
                    new PortfolioStructureLeafPaper
                    {
                        Paper = new SharePaper { SecId = "Paper1" },
                        Volume = 1
                    }
                }
            };

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(1);
            content.Papers[0].Count.ShouldBe(1000);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            var actions = await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum.ToString()}
            });
            actions.ShouldNotBeEmpty();
            service.ApplyActions(actions);

            var sum = actions.OfType<PortfolioMoneyAction>().Sum(x => x.Sum);
            sum.ShouldBe(100000);

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);
            content.Papers[0].Paper.SecId.ShouldBe("Paper2");
            content.Papers[0].Count.ShouldBe(1000);
            content.Papers[1].Paper.SecId.ShouldBe("Paper1");
            content.Papers[1].Count.ShouldBe(1000);
        }

        [TestMethod]
        public async Task Buy_AllowSell_Test()
        {
            // аналогично предыдущему тесту, но здесь мы разрешаем продавать бумаги
            // поэтому по результатам ожидаем что будут проданы бумаги, которых нет в структуре
            // а на полученные деньги и на внесенную сумму мы докупим бумагу, которая есть в портфеле в нужном объёме
            TestsHelper.LastPrices.Add("Paper1", 100);
            TestsHelper.LastPrices.Add("Paper2", 100);

            var portfolio = BuildSimplePortfolioAndBuy(0, 1000);
            portfolio.RootLeaf = new PortfolioStructureLeaf
            {
                Papers = new List<PortfolioStructureLeafPaper>
                {
                    new PortfolioStructureLeafPaper
                    {
                        Paper = new SharePaper { SecId = "Paper1" },
                        Volume = 1
                    }
                }
            };

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(1);
            content.Papers[0].Count.ShouldBe(1000);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            var actions = await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            });
            actions.ShouldNotBeEmpty();
            service.ApplyActions(actions);

            var sum = actions.OfType<PortfolioMoneyAction>().Where(x => x.MoneyAction == MoneyActionType.IncomeSellOnMarket).Sum(x => x.Sum);

            sum.ShouldBe(100000);
            sum = actions.OfType<PortfolioMoneyAction>().Where(x => x.MoneyAction == MoneyActionType.OutcomeBuyOnMarket).Sum(x => x.Sum);
            sum.ShouldBe(200000);

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(1);
            content.Papers[0].Paper.SecId.ShouldBe("Paper1");
            content.Papers[0].Count.ShouldBe(2000);
        }

        [TestMethod]
        public async Task Rebalance_NoSell_Test()
        {
            // есть несколько бумаг в портфеле, но в пропорациях, не соответствующих заданному
            // ожидаем что после выполнения, будет докуплено столько будмаг, что бы их вес в портфеле
            // стал соответствовать заданному
            var portfolio = BuildSimplePortfolioAndBuy(100, 1000);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);
            var actionsRepo = TestsHelper.CreatePortfolioActions();

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);
            content.Papers[0].Count.ShouldBe(100);
            content.Papers[1].Count.ShouldBe(1000);

            var prevBuySum = 12000;

            var moneyActions = actionsRepo.MoneyActions(portfolio.Id);
            var sum = moneyActions.Sum(x => x.Sum);
            sum.ShouldBe(prevBuySum);

            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / sum).ShouldBe(0.16m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / sum).ShouldBe(0.84m, 0.1m);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            var actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum.ToString()},
            })).ToList();
            service.ApplyActions(actions);

            sum = actions.OfType<PortfolioMoneyAction>().Sum(x => x.Sum);
            sum.ShouldBe(availSum);

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            availSum += prevBuySum;

            papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / availSum).ShouldBe(0.8m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / availSum).ShouldBe(0.2m, 0.01m);
        }
        
        [TestMethod]
        public async Task Rebalance_AllowSell_Test()
        {
            // аналогично тесту где нельзя было продавать, но тут разрешаем
            // поэтому проводим две итерации - сначала докупка на небольшую сумму,
            // и в этом случае часть бумаг превышающая долю будет продана
            // и вторая итерация на большую сумму, где продаж уже не будет, а будут только докупки
            var portfolio = BuildSimplePortfolioAndBuy(100, 1000);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);
            var actionsRepo = TestsHelper.CreatePortfolioActions();

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);
            content.Papers[0].Count.ShouldBe(100);
            content.Papers[1].Count.ShouldBe(1000);

            var prevBuySum = 12000;

            var moneyActions = actionsRepo.MoneyActions(portfolio.Id);
            var sum = moneyActions.Sum(x => x.Sum);
            sum.ShouldBe(prevBuySum);

            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / sum).ShouldBe(0.16m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / sum).ShouldBe(0.84m, 0.1m);

            var availSum = 8000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            var actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            })).ToList();
            service.ApplyActions(actions);

            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper1" && x.PaperAction == PaperActionType.Buy).ShouldBeTrue();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper1" && x.PaperAction == PaperActionType.Sell).ShouldBeFalse();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper2" && x.PaperAction == PaperActionType.Sell).ShouldBeTrue();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper2" && x.PaperAction == PaperActionType.Buy).ShouldBeFalse();

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            availSum += prevBuySum;
            
            papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / availSum).ShouldBe(0.8m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / availSum).ShouldBe(0.2m, 0.01m);

            papers["Paper1"].Count.ShouldBe(800);
            papers["Paper2"].Count.ShouldBe(400);

            var availSum2 = 100000m;

            adviser = new BuyAdviser(TestsHelper.CreateLogger(), builder, calculator, service);

            actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHold.P_AvailSum, availSum2.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            })).ToList();
            service.ApplyActions(actions);

            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper1" && x.PaperAction == PaperActionType.Buy).ShouldBeTrue();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper1" && x.PaperAction == PaperActionType.Sell).ShouldBeFalse();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper2" && x.PaperAction == PaperActionType.Buy).ShouldBeTrue();
            actions.OfType<PortfolioPaperAction>().Any(x => x.SecId == "Paper2" && x.PaperAction == PaperActionType.Sell).ShouldBeFalse();

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            availSum += availSum2;

            papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / availSum).ShouldBe(0.8m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / availSum).ShouldBe(0.2m, 0.01m);

            papers["Paper1"].Count.ShouldBe(4800);
            papers["Paper2"].Count.ShouldBe(2400);
        }

        [TestMethod]
        public async Task VA_NoExceed_Test()
        {
            // тесты на метод VA.
            // простой тест, требуется докупить несколько бумаг. по факту, тут отработает как обычная покупка на указанную сумму.
            var portfolio = BuildSimplePortfolioAndBuy(10, 20);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            var sum = 400;
            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / sum).ShouldBe(0.5m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / sum).ShouldBe(0.5m, 0.1m);

            var adviser = new BuyAdviserVA(TestsHelper.CreateLogger(), builder, calculator, service);

            // ожидаем, что итоговая сумма портфеля чуть чуть не дотянет
            var expectedSumm = 9995m;

            var actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume, expectedSumm.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            })).ToList();
            service.ApplyActions(actions);

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / expectedSumm).ShouldBe(0.8m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / expectedSumm).ShouldBe(0.2m, 0.01m);

            papers["Paper1"].Count.ShouldBe(400);
            papers["Paper2"].Count.ShouldBe(199);
        }

        [TestMethod]
        public async Task VA_Exceed_NoSell_Test()
        {
            // тесты на метод VA.
            // простой тест, требуется докупить несколько бумаг. по факту, тут отработает как обычная покупка на указанную сумму.
            var portfolio = BuildSimplePortfolioAndBuy(100, 200);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            var sum = 4000;
            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / sum).ShouldBe(0.5m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / sum).ShouldBe(0.5m, 0.1m);

            var adviser = new BuyAdviserVA(TestsHelper.CreateLogger(), builder, calculator, service);

            // ожидаем небольшое превышение цены
            var expectedSumm = 3005m;

            var actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume, expectedSumm.ToString()},
            })).ToList();
            service.ApplyActions(actions);
            actions.ShouldBeEmpty();
        }

        [TestMethod]
        public async Task VA_Exceed_AllowSell_Test()
        {
            // тесты на метод VA.
            // простой тест, требуется докупить несколько бумаг. по факту, тут отработает как обычная покупка на указанную сумму.
            var portfolio = BuildSimplePortfolioAndBuy(100, 200);

            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            var sum = 4000;
            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / sum).ShouldBe(0.5m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / sum).ShouldBe(0.5m, 0.1m);

            var adviser = new BuyAdviserVA(TestsHelper.CreateLogger(), builder, calculator, service);

            var expectedSumm = 2005m;

            var actions = (await adviser.Advise(portfolio, TestsHelper.ImportType, new Dictionary<string, string>
            {
                {Constants.Adviser.BuyAndHoldWithVA.P_ExpectedVolume, expectedSumm.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            })).ToList();
            service.ApplyActions(actions);

            content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(2);

            papers = content.Papers.ToDictionary(x => x.Paper.SecId);
            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / expectedSumm).ShouldBe(0.8m, 0.1m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / expectedSumm).ShouldBe(0.2m, 0.1m);

            papers["Paper1"].Count.ShouldBe(80);
            papers["Paper2"].Count.ShouldBe(40);
        }
    }
}