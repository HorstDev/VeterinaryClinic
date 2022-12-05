using Microsoft.EntityFrameworkCore;

namespace VeterinaryClinic.Data
{
    public class ClinicDataContext : DbContext
    {
        public ClinicDataContext(DbContextOptions<ClinicDataContext> options)
            : base(options)
        {

        }

        // Сразу в базу данных добавляем роли
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string petOwnerRoleName = "PetOwner";
            const string adminRoleName = "Admin";

            Role PetOwnerRole = new Role { Id = 1, Name = petOwnerRoleName };
            Role AdminRole = new Role { Id = 2, Name = adminRoleName };

            modelBuilder.Entity<Role>().HasData(new Role[] { PetOwnerRole, AdminRole });

            modelBuilder.UseSerialColumns();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
