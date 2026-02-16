using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NV_WebCatalog.Domain.Entities;
using NV_WebCatalog.Application.Services;

namespace NV_WebCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly string _connectionString;

        public ProductController(IProductRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllAsync();
            return View(products);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // CREATE (GET)
        // =========================
        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await GetSuppliersAsync();
            ViewBag.Categories = await GetCategoriesAsync();
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await GetSuppliersAsync();
                ViewBag.Categories = await GetCategoriesAsync();
                return View(product);
            }

            await _repository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            ViewBag.Suppliers = await GetSuppliersAsync();
            ViewBag.Categories = await GetCategoriesAsync();

            return View(product);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await GetSuppliersAsync();
                ViewBag.Categories = await GetCategoriesAsync();
                return View(product);
            }

            await _repository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // PRIVATE METHODS
        // =========================
        private async Task<List<Supplier>> GetSuppliersAsync()
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

        private async Task<List<Category>> GetCategoriesAsync()
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
