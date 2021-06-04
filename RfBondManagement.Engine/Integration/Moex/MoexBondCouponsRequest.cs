using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class MoexBondCouponsRequest : MoexBaseRequest<Bondization>
    {
        public override Bondization Read(string tiker)
        {
            return BaseRead($"/securities/{tiker}/bondization");
        }
    }
}