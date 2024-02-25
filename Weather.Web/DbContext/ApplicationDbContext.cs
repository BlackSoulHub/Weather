using Microsoft.EntityFrameworkCore;
using Weather.Web.Entity;

namespace Weather.Web.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt)
    : Microsoft.EntityFrameworkCore.DbContext(opt)
{

    public required DbSet<WeatherRecord> Records { get; init; }
}