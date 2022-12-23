using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Data.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public List<int> ServicesId { get; set; }

        public Doctor()
        {
            ServicesId = new List<int>();
        }
    }
}
