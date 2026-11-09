using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityCafeteria.Data;
using UniversityCafeteria5.Models;

namespace UniversityCafeteria5.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadLists(new Product());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!_context.Categories.Any(item => item.ID == product.CategoryID))
            {
                ModelState.AddModelError(nameof(Product.CategoryID), "Select a valid category.");
            }

            if (!_context.Suppliers.Any(item => item.ID == product.SupplierID))
            {
                ModelState.AddModelError(nameof(Product.SupplierID), "Select a valid supplier.");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadLists(product);
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            LoadLists(product);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var existing = _context.Products.Find(product.ID);
            if (existing == null)
            {
                return NotFound();
            }

            if (!_context.Categories.Any(item => item.ID == product.CategoryID))
            {
                ModelState.AddModelError(nameof(Product.CategoryID), "Select a valid category.");
            }

            if (!_context.Suppliers.Any(item => item.ID == product.SupplierID))
            {
                ModelState.AddModelError(nameof(Product.SupplierID), "Select a valid supplier.");
            }

            if (ModelState.IsValid)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.CategoryID = product.CategoryID;
                existing.SupplierID = product.SupplierID;
                _context.Products.Update(existing);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadLists(product);
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        private void LoadLists(Product product)
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.ToList(),
                nameof(Category.ID),
                nameof(Category.Name),
                product.CategoryID);
            ViewBag.Suppliers = new SelectList(
                _context.Suppliers.ToList(),
                nameof(Supplier.ID),
                nameof(Supplier.Name),
                product.SupplierID);
        }
    }
}
