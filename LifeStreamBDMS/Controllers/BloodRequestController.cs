using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LifeStreamBDMS.Data;
using LifeStreamBDMS.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LifeStreamBDMS.Controllers
{
    [Authorize(Roles = "Admin")] // 🔹 Restricts the entire controller to Admins
    public class BloodRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BloodRequestController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // 🩸 Request Form (GET)
        [AllowAnonymous] // 🔹 Makes the public request form accessible to all users
        public IActionResult Request()
        {
            return View();
        }

        // 🩸 Request Form (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous] // 🔹 Allows non-admins to submit requests
        public IActionResult Request(BloodRequest model)
        {
            if (ModelState.IsValid)
            {
                model.Status = RequestStatus.Pending;
                _context.BloodRequests.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Your request has been submitted!";
                return RedirectToAction("Request");
            }
            return View(model);
        }

        // 🛡 Admin - All Requests
        public IActionResult AllRequest()
        {
            var requests = _context.BloodRequests.OrderByDescending(r => r.RequestDate).ToList();
            return View("AllRequest", requests); // 🔹 Ensure explicit view name
        }


        // ✅ Admin - Pending Only
        public IActionResult PendingRequests()
        {
            var pending = _context.BloodRequests
                .Where(r => r.Status == RequestStatus.Pending)
                .OrderByDescending(r => r.RequestDate)
                .ToList();
            return View(pending);
        }

        // ✅ Approve Request
        public IActionResult Approve(int id)
        {
            var request = _context.BloodRequests.Find(id);
            if (request != null)
            {
                request.Status = RequestStatus.Accepted;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Request Approved!";
            }
            else
            {
                TempData["ErrorMessage"] = "Request not found!";
            }

            return RedirectToAction("PendingRequests");
        }

        // ❌ Reject Request
        public IActionResult Reject(int id)
        {
            var request = _context.BloodRequests.Find(id);
            if (request != null)
            {
                request.Status = RequestStatus.Rejected;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Request Rejected!";
            }
            else
            {
                TempData["ErrorMessage"] = "Request not found!";
            }

            return RedirectToAction("PendingRequests");
        }

        // 👤 Public - Accepted Only
        [AllowAnonymous] // 🔹 Allows non-admin users to view accepted requests
        public IActionResult PublicRequests()
        {
            var accepted = _context.BloodRequests
                .Where(r => r.Status == RequestStatus.Accepted)
                .OrderByDescending(r => r.RequestDate)
                .ToList();
            return View(accepted);
        }
    }
}
