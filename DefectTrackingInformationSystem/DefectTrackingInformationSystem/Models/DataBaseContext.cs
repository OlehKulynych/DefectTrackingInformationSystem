using Microsoft.EntityFrameworkCore;

namespace DefectTrackingInformationSystem.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
