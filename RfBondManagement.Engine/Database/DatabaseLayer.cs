using LiteDB;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class DatabaseLayer : IDatabaseLayer
    {
        public ILiteDatabase Database { get; protected set; }

        public DatabaseLayer()
        {
            Database = new LiteDatabase("portfolios.db");
        }

        public void Dispose()
        {
            Database?.Dispose();
            Database = null;
        }
    }
}