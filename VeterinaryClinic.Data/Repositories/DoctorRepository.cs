using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Data.Interfaces;

namespace VeterinaryClinic.Data.Repositories
{
    public class DoctorRepository : IBaseRepository<Doctor>
    {
        private readonly ClinicDataContext _db;

        public DoctorRepository(ClinicDataContext db)
        {
            _db = db;
        }

        public async Task Create(Doctor entity)
        {
            await _db.Doctors.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Doctor> GetAll()
        {
            return _db.Doctors;
        }

        public async Task Delete(Doctor entity)
        {
            _db.Doctors.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Doctor> Update(Doctor entity)
        {
            _db.Doctors.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
