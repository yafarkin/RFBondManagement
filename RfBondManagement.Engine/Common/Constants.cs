namespace RfBondManagement.Engine.Common
{
    public static class Constants
    {
        public static class Adviser
        {
            public static class BuyAndHold
            {
                /// <summary>
                /// Подход "купил и держи"
                /// </summary>
                public static readonly string Name = "buy_and_hold";

                /// <summary>
                /// Доступная сейчас сумма
                /// </summary>
                public static readonly string P_AvailSum = "avail_sum";
            }

            public static class BuyAndHoldWithVA
            {
                /// <summary>
                /// Купил и держи, но с VA подходом
                /// </summary>
                public static readonly string Name = "buy_and_hold_with_va";

                /// <summary>
                /// Ожидаемый объём портфеля для VA подхода
                /// </summary>
                public static readonly string P_ExpectedVolume = "expected_volume";
            }

            /// <summary>
            /// Смотреть цены на бумаги на указанную дату (в прошлом)
            /// </summary>
            public static readonly string P_OnDate = "on_date";

            /// <summary>
            /// Разрешено ли продавать бумагу
            /// </summary>
            public static readonly string P_AllowSell = "allow_sell";
        }
    }
}