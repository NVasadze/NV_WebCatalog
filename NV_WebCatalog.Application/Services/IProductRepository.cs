using NV_WebCatalog.Application.Services;
using NV_WebCatalog.Domain.Entities;


namespace NV_WebCatalog.Application.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}
