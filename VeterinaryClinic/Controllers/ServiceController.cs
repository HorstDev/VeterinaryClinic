using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;
using VeterinaryClinic.ViewModels.Service;

namespace VeterinaryClinic.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IBaseRepository<Service> _serviceRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Appointment> _appointmentRepository;

        public ServiceController(IBaseRepository<Service> serviceRepository, IBaseRepository<User> userRepository, IBaseRepository<Appointment> appointmentRepository)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public IActionResult AboutService(int id)
        {
            Service service = _serviceRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            return View(service);
        }

        // Запись на прием
        [HttpGet]
        public IActionResult MakeAnAppointment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(AppointmentModel model)
        {
            Appointment appointment = new Appointment { 
                Name = model.Name, Surname = model.Surname, TypeOfAnimal = model.TypeOfAnimal,
                Email = model.Email, TypeOfService = model.TypeOfService, DateTime = model.DateTime, Note = model.Note };

            string? userName = null;
            if (User.Identity!.IsAuthenticated)
            {
                userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType)!.Value;
                appointment.IdUser = _userRepository.GetAll().FirstOrDefault(x => x.Email == userName)!.Id;
            }

            await _appointmentRepository.Create(appointment);

            return RedirectToAction("General", "Home");
        }

        // Не рассмотренные заявки
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ApprovalAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            appointments = _appointmentRepository.GetAll().Where(x => x.StatusCode == AppointmentStatus.NotConsidered).ToList();
            return View(appointments);
        }

        // Одобренные
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ApprovedAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            appointments = _appointmentRepository.GetAll().Where(x => x.StatusCode == AppointmentStatus.Approved).ToList();
            return View(appointments);
        }

        // Отклоненные
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RejectedAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            appointments = _appointmentRepository.GetAll().Where(x => x.StatusCode == AppointmentStatus.Rejected).ToList();
            return View(appointments);
        }

        // О заявке
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AboutAppointment(int id)
        {
            Appointment appointment = _appointmentRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            return View(appointment);
        }

        // Одобрить заявку
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            Appointment appointment = _appointmentRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            await appointment.Approve();
            await _appointmentRepository.Update(appointment);
            return RedirectToAction("ApprovalAppointments");
        }

        // Отклонить заявку
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            Appointment appointment = _appointmentRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            await appointment.Reject();
            await _appointmentRepository.Update(appointment);
            return RedirectToAction("ApprovalAppointments");
        }
    }
}
