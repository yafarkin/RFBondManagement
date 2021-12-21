using System.Collections.Generic;
using NUnit.Framework;
using RfBondManagement.Engine.Calculations;
using RfFondPortfolio.Common.Dtos;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class BuyAdviserTests
    {
        [Test]
        public void FlattenStructure_CorrectPercentCalc_Test()
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

            var flattenList = BuyAdviser.FlattenPaperStructure(portfolio.RootLeaf, 1);
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
    }
}