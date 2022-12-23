using System.ComponentModel.DataAnnotations;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;
using VeterinaryClinic.Data.Repositories;

namespace VeterinaryClinic.ViewModels
{
    public class AppointmentModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Имя должно содержать более 1 символа!")]
        public string? Name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Фамилия должна содержать более 1 символа!")]
        public string? Surname { get; set; }

        [Required]
        public string? TypeOfAnimal { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        [Required]
        public string? TypeOfService { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public DateTime DateTimeEnd { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

        public string? Note { get; set; }

        // Костыль
        public string? TimeInString { get; set; }

        public DateTime Kostilb(DateTime datetime, string time)
        {
            DateTime newDateTime = Convert.ToDateTime(time);
            DateTime result = new DateTime(datetime.Year, datetime.Month, datetime.Day, newDateTime.Hour, newDateTime.Minute, newDateTime.Second);
            return result;
        }

        public bool EverythingIsFilledIn()
        {
            return (Name != null && Surname != null && TypeOfAnimal != null && Email != null && TypeOfService != null && Doctor != null && TimeInString != null);
        }

        // Возвращает список доступных времен для 
        public List<DateTime> AvailableTimes(IBaseRepository<Service> serviceRepository, IBaseRepository<Appointment> appointmentRepository)
        {
            List<DateTime> availableTimes = new List<DateTime>();
            // Сколько минут добавляет к каждому моменту времени (в зависимости от услуги время разное, т.к разные услуги длятся по-разному)
            int toAddMinutes = serviceRepository.GetAll().FirstOrDefault(x => x.Name == TypeOfService)!.ReceptionTimeMinutes;
            DateTime datetime = DateTime; // время
            DateTime datetimeTemp; // Время, на 15 минут большее datetime (список времен будет через каждые 15 минут)
            datetime = datetime.AddHours(8); // начинаем работу с 8 часов утра
            do
            {
                datetimeTemp = datetime.AddMinutes(15);
                // Ищем услугу, у которой имя доктора совпадает с уже введенными именем доктора, статус заявки != отклонен И (если начало услуги попадает в временной промежуток РАНЕЕ созданной услуги ИЛИ конец услуги попадает в временной промежуток РАНЕЕ созданной услуги)
                Appointment? temp = appointmentRepository.GetAll().FirstOrDefault(x => (x.Doctor.FullName == Doctor.FullName && x.StatusCode != AppointmentStatus.Rejected && 
                            ((datetime >= x.DateTimeStart && datetime < x.DateTimeEnd) || (datetime.AddMinutes(toAddMinutes) > x.DateTimeStart && datetime.AddMinutes(toAddMinutes) < x.DateTimeEnd))));
                if (temp == null) // если такая услуга не найдена, соответственно время не занято
                    availableTimes.Add(datetime);

                datetime = datetime.AddMinutes(15);
            } while (datetimeTemp.Hour < 20);

            return availableTimes;
        }
    }

    public class AppointmentFormModel
    {
        public AppointmentModel AppointmentModel { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Service> Services { get; set; }
        public List<DateTime> Times { get; set; }

        public static explicit operator Appointment(AppointmentFormModel model)
        {
            return new Appointment
            {
                Name = model.AppointmentModel.Name,
                Surname = model.AppointmentModel.Surname,
                TypeOfAnimal = model.AppointmentModel.TypeOfAnimal,
                Email = model.AppointmentModel.Email,
                TypeOfService = model.AppointmentModel.TypeOfService,
                DateTimeStart = model.AppointmentModel.DateTime,
                DateTimeEnd = model.AppointmentModel.DateTimeEnd,
                Doctor = model.AppointmentModel.Doctor,
                Note = model.AppointmentModel.Note
            };
        }
    }
}
