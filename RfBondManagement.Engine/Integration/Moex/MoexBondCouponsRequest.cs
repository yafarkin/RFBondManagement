using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexBondCouponsRequest : MoexBaseRequest<JsonBondization>
    {
        public override JsonBondization Read(string tiker)
        {
            return BaseRead($"/securities/{tiker}/bondization");
        }
    }
}