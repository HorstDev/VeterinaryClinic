using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Data.Entities
{
    // Услуги
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public int Price { get; set; }

        // Возвращает по 3 услуги в одном элементе List тут много костылей
        public static List<List<Service>> servicesForView(List<Service> allServices)
        {
            List<List<Service>> result = new List<List<Service>>();
            List<Service> threeServices = new List<Service>(3);

            for (int i = 0; i < allServices.Count; i++)
            {
                if (threeServices.Count == 3)
                {
                    List<Service> toAdd = new List<Service> { threeServices[0], threeServices[1], threeServices[2]};
                    result.Add(toAdd);
                    threeServices.Clear();
                    i--;
                }
                else
                    threeServices.Add(allServices[i]);
            }

            if (threeServices.Count == 3)
            {
                List<Service> toAdd = new List<Service> { threeServices[0], threeServices[1], threeServices[2] };
                result.Add(toAdd);
                threeServices.Clear();
            }
            else if (threeServices.Count != 0)
            {
                result.Add(threeServices);
            }

            return result;
        }
    }
}
