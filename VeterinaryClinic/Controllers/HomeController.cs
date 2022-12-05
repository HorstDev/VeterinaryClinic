using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class HomeController : Controller
    {
        // Главная
        [HttpGet]
        public IActionResult General()
        {
            return View();
        }

        // Услуги
        [HttpGet]
        public IActionResult Services()
        {
            return View();
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}