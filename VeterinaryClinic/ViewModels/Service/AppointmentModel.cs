using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.ViewModels.Service
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

        public string? Note { get; set; }
    }
}
