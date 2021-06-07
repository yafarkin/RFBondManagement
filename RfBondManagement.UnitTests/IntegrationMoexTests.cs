using NUnit.Framework;
using RfBondManagement.Engine.Integration.Moex;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class IntegrationMoexTests
    {
        [Test]
        public void PaperDefinitionTest()
        {
            var request = new MoexPaperDefinitionRequest("SBERP");
            var paper = request.Read();

            paper.ShouldNotBeNull();
            var name = paper.Description.GetDataFor("name", "NAME");
            var isin = paper.Description.GetDataFor("name", "Isin");
            isin["value"].ShouldBe("RU0009029557");
            name["value"].ShouldBe("Сбербанк России ПАО ап");

            request = new MoexPaperDefinitionRequest("SU26227RMFS7");
            paper = request.Read();

            paper.ShouldNotBeNull();
            name = paper.Description.GetDataFor("name", "NAME");
            isin = paper.Description.GetDataFor("name", "Isin");

            name["value"].ShouldBe("ОФЗ-ПД 26227 17/07/24");
            isin["value"].ShouldBe("RU000A1007F4");
        }

        [Test]
        public void BondCouponsTest()
        {
            // https://smart-lab.ru/q/bonds/SU26227RMFS7/, ОФЗ 26227
            var request = new MoexBondCouponsRequest("SU26227RMFS7");

            var coupons = request.Read();

            coupons.ShouldNotBeNull();
            coupons.Coupons.Data.Count.ShouldBe(11);

            var firstCoupon = coupons.Coupons.GetDataFor("coupondate", "2019-07-24");
            firstCoupon["value_rub"].ShouldBe("24.13");
        }

        [Test]
        public void HistoryTest()
        {
            var request = new MoexSecurityHistoryRequest("stock", "bonds", "SU26208RMFS7");

            var response = request.CursorRead();

            response.ShouldNotBeNull();
        }

        [Test]
        public void MapStockTest()
        {
            var request = new MoexPaperDefinitionRequest("SBERP");
            var jsonPaper = request.Read();
            var localPaper = StockPaperConverter.Map(jsonPaper);

            localPaper.Isin.ShouldBe("RU0009029557");
        }
    }
}