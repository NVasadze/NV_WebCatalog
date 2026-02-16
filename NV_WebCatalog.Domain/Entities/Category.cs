namespace NV_WebCatalog.Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
