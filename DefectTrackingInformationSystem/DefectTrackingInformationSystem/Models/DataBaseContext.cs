using Microsoft.EntityFrameworkCore;
using DTIS.Shared.Models;

namespace DefectTrackingInformationSystem.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<DTIS.Shared.Models.Defect> Defectes { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
