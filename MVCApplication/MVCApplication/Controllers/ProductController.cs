using Microsoft.AspNetCore.Mvc;
using MVCApplication.Data;
using MVCApplication.Models;

namespace MVCApplication.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _db.Products.ToList();
            return View(objProductList);
        }
    }
}
