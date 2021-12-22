using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using RfBondManagement.Engine.Calculations;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestClass]
    public class BuyAdviserTests
    {
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
        public void Advice_BuyOnly_Test()
        {
            var portfolio = BuildSamplePortfolio();

            var logger = new Mock<ILogger>().Object;
            var import = new Mock<IExternalImport>().Object;

            var importFactoryMock = new Mock<IExternalImportFactory>();
            importFactoryMock.Setup(m => m.GetDefaultImpl()).Returns(() => import);

            var paperRepository = new Mock<IPaperRepository>().Object;
            var moneyActionRepository = new Mock<IPortfolioMoneyActionRepository>().Object;
            var paperActionRepository = new Mock<IPortfolioPaperActionRepository>().Object;
            var splitRepository = new Mock<ISplitRepository>().Object;
            var bondCalculator = new Mock<IBondCalculator>().Object;

            var engine = new PortfolioEngine(importFactoryMock.Object, paperRepository, moneyActionRepository, paperActionRepository, splitRepository, bondCalculator, logger);
            engine.Configure(portfolio, ExternalImportType.Moex);

            var advice = BuyAdviser.Advise(logger, engine, 100000, false, false,null, import, null);
        }
    }
}