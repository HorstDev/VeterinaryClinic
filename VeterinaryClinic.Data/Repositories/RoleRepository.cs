using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Data.Interfaces;

namespace VeterinaryClinic.Data.Repositories
{
    public class RoleRepository : IBaseRepository<Role>
    {
        private readonly ClinicDataContext _db;

        public RoleRepository(ClinicDataContext db)
        {
            _db = db;
        }

        public async Task Create(Role entity)
        {
            await _db.Roles.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Role> GetAll()
        {
            return _db.Roles;
        }

        public async Task Delete(Role entity)
        {
            _db.Roles.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Role> Update(Role entity)
        {
            _db.Roles.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
