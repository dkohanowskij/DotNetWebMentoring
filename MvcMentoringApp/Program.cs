using Microsoft.EntityFrameworkCore;
using MvcMentoringApp.ExceptionHandlers;
using MvcMentoringApp.Models;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NorthwindContext>(options =>
options.UseSqlServer(
       builder.Configuration.GetConnectionString("NorthwindConnectionString")));

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation($"Log from startup. Additional information: application location - {AppDomain.CurrentDomain.BaseDirectory}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
