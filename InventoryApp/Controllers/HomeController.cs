using InventoryApp.DataAccess;
using InventoryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        public HomeController() {
            
        }
        public async Task<IActionResult> Index()
        {
            
            return Json(null);
        }
    }
}
