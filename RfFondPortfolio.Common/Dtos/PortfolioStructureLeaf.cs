using System.Collections.Generic;

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
        public PortfolioStructureLeaf Parent { get; set; }

        /// <summary>
        /// Список дочерних листов
        /// </summary>
        public IEnumerable<PortfolioStructureLeaf> Childs { get; set; }

        /// <summary>
        /// Размер доли портфеля
        /// </summary>
        public decimal PercentLimit { get; set; }

        /// <summary>
        /// Название листа, опционально
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список включенных бумаг, опционально
        /// </summary>
        public IEnumerable<AbstractPaper> Papers { get; set; }
    }
}