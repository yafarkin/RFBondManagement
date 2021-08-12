using System.Collections.Generic;
using LiteDB;

namespace RfFondPortfolio.Common.Dtos
{
    /// <summary>
    /// Лист структуры портфеля
    /// </summary>
    public class PortfolioStructureLeaf
    {
        /// <summary>
        /// Ссылка на корневой лист
        /// </summary>
        [BsonIgnore]
        public PortfolioStructureLeaf Parent { get; set; }

        /// <summary>
        /// Список дочерних листов
        /// </summary>
        public IList<PortfolioStructureLeaf> Children { get; set; }

        /// <summary>
        /// Размер (объём) доли портфеля
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Название листа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список включенных бумаг, опционально (% бумаги и сама бумага)
        /// </summary>
        public IList<PortfolioStructureLeafPaper> Papers { get; set; }
    }
}