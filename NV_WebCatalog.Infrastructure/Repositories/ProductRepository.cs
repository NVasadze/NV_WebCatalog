using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NV_WebCatalog.Domain.Entities;
using NV_WebCatalog.Application.Services;

namespace NV_WebCatalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Products", connection);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(MapProduct(reader));
                    }
                }
            }

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            Product? product = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT * FROM Products WHERE ProductID = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = MapProduct(reader);
                    }
                }
            }

            return product;
        }

        public async Task AddAsync(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    INSERT INTO Products 
                    (ProductName, QuantityPerUnit, SupplierID, CategoryID, 
                     UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
                    VALUES 
                    (@ProductName, @QuantityPerUnit, @SupplierID, @CategoryID,
                     @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)",
                    connection);

                command.Parameters.AddWithValue("@ProductName", product.ProductName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@QuantityPerUnit", product.QuantityPerUnit ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SupplierID", product.SupplierID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CategoryID", product.CategoryID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitsOnOrder", product.UnitsOnOrder ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    UPDATE Products 
                    SET ProductName = @ProductName,
                        QuantityPerUnit = @QuantityPerUnit,
                        SupplierID = @SupplierID,
                        CategoryID = @CategoryID,
                        UnitPrice = @UnitPrice,
                        UnitsInStock = @UnitsInStock,
                        UnitsOnOrder = @UnitsOnOrder,
                        ReorderLevel = @ReorderLevel,
                        Discontinued = @Discontinued
                    WHERE ProductID = @ProductID",
                    connection);

                command.Parameters.AddWithValue("@ProductID", product.ProductId);
                command.Parameters.AddWithValue("@ProductName", product.ProductName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@QuantityPerUnit", product.QuantityPerUnit ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SupplierID", product.SupplierID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CategoryID", product.CategoryID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UnitsOnOrder", product.UnitsOnOrder ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "DELETE FROM Products WHERE ProductID = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        private Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = (int)reader["ProductID"],
                ProductName = reader["ProductName"]?.ToString(),
                QuantityPerUnit = reader["QuantityPerUnit"]?.ToString(),
                SupplierID = reader["SupplierID"] as int?,
                CategoryID = reader["CategoryID"] as int?,
                UnitPrice = reader["UnitPrice"] as decimal?,
                UnitsInStock = reader["UnitsInStock"] as short?,
                UnitsOnOrder = reader["UnitsOnOrder"] as short?,
                ReorderLevel = reader["ReorderLevel"] as short?,
                Discontinued = (bool)reader["Discontinued"]
            };
        }
    }
}
