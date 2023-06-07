using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Threading.Tasks.Sources;
using TaskManagement.Core.Models;
using TaskManagement.Data.EF;
using TaskManagement.Data.RepositoryBase;
using TaskManagement.Data.RepositoryEntity.IRepository;
using TaskManagement.Data.RepositoryEntity.Repository;
using TaskManagement.Data.RepositoryManager;
using TaskManagement.Services.IService;
using TaskManagement.Services.Service;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddTransient<IRepositoryManager, RepositoryManager>();
//builder.Services.AddTransient<ITaskRepository, TaskRepository>();



builder.Services.AddTransient<ITaskService, TaskService>();

builder.Services.AddScoped<IUnitOfWork>(serviceProvider =>
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();    
    return context;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlite(builder.Configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'SQLite' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

SeedData.Seed(app.Services);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
