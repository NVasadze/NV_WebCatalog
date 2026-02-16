using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NV_WebCatalog.Domain.Entities;

namespace NV_WebCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly string _connectionString;

        public ProductService(IProductRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Product product)
        {
            await _repository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
        {
            var list = new List<Supplier>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT SupplierID, CompanyName FROM Suppliers", connection);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Supplier
                        {
                            SupplierID = (int)reader["SupplierID"],
                            CompanyName = reader["CompanyName"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var list = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories", connection);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Category
                        {
                            CategoryID = (int)reader["CategoryID"],
                            CategoryName = reader["CategoryName"].ToString()
                        });
                    }
                }
            }

            return list;
        }
    }
}
