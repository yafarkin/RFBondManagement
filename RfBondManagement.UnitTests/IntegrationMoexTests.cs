using NUnit.Framework;
using RfBondManagement.Engine.Integration.Moex;
using Shouldly;

namespace RfBondManagement.UnitTests
{
    [TestFixture]
    public class IntegrationMoexTests
    {
        [Test]
        public void BondCouponsTests()
        {
            var requestor = new MoexBondCouponsRequest();

            // https://smart-lab.ru/q/bonds/SU26227RMFS7/, ОФЗ 26227
            var coupons = requestor.Read("SU26227RMFS7");

            coupons.ShouldNotBeNull();
        }
    }
}