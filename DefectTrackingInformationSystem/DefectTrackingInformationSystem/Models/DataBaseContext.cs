using Microsoft.EntityFrameworkCore;

namespace DefectTrackingInformationSystem.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Defect> Defectes { get; set; }
    }
}
