using System;
using System.Collections.Generic;
using System.Linq;

namespace RfBondManagement.Engine.Common
{
    public abstract class BaseStockPaper
    {
        /// <summary>
        /// Внутренний идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Код ценной бумаги
        /// </summary>
        public string SecId { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// ISIN код
        /// </summary>
        public string Isin { get; set; }

        /// <summary>
        /// Номинальная стоимость
        /// </summary>
        public decimal? FaceValue { get; set; }

        /// <summary>
        /// Дата начала торгов
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Вид\категория ценной бумагни
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Код типа инструмента
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Тип бумаги
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Тип инструмента
        /// </summary>
        public string GroupName { get; set; }

        [Obsolete]
        public string Code { get; set; }

        [Obsolete]
        public List<PriceOnDate> Price { get; set; }

        [Obsolete]
        public PriceOnDate LastPrice => Price?.OrderByDescending(p => p.Date).FirstOrDefault();

        public override string ToString() => Name;
    }
}
