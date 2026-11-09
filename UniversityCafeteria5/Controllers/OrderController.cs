using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityCafeteria.Data;
using UniversityCafeteria5.Models;

namespace UniversityCafeteria5.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadLists(new Order());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (!_context.Employees.Any(item => item.Id == order.EmployeeId))
            {
                ModelState.AddModelError(nameof(Order.EmployeeId), "Select a valid employee.");
            }

            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadLists(order);
            return View(order);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            LoadLists(order);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            var existing = _context.Orders.Find(order.Id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!_context.Employees.Any(item => item.Id == order.EmployeeId))
            {
                ModelState.AddModelError(nameof(Order.EmployeeId), "Select a valid employee.");
            }

            if (ModelState.IsValid)
            {
                existing.CustomerName = order.CustomerName;
                existing.OrderDate = order.OrderDate;
                existing.TotalPrice = order.TotalPrice;
                existing.Status = order.Status;
                existing.EmployeeId = order.EmployeeId;
                _context.Orders.Update(existing);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadLists(order);
            return View(order);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private void LoadLists(Order order)
        {
            ViewBag.Employees = new SelectList(
                _context.Employees.ToList(),
                nameof(Employee.Id),
                nameof(Employee.FullName),
                order.EmployeeId);
        }
    }
}
