using Microsoft.EntityFrameworkCore;
using Weather.Web.DbContext;
using Weather.Web.Extensions;
using Weather.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();
services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlite("Data Source=database.db");
});

services.AddScoped<DocumentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.ApplyMigrations();
}

if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();