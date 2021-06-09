using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RfBondManagement.Engine.Integration.Moex;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class IntegrationMoexTests
    {
        [Test]
        public async Task PaperDefinitionTest()
        {
            var request = new MoexPaperDefinitionRequest("SBERP");
            var paper = await request.Read();

            paper.ShouldNotBeNull();
            var name = paper.Description.GetDataFor("name", "NAME");
            var isin = paper.Description.GetDataFor("name", "ISIN");
            isin["value"].ShouldBe("RU0009029557");
            name["value"].ShouldBe("Сбербанк России ПАО ап");

            request = new MoexPaperDefinitionRequest("SU26227RMFS7");
            paper = await request.Read();

            paper.ShouldNotBeNull();
            name = paper.Description.GetDataFor("name", "NAME");
            isin = paper.Description.GetDataFor("name", "ISIN");

            name["value"].ShouldBe("ОФЗ-ПД 26227 17/07/24");
            isin["value"].ShouldBe("RU000A1007F4");
        }

        [Test]
        public async Task BondCouponsTest()
        {
            // https://smart-lab.ru/q/bonds/SU26227RMFS7/, ОФЗ 26227
            var request = new MoexBondCouponsRequest("SU26227RMFS7");

            var coupons = await request.Read();

            coupons.ShouldNotBeNull();
            coupons.Coupons.Data.Count.ShouldBe(11);

            var firstCoupon = coupons.Coupons.GetDataFor("coupondate", "2019-07-24");
            firstCoupon["value_rub"].ShouldBe("24.13");
        }

        [Test]
        public async Task HistoryTest()
        {
            var request = new MoexSecurityHistoryRequest("stock", "bonds", "SU26208RMFS7");

            var response = await request.CursorRead();

            response.ShouldNotBeNull();
        }

        [Test]
        public async Task DividendsRequestTest()
        {
            var request = new MoexDividendsRequest("SBER");
            var dividends = await request.Read();

            dividends.ShouldNotBeNull();
            var div = dividends.Dividends.GetDataForString("secid", "SBER", "isin");
            div.ShouldBe("RU0009029540");
        }

        [Test]
        public async Task MapShareTest()
        {
            var request = new MoexPaperDefinitionRequest("SBERP");
            var jsonPaper = await request.Read();
            var sharePaper = StockPaperConverter.Map(jsonPaper);

            sharePaper.Isin.ShouldBe("RU0009029557");
            sharePaper.IsShare.ShouldBeTrue();
            sharePaper.IsPreferedShare.ShouldBeTrue();
            sharePaper.PrimaryBoard.BoardId.ShouldBe("TQBR");
        }

        [Test]
        public async Task MapBondTest()
        {
            var bondRequest = new MoexPaperDefinitionRequest("SU26208RMFS7");
            var couponRequest = new MoexBondCouponsRequest("SU26208RMFS7");

            var jsonBond = await bondRequest.Read();
            var jsonCoupon = await couponRequest.Read();

            var bondPaper = StockPaperConverter.Map(jsonBond, jsonCoupon);
            bondPaper.ShouldNotBe(null);

            bondPaper.Isin.ShouldBe("RU000A0JS4M5");
            bondPaper.Coupons.Count.ShouldBe(14);
            bondPaper.Coupons.First().Date.ShouldBe(new DateTime(2012, 9, 5));
            bondPaper.Coupons.First().Value.ShouldBe(37.4m);
            bondPaper.Coupons.Last().Date.ShouldBe(new DateTime(2019, 2, 27));
            bondPaper.Coupons.Last().Value.ShouldBe(37.4m);

            bondPaper.IsBond.ShouldBeTrue();
            bondPaper.IsOfzBond.ShouldBeTrue();
            bondPaper.PrimaryBoard.BoardId.ShouldBe("TQOB");
        }

        [Test]
        public async Task GetLastPriceTest()
        {
            var requestPaper = new MoexPaperDefinitionRequest("SBERP");
            var jsonPaper = await requestPaper.Read();
            var paper = StockPaperConverter.Map(jsonPaper);

            var requestPrice = new MoexLastPriceRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            var response = await requestPrice.Read();

            var lastPrice = response.Securities.GetDataForDecimal("SECID", paper.SecId, "PREVADMITTEDQUOTE");
            lastPrice.ShouldNotBeNull();
            lastPrice.Value.ShouldBeGreaterThan(0.01m);

            requestPaper = new MoexPaperDefinitionRequest("FXIT");
            jsonPaper = await requestPaper.Read();
            paper = StockPaperConverter.Map(jsonPaper);

            requestPrice = new MoexLastPriceRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            response = await requestPrice.Read();

            lastPrice = response.Securities.GetDataForDecimal("SECID", paper.SecId, "PREVADMITTEDQUOTE");
            lastPrice.ShouldNotBeNull();
            lastPrice.Value.ShouldBeGreaterThan(0.01m);

            // ofz bond
            requestPaper = new MoexPaperDefinitionRequest("SU29006RMFS2");
            jsonPaper = await requestPaper.Read();
            paper = StockPaperConverter.Map(jsonPaper);

            requestPrice = new MoexLastPriceRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            response = await requestPrice.Read();

            lastPrice = response.Securities.GetDataForDecimal("SECID", paper.SecId, "PREVADMITTEDQUOTE");
            lastPrice.ShouldNotBeNull();
            lastPrice.Value.ShouldBeGreaterThan(0.01m);

            // corporate bond
            requestPaper = new MoexPaperDefinitionRequest("RU000A1018X4");
            jsonPaper = await requestPaper.Read();
            paper = StockPaperConverter.Map(jsonPaper);

            requestPrice = new MoexLastPriceRequest(paper.PrimaryBoard.Market, paper.PrimaryBoard.BoardId, paper.SecId);
            response = await requestPrice.Read();

            lastPrice = response.Securities.GetDataForDecimal("SECID", paper.SecId, "PREVADMITTEDQUOTE");
            lastPrice.ShouldNotBeNull();
            lastPrice.Value.ShouldBeGreaterThan(0.01m);

            // history, field CLOSE
            // http://iss.moex.com/iss/history/engines/stock/markets/shares/boards/TQBR/securities/SBER.json?iss.json=extended&from=2000-01-01
        }

        [Test]
        public void GetNextCouponTest()
        {

        }

    }
}