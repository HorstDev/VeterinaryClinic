using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;

namespace VeterinaryClinic.Data.Repositories
{
    public class ServiceRepository : IBaseRepository<Service>
    {
        private readonly ClinicDataContext _db;

        public ServiceRepository(ClinicDataContext db)
        {
            _db = db;
        }

        public async Task Create(Service entity)
        {
            await _db.Services.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Service> GetAll()
        {
            return _db.Services;
        }

        public async Task Delete(Service entity)
        {
            _db.Services.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Service> Update(Service entity)
        {
            _db.Services.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
