using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LifeStreamBDMS.Data;
using LifeStreamBDMS.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LifeStreamBDMS.Controllers
{
    [Authorize(Roles = "Admin")] // 🔹 Restricts entire controller to Admins
    public class DonorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Display Registration Form (No Role Restriction)
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Handle Donor Registration & Update Blood Inventory (No Role Restriction)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Register(Donor donor)
        {
            if (ModelState.IsValid)
            {
                _context.Donors.Add(donor);
                _context.SaveChanges();

                // 🔹 Update blood inventory efficiently
                var existingInventory = _context.BloodInventories.FirstOrDefault(b => b.BloodType == donor.BloodType);

                if (existingInventory != null)
                {
                    existingInventory.AvailableUnits++; // Increment blood units
                    existingInventory.LastUpdated = DateTime.Now;
                }
                else
                {
                    _context.BloodInventories.Add(new BloodInventory
                    {
                        BloodType = donor.BloodType,
                        AvailableUnits = 1,
                        Status = "Adequate",
                        LastUpdated = DateTime.Now
                    });
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Donor registered successfully! Blood inventory updated.";
                return RedirectToAction("Register");
            }

            return View(donor);
        }

        // ✅ View Registered Donors (Admin Access Only)
        public IActionResult ViewRegisteredDonors()
        {
            var donors = _context.Donors.AsNoTracking()
                                        .OrderByDescending(d => d.LastDonationDate)
                                        .ToList();
            return View(donors);
        }

        // ✅ Display Donation History (Admin Access Only)
        public IActionResult History()
        {
            var donorHistory = _context.Donors.AsNoTracking()
                                              .OrderByDescending(d => d.LastDonationDate)
                                              .ToList();
            return View(donorHistory);
        }

        // ✅ Delete Donor Record (Admin Access Only)
        public IActionResult Delete(int id)
        {
            var donor = _context.Donors.Find(id);
            if (donor == null) return NotFound();

            _context.Donors.Remove(donor);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Donor record deleted successfully!";
            return RedirectToAction("ViewRegisteredDonors");
        }
    }
}
