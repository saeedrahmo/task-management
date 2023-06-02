using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskManagement.Data;
using TaskManagement.Models;

//TODO: pagination of tasks

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlite(builder.Configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'SQLite' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
// {
//     // Default User settings.
//     options.User.AllowedUserNameCharacters =
//             "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//     options.User.RequireUniqueEmail = false;
// }

//)
////.AddDefaultTokenProviders()
//.AddEntityFrameworkStores<AppDbContext>();

//services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<ApplicationUser>(
    //options => options.SignIn.RequireConfirmedAccount = true
    options =>
    {
        // Default User settings.
        options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
        // Use username as the primary identifier

        //options.SignIn.RequireConfirmedAccount = true;

        //// Password options
        //options.Password.RequireDigit = true;
        //options.Password.RequiredLength = 8;
        //options.Password.RequireUppercase = true;
        //options.Password.RequireLowercase = true;
        //options.Password.RequireNonAlphanumeric = true;

        //// Lockout options
        //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        //options.Lockout.MaxFailedAccessAttempts = 5;

        //// User options
        //options.User.RequireUniqueEmail = true;
    }
).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

SeedData.Seed(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.MapControllers();

app.Run();
