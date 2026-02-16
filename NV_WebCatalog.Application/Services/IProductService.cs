using NV_WebCatalog.Domain.Entities;

namespace NV_WebCatalog.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        Task<List<Supplier>> GetSuppliersAsync();
        Task<List<Category>> GetCategoriesAsync();
    }
}
