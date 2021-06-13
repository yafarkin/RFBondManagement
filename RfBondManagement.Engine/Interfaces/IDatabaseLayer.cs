using System;
using LiteDB;

namespace RfBondManagement.Engine.Interfaces
{
    public interface IDatabaseLayer : IDisposable
    {
        public ILiteDatabase Database { get; }
    }
}