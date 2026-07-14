using SQLite;

namespace AppInventario.Models
{

    [Table("Products")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50), NotNull, Unique]
        public string Code { get; set; } = string.Empty;

        [MaxLength(100), NotNull]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [NotNull]
        public decimal PurchasePrice { get; set; }

        [NotNull]
        public decimal SalePrice { get; set; }

        [NotNull]
        public int InitialStock { get; set; }

        [NotNull]
        public int CurrentStock { get; set; }

        [NotNull]
        public int MinimumStock { get; set; }

        [NotNull]
        public ProductCategory Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        [Ignore]
        public bool IsLowStock => CurrentStock <= MinimumStock;

        [Ignore]
        public decimal ProfitMargin => SalePrice - PurchasePrice;

        [Ignore]
        public decimal ProfitMarginPercentage => PurchasePrice > 0 
            ? Math.Round(((SalePrice - PurchasePrice) / PurchasePrice) * 100, 2) 
            : 0;
        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}