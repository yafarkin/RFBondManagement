using System.Collections.Generic;
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

        public void BuyPaper(Portfolio portfolio, string secId, long count, decimal price)
        {
            //portfolio.
        }

        public Portfolio BuildSamplePortfolio()
        {
            var portfolio = new Portfolio
            {
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

            var adviser = new BuyAdviser(TestsHelper.CreateLogger(), new Dictionary<string, string>
            {
                { Constants.Adviser.P_AvailSum, "100000" }
            }, TestsHelper.CreateBuilder(), TestsHelper.CreateCalculator(portfolio), TestsHelper.CreateService(portfolio));

            var result = await adviser.Advise(portfolio);
            Assert.Fail("тест только начат");
        }
    }
}