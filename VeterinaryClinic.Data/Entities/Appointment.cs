using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Data.Entities
{
    public enum AppointmentStatus
    {
        NotConsidered, // не рассмотрена
        Approved,      // одобрена
        Rejected       // отклонена
    }

    // Запись на прием
    public class Appointment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? TypeOfAnimal { get; set; }
        public string? Email { get; set; }
        public string? TypeOfService { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string? Note { get; set; }
        public int IdUser { get; set; } // Id пользователя, который оставлял заявку
        public Doctor Doctor { get; set; }
        public AppointmentStatus StatusCode { get; set; }

        public Appointment()
        {
            StatusCode = AppointmentStatus.NotConsidered;
            IdUser = -1; // -1, т.к заявку может оставлять незарегистрированный пользователь
        }

        // Одобрить запись
        public async Task Approve()
        {
            StatusCode = AppointmentStatus.Approved;
            string messageForUser = "Вы оставляли заявку на прием в ветеринарную клинику \"Когти и клыки\"\n" +
                "Информация о заявке:\n" +
                $"Имя - {Name}, Фамилия - {Surname}\n" +
                $"Заявка осуществлялась данный вид услуги: {TypeOfService}\n" +
                $"Животное, указанное в анкете - {TypeOfAnimal}\n" +
                $"Дата и время, на которое осуществлялась запись - {DateTimeStart.ToString()}\n" +
                $"Статус заявки - ОДОБРЕНА, ждем вас в указанную дату - {DateTimeStart.ToString()}";
            await EMail.SendMessage(Email!, "Ветеринарная клиника Когти и клыки", messageForUser);
        }
        // Отклонить запись
        public async Task Reject()
        {
            StatusCode = AppointmentStatus.Rejected;
            string messageForUser = "Вы оставляли заявку на прием в ветеринарную клинику \"Когти и клыки\"\n" +
                "Информация о заявке:\n" +
                $"Имя - {Name}, Фамилия - {Surname}\n" +
                $"Заявка осуществлялась данный вид услуги: {TypeOfService}\n" +
                $"Животное, указанное в анкете - {TypeOfAnimal}\n" +
                $"Дата и время, на которое осуществлялась запись - {DateTimeStart.ToString()}\n" +
                $"Статус заявки - ОТКЛОНЕНА, просим прощения за доставленные неудобства";
            await EMail.SendMessage(Email!, "Ветеринарная клиника Когти и клыки", messageForUser);
        }

    }
}
