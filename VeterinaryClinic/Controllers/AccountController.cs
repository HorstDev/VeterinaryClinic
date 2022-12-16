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
using VeterinaryClinic.Data.Repositories;
using VeterinaryClinic.Data.Interfaces;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<Appointment> _appointmentRepository;

        public AccountController(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository, IBaseRepository<Appointment> appointmentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _appointmentRepository = appointmentRepository;
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
                User user = (await _userRepository.GetAll()
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
                User user = (await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == model.Email))!;
                if (user == null)
                {
                    // Присваиваем роль
                    user = new User { Email = model.Email, Password = model.Password };
                    Role userRole = (await _roleRepository.GetAll().FirstOrDefaultAsync(r => r.Name == "PetOwner"))!;
                    if (userRole != null)
                        user.Role = userRole;

                    //Добавляем в БД
                    await _userRepository.Create(user);

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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("General", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
