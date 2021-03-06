﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using NUnit.Framework;
using RfBondManagement.Engine;
using RfBondManagement.Engine.Database;
using RfFondPortfolio.Common.Dtos;
using RfFondPortfolio.Common.Interfaces;
using RfFondPortfolio.Integration.Moex;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class IntegrationMoexTests
    {
        public IExternalImport Import;
        public ILogger Logger;

        [SetUp]
        public void Setup()
        {
            Import = new MoexImport();
            Logger = LogManager.GetCurrentClassLogger();
        }

        [Test]
        public async Task ShareImportTest()
        {
            var paper = (await Import.ImportPaper(Logger, "SBERP")) as SharePaper;
            paper.ShouldNotBeNull();

            paper.PaperType.ShouldBe(PaperType.Share);
            paper.Isin.ShouldBe("RU0009029557");
            paper.Name.ShouldBe("Сбербанк России ПАО ап");
            paper.PrimaryBoard.BoardId.ShouldBe("TQBR");
            paper.IsPreferedShare.ShouldBeTrue();

            var dividends = paper.Dividends;
            dividends.ShouldNotBeNull();

            var firstDividend = dividends.First();
            firstDividend.Value.ShouldBe(16.0m);
            firstDividend.RegistryCloseDate.ShouldBe(new DateTime(2019, 6, 13));
        }

        [Test]
        public async Task BondImportTest()
        {
            // https://smart-lab.ru/q/bonds/SU26227RMFS7/, ОФЗ 26227
            var paper = (await Import.ImportPaper(Logger, "SU26227RMFS7")) as BondPaper;
            paper.ShouldNotBeNull();
            paper.PaperType.ShouldBe(PaperType.Bond);
            paper.Isin.ShouldBe("RU000A1007F4");
            paper.Name.ShouldBe("ОФЗ-ПД 26227 17/07/24");
            paper.PrimaryBoard.BoardId.ShouldBe("TQOB");
            paper.IsOfzBond.ShouldBeTrue();

            var coupons = paper.Coupons;
            coupons.ShouldNotBeNull();
            coupons.Count.ShouldBe(11);

            var firstCoupon = coupons.First();
            firstCoupon.ValueRub.ShouldBe(24.13m);
            firstCoupon.CouponDate.ShouldBe(new DateTime(2019, 7, 24));
        }

        [Test]
        public async Task HistoryTest()
        {
            using (var db = new DatabaseLayer())
            {
                var historyRepository = new HistoryRepository(db);
                var historyEngine = new HistoryEngine(historyRepository, Import, Logger);
                //var lastDate = historyEngine.GetLastHistoryDate("SBERP");

                await historyEngine.ImportHistory("SBERP");
            }
        }

        [Test]
        public async Task GetLastPriceTest()
        {
            // share
            var sharePaper = await Import.ImportPaper(Logger, "SBERP");
            var lastPrice = await Import.LastPrice(Logger, sharePaper);
            lastPrice.ShouldNotBeNull();
            lastPrice.LotSize.ShouldBe(10);
            lastPrice.Price.ShouldBeGreaterThan(0);

            // corporate bond
            var bondPaper = await Import.ImportPaper(Logger, "RU000A1018X4");
            lastPrice = await Import.LastPrice(Logger, bondPaper);
            lastPrice.ShouldNotBeNull();
            lastPrice.LotSize.ShouldBe(1);
            lastPrice.Price.ShouldBeGreaterThan(0);

            // history, field CLOSE
            // http://iss.moex.com/iss/history/engines/stock/markets/shares/boards/TQBR/securities/SBER.json?iss.json=extended&from=2000-01-01
        }

        [Test]
        public async Task GetHistoryTest()
        {
            var paper = await Import.ImportPaper(Logger, "SU26208RMFS7");
            var history = (await Import.HistoryPrice(Logger, paper)).ToList();
            history.Count.ShouldBe(1494);
        }
    }
}