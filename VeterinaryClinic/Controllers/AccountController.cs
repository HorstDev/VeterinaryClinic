using VeterinaryClinic.Models;
using VeterinaryClinic.ViewModels;
using VeterinaryClinic.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace VeterinaryClinic.Controllers
{
    public class AccountController : Controller
    {
        private ClinicDataContext _db;

        public AccountController(ClinicDataContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = (await _db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password))!;

                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("General", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = (await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email))!;
                if (user == null)
                {
                    // Присваиваем роль
                    user = new User { Email = model.Email, Password = model.Password };
                    Role userRole = (await _db.Roles.FirstOrDefaultAsync(r => r.Name == "PetOwner"))!;
                    if (userRole != null)
                        user.Role = userRole;

                    //Добавляем в БД
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("General", "Home");
                }
                else
                    ModelState.AddModelError("", "Такой пользователь уже существует!");
            }
            return View(model);
        }

        // Аутентификация пользователя
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email!),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name!)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
