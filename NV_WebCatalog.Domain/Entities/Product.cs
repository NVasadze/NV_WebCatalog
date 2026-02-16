using System.ComponentModel.DataAnnotations.Schema;

namespace NV_WebCatalog.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? QuantityPerUnit { get; set; }

        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [ForeignKey("SupplierID")]
        public Supplier? Supplier { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }
    }
}
