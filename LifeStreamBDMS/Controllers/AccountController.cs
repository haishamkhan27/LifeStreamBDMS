using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LifeStreamBDMS.Data;
using LifeStreamBDMS.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LifeStreamBDMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Display Login Page
        public IActionResult Login()
        {
            return View();
        }

        // ✅ Handle Login Submission (Allows Admin to use plain password)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (user != null)
            {
                Console.WriteLine($"🔍 Checking user: {user.Username}, Role: {user.Role}");

                bool isAdmin = user.Role == "Admin";
                bool isPasswordValid = isAdmin ? (model.Password == user.Password) : VerifyPassword(model.Password, user.Password);

                if (isPasswordValid)
                {
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("Username", user.Username);

                    // 🔹 Claims-based authentication (Ensures role persists)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    Console.WriteLine($"✅ Login Success - User: {user.Username}, Role: {user.Role}");

                    return isAdmin ? RedirectToAction("Dashboard", "Admin") : RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("❌ Password mismatch during login.");
                }
            }
            else
            {
                Console.WriteLine("❌ Username not found.");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        // ✅ Display User Registration Page
        public IActionResult Register()
        {
            return View();
        }

        // ✅ Handle User Registration (Users use hashed passwords)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View(model);
            }

            var newUser = new User
            {
                Username = model.Username,
                Password = HashPassword(model.Password),
                Role = "User"
            };

            Console.WriteLine($"🔑 Registering User: {model.Username} with hashed password.");
            _context.Users.Add(newUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User registered successfully!";
            return RedirectToAction("Login");
        }

        // ✅ Display Admin Registration Page (Restricted to Admins)
        [Authorize(Roles = "Admin")]
        public IActionResult RegisterAdmin()
        {
            return View("Register"); // ✅ Use existing "Register.cshtml" view
        }

        // ✅ Handle Admin Registration (Admins have plain-text passwords)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAdmin(RegisterViewModel model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View("Register"); // ✅ Redirect to "Register.cshtml" if validation fails
            }

            var newAdmin = new User
            {
                Username = model.Username,
                Password = model.Password, // Admins have plain-text passwords
                Role = "Admin"
            };

            Console.WriteLine($"🔑 Registering Admin: {model.Username} with password: {model.Password}");
            _context.Users.Add(newAdmin);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Admin registered successfully!";
            return RedirectToAction("Dashboard", "Admin");
        }


        // ✅ Handle Logout (Clears authentication)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ✅ Password Hashing for Users (Admins use plain-text passwords)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // ✅ Verify User Passwords (Admins use direct comparison)
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword);

            Console.WriteLine($"🔑 Comparing User Passwords:\nStored: {storedPassword}\nEntered (Hashed): {hashedEnteredPassword}");

            return hashedEnteredPassword == storedPassword;
        }
    }
}
