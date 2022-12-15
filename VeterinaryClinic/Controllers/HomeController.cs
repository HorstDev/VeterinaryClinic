using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseRepository<Service> _serviceRepository;

        public HomeController(IBaseRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        // Главная
        [HttpGet]
        public IActionResult General()
        {
            List<Service> services = new List<Service>();
            services = _serviceRepository.GetAll().ToList();
            // по 3 услуги в каждом элементе списка
            List<List<Service>> servicesForView = Service.servicesForView(services);

            return View(servicesForView);
        }

        [HttpGet]
        public IActionResult AboutService(int id)
        {
            Service service = _serviceRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            return View(service);
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}