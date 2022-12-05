using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class OwnersController : Controller
    {
        // Вывод всех хозяев на экран
        [HttpGet]
        public IActionResult AllOwners()
        {
            return View();
        }
    }
}
