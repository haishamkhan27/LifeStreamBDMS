using Microsoft.AspNetCore.Mvc;
using LifeStreamBDMS.Data;
using LifeStreamBDMS.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LifeStreamBDMS.Controllers
{
    public class BloodBankController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BloodBankController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Show Inventory List
        public IActionResult Inventory()
        {
            var inventoryList = _context.BloodInventories.OrderByDescending(b => b.LastUpdated).ToList();
            return View(inventoryList);
        }

        // ✅ Create Blood Inventory Entry
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BloodInventory inventory)
        {
            if (ModelState.IsValid)
            {
                inventory.UpdateStatus(); // Auto-set status before saving
                _context.BloodInventories.Add(inventory);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Blood inventory updated successfully!";
                return RedirectToAction("Inventory");
            }
            return View(inventory);
        }

        // ✅ Edit Blood Inventory Entry
        public IActionResult Edit(int id)
        {
            var inventory = _context.BloodInventories.Find(id);
            if (inventory == null) return NotFound();
            return View(inventory);
        }

        public IActionResult ManageInventory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BloodInventory inventory)
        {
            if (ModelState.IsValid)
            {
                inventory.UpdateStatus(); // Auto-update status
                _context.BloodInventories.Update(inventory);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Inventory updated successfully!";
                return RedirectToAction("Inventory");
            }
            return View(inventory);
        }

        // GET: BloodBank/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventory = _context.BloodInventories.FirstOrDefault(b => b.Id == id);
            if (inventory == null) return NotFound();

            return View(inventory); // shows confirmation
        }

        // POST: BloodBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var inventory = _context.BloodInventories.Find(id);
            if (inventory == null) return NotFound();

            _context.BloodInventories.Remove(inventory);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Blood inventory deleted successfully!";
            return RedirectToAction("Inventory");
        }

        // ==================== API ENDPOINTS ====================

        /// <summary>
        /// Gets all blood inventory items
        /// </summary>
        [HttpGet("api/inventory")]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventory = await _context.BloodInventories
                .OrderByDescending(b => b.LastUpdated)
                .ToListAsync();
            return Ok(inventory);
        }

        /// <summary>
        /// Gets blood inventory by blood type
        /// </summary>
        [HttpGet("api/inventory/{bloodType}")]
        public async Task<IActionResult> GetInventoryByBloodType(string bloodType)
        {
            var inventory = await _context.BloodInventories
                .Where(b => b.BloodType == bloodType)
                .OrderByDescending(b => b.LastUpdated)
                .ToListAsync();

            if (!inventory.Any())
            {
                return NotFound($"No inventory found for blood type {bloodType}");
            }

            return Ok(inventory);
        }

        /// <summary>
        /// Gets blood inventory by status
        /// </summary>
        [HttpGet("api/inventory/status/{status}")]
        public async Task<IActionResult> GetInventoryByStatus(string status)
        {
            var inventory = await _context.BloodInventories
                .Where(b => b.Status == status)
                .OrderByDescending(b => b.LastUpdated)
                .ToListAsync();

            if (!inventory.Any())
            {
                return NotFound($"No inventory found with status {status}");
            }

            return Ok(inventory);
        }

        /// <summary>
        /// Gets critical inventory items (low stock)
        /// </summary>
        [HttpGet("api/inventory/critical")]
        public async Task<IActionResult> GetCriticalInventory()
        {
            var criticalInventory = await _context.BloodInventories
                .Where(b => b.Status == "Critical")
                .OrderByDescending(b => b.LastUpdated)
                .ToListAsync();

            return Ok(criticalInventory);
        }
    }
}