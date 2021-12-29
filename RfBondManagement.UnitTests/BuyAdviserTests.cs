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

        [TestMethod]
        public void FlattenStructure_CorrectPercentCalc_Test()
        {
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

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), new Dictionary<string, string>
            {
                { Constants.Adviser.P_AvailSum, availSum.ToString() }
            }, builder, calculator, service);

            var actions = await adviser.Advise(portfolio);
            actions.ShouldNotBeEmpty();

            service.ApplyActions(actions);

            var sum = actions.OfType<PortfolioMoneyAction>().Sum(x => x.Sum);
            sum.ShouldBe(99990);

            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(6);

            var papers = content.Papers.ToDictionary(x => x.Paper.SecId);

            (papers["Paper1"].Count * TestsHelper.LastPrices["Paper1"] / availSum).ShouldBe(0.16m, 0.01m);
            (papers["Paper2"].Count * TestsHelper.LastPrices["Paper2"] / availSum).ShouldBe(0.33m, 0.01m);
            (papers["Paper3"].Count * TestsHelper.LastPrices["Paper3"] / availSum).ShouldBe(0.15m, 0.01m);
            (papers["Paper4"].Count * TestsHelper.LastPrices["Paper4"] / availSum).ShouldBe(0.1m, 0.01m);
            (papers["Paper5"].Count * TestsHelper.LastPrices["Paper5"] / availSum).ShouldBe(0.165m, 0.01m);
            (papers["Paper6"].Count * TestsHelper.LastPrices["Paper6"] / availSum).ShouldBe(0.083m, 0.01m);
        }

        [TestMethod]
        public async Task BuyNoSell_Test()
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
                            Volume = 1
                        }
                    }
                }
            };
            
            for (var i = 0; i < 2; i++)
            {
                TestsHelper.Papers.Add(new SharePaper { SecId = $"Paper{i + 1}" });
            }

            TestsHelper.LastPrices.Add("Paper1", 100);
            TestsHelper.LastPrices.Add("Paper2", 100);
            
            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            service.ApplyActions(calculator.BuyPaper(new SharePaper {SecId = "Paper2"}, 1000, 100));
            
            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(1);
            content.Papers[0].Count.ShouldBe(1000);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), new Dictionary<string, string>
            {
                { Constants.Adviser.P_AvailSum, availSum.ToString() }
            }, builder, calculator, service);

            var actions = await adviser.Advise(portfolio);
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
        public async Task BuyAllowSell_Test()
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
                            Volume = 1
                        }
                    }
                }
            };
            
            for (var i = 0; i < 2; i++)
            {
                TestsHelper.Papers.Add(new SharePaper { SecId = $"Paper{i + 1}" });
            }

            TestsHelper.LastPrices.Add("Paper1", 100);
            TestsHelper.LastPrices.Add("Paper2", 100);
            
            var builder = TestsHelper.CreateBuilder();
            var calculator = TestsHelper.CreateCalculator(portfolio);
            var service = TestsHelper.CreateService(portfolio);

            service.ApplyActions(calculator.BuyPaper(new SharePaper {SecId = "Paper2"}, 1000, 100));
            
            var content = builder.Build(portfolio.Id);
            content.Papers.Count.ShouldBe(1);
            content.Papers[0].Count.ShouldBe(1000);

            var availSum = 100000m;

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), new Dictionary<string, string>
            {
                {Constants.Adviser.P_AvailSum, availSum.ToString()},
                {Constants.Adviser.P_AllowSell, "true"}
            }, builder, calculator, service);

            var actions = await adviser.Advise(portfolio);
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

    }
}