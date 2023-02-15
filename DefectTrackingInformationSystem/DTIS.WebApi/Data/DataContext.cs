using DTIS.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DTIS.WebApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Defect> Defects { get; set; }
}
