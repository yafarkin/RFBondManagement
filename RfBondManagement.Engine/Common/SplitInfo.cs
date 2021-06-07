﻿using System;
using LiteDB;

namespace RfBondManagement.Engine.Common
{
    public class SplitInfo
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public decimal Multiplier { get; set; }
    }
}