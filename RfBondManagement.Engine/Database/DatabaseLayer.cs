using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using NLog.Targets;
using RfBondManagement.Engine.Common;
using RfBondManagement.Engine.Interfaces;

namespace RfBondManagement.Engine.Database
{
    public class DatabaseLayer : IDatabaseLayer
    {
        protected ILiteDatabase _database;

        protected ILiteCollection<Settings> _settingsSet;
        protected ILiteCollection<BaseStockPaper> _papersList;
        protected ILiteCollection<BaseBondPaperInPortfolio> _bonds;

        public DatabaseLayer()
        {
            _database = new LiteDatabase("bondmanagement.db");
            _settingsSet = _database.GetCollection<Settings>("settings");
            _papersList = _database.GetCollection<BaseStockPaper>("papersList");
            _bonds = _database.GetCollection<BaseBondPaperInPortfolio>("bondsInPortfolio");
        }

        public Settings LoadSettings()
        {
            var result = _settingsSet.FindAll().FirstOrDefault();
            if (null == result)
            {
                result = new Settings();
                _settingsSet.Insert(result);
            }

            return result;
        }

        public void SaveSettings(Settings settings)
        {
            _settingsSet.DeleteAll();
            _settingsSet.Insert(settings);
        }

        public IEnumerable<BaseBondPaperInPortfolio> GetBondsInPortfolio()
        {
            var r = new List<BaseBondPaperInPortfolio>();
            var p = GetPapers().OfType<BaseBondPaper>().First();

            r.Add(new BaseBondPaperInPortfolio
            {
                Paper = p,
                Actions = new List<BaseAction<BaseBondPaper>>
                {
                    new BondBuyAction
                    {
                        IsBuy = true,
                        Paper = p,
                        Nkd = 29.56m,
                        Date = new DateTime(2021, 2, 12),
                        Count = 1,
                        Price = p.LastPrice.Price,
                    }
                }
            });

            return r;
        }

        public IEnumerable<BaseStockPaper> GetPapers()
        {
            var r = new List<BaseStockPaper>();

            r.Add(new BaseBondPaper
            {
                Name = "ОФЗ 26223",
                ISIN = "RU000A0ZYU88",
                Code = "SU26223RMFS6",
                PublishDate = new DateTime(2018, 2, 21),
                MaturityDate = new DateTime(2024, 2, 28),
                Price = new List<PriceOnDate>
                {
                    new PriceOnDate
                    {
                        Date = new DateTime(2021, 2, 12),
                        Price = 103.001m
                    }
                },
                Duration = 999,
                BondPar = 1000,
                Currency = "RUR",
                Coupons = new List<BondCoupon>
                {
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 3, 3),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2021, 9, 1),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 3, 2),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2022, 8, 31),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 3, 1),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2023, 8, 30),
                        Value = 32.41m
                    },
                    new BondCoupon
                    {
                        Date = new DateTime(2024, 2, 28),
                        Value = 32.41m
                    }
                }
            });

            return r;

            //return _papersList.FindAll();
        }

        public void Dispose()
        {
            _database?.Dispose();
            _database = null;
        }
    }
}