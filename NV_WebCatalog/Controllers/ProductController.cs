using Microsoft.AspNetCore.Mvc;
using NV_WebCatalog.Application.Services;
using NV_WebCatalog.Domain.Entities;

namespace NV_WebCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _service.GetSuppliersAsync();
            ViewBag.Categories = await _service.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _service.GetSuppliersAsync();
                ViewBag.Categories = await _service.GetCategoriesAsync();
                return View(product);
            }

            await _service.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            ViewBag.Suppliers = await _service.GetSuppliersAsync();
            ViewBag.Categories = await _service.GetCategoriesAsync();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _service.GetSuppliersAsync();
                ViewBag.Categories = await _service.GetCategoriesAsync();
                return View(product);
            }

            await _service.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
