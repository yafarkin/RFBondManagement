namespace RfBondManagement.Engine.Interfaces
{
    public interface IAdviserFactory
    {
        IAdviser GetAdviser(string adviserType);
    }
}