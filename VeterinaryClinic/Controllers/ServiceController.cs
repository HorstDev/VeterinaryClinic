using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;
using VeterinaryClinic.ViewModels;

namespace VeterinaryClinic.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IBaseRepository<Service> _serviceRepository;
        private readonly IBaseRepository<Appointment> _appointmentRepository;
        private readonly IBaseRepository<Doctor> _doctorRepository;


        public ServiceController(IBaseRepository<Service> serviceRepository, IBaseRepository<Appointment> appointmentRepository, IBaseRepository<Doctor> doctorRepository)
        {
            _serviceRepository = serviceRepository;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public IActionResult AboutService(int id)
        {
            Service service = _serviceRepository.GetAll().FirstOrDefault(x => x.Id == id)!;
            return View(service);
        }

        [HttpGet]
        public IActionResult MakeAnAppointment()
        {
            AppointmentFormModel model = new AppointmentFormModel()
                { AppointmentModel = new AppointmentModel() };

            model.Services = _serviceRepository.GetAll().ToList();
            model.Doctors = _doctorRepository.GetAll().ToList();
            return View(model);
        }

        // После того, как мы получили врача и дату приема, выполняется этот метод
        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(AppointmentFormModel model)
        {
            // Если все заполнено, отправляем услугу в БД
            if (model.AppointmentModel.EverythingIsFilledIn())
            {
                model.AppointmentModel.DateTime = model.AppointmentModel.Kostilb(model.AppointmentModel.DateTime, model.AppointmentModel.TimeInString!); // - костыль для даты и времени
                model.AppointmentModel.DateTimeEnd = model.AppointmentModel.DateTime.AddMinutes(_serviceRepository.GetAll().FirstOrDefault
                    (x => x.Name == model.AppointmentModel.TypeOfService)!.ReceptionTimeMinutes);
                model.AppointmentModel.Doctor = _doctorRepository.GetAll().FirstOrDefault(x => x.FullName == model.AppointmentModel.Doctor.FullName)!;
                Appointment appointment = (Appointment)model;
                await _appointmentRepository.Create(appointment);
                return RedirectToAction("General", "Home");
            }
            else
            {
                // Подбираем свободные времена
                model.Times = model.AppointmentModel.AvailableTimes(_serviceRepository, _appointmentRepository);
                model.Services = _serviceRepository.GetAll().ToList();
                model.Doctors = _doctorRepository.GetAll().ToList();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult FormAppointment(AppointmentModel model)
        {
            return View(model);
        }

        // Нерассмотренные заявки
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
