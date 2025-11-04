using Microsoft.AspNetCore.Mvc;
using MVCApplication.Data;
using MVCApplication.Models;

namespace MVCApplication.Controllers
{
    public class InvoiceController : Controller
    {
        public readonly AppDbContext _db;
        public InvoiceController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            List<Invoice> objInvoiceList = _db.Invoices.OrderByDescending(i => i.Id).ToList();
            return View(objInvoiceList);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Invoice obj)
        {
            if (ModelState.IsValid)
            {
                _db.Invoices.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
