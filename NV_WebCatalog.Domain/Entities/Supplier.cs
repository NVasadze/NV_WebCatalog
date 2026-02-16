namespace NV_WebCatalog.Domain.Entities
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        public string? CompanyName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
