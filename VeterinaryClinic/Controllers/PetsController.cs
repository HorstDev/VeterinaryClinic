using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class PetsController : Controller
    {
        // Вывод всех питомцев на экран
        [HttpGet]
        public IActionResult AllPets()
        {
            return View();
        }
    }
}
