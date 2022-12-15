using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;

namespace VeterinaryClinic.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModificationController : Controller
    {
        private readonly IBaseRepository<Service> _serviceRepository;

        public ModificationController(IBaseRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public IActionResult Modifications()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View(new Service());
        }

        [HttpPost]
        public IActionResult AddService(Service service)
        {
            _serviceRepository.Create(service);
            return RedirectToAction("General", "Home");
        }

        [HttpPost]
        public IActionResult RemoveService(Service service) // сюда поступает только название
        {
            Service toRemoveService = _serviceRepository.GetAll().FirstOrDefault(x => x.Name == service.Name)!;
            _serviceRepository.Delete(toRemoveService);
            return RedirectToAction("General", "Home");
        }
    }
}
