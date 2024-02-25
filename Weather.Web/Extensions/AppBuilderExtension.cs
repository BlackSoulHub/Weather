using Microsoft.EntityFrameworkCore;
using Weather.Web.DbContext;

namespace Weather.Web.Extensions;

public static class AppBuilderExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var logService = serviceScope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        logService.LogInformation("Начало применения миграций");
        dbContext.Database.Migrate();
        logService.LogInformation("Миграции успешно применены");
    }
}