using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LifeStreamBDMS.Data;
using System.Linq;

namespace LifeStreamBDMS.Controllers
{
    [Authorize(Roles = "Admin")] // Restrict the entire controller to Admins
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }




        public IActionResult Inventory()
        {
            return View();
        }
    }
}
