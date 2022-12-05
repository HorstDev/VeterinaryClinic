using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
