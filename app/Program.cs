using Microsoft.EntityFrameworkCore;
using ActuarialApplications.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<LocalRateDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalRateDbContext")));
builder.Services.AddDbContext<LocalLifeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalLifeDbContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedSwapData.Initialize(services);
    Contract.SeedContractData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
