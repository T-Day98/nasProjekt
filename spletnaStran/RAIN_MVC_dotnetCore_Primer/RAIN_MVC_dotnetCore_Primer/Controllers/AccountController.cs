using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;
using Vaja3.Models;
using Microsoft.EntityFrameworkCore;

namespace Vaja3.Controllers
{
    

    public class AccountController : Controller
    {
        private readonly DBContext _context;

        public IActionResult Login() {
            return View();
        }
        public AccountController(DBContext context)
        {
            _context = context;
        }
        String vrsta;

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password) {
            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)) {
                return RedirectToAction("Login");
            }
            HttpContext.Session.Clear();
            byte[] hashedBytes;
            String hash;

            using (var sha256 = SHA256.Create())
            {
                hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));    //geslo pretvorimo v HASH
                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

            Racun account = await _context.Racun.SingleOrDefaultAsync(m => m.upime == userName); //najdemo racun iz baze po usernamu (ID
            if (account == null)
            {
                return NotFound();
            }
            if(account.upime == userName && account.geslo == hash) //preverimo ali je pravilno geslo za ta up. ime
            {
                if(account.vrsta == "admin")
                {
                    vrsta = "admin";
                }
                else
                {
                    vrsta = "user";
                }
            }


            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            // Admin
            if (vrsta == "admin") {
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            // User
            if (vrsta == "user") {
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            if (isAuthenticated) {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}