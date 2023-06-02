using Microsoft.AspNetCore.Identity;
using TaskManagement.Core.Models;
using TaskManagement.Data.EF;
using TaskManagement.Extentions;


//TODO: pagination of tasks
//TODO: add error message  [Required(ErrorMessage = "Username is required")]

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>
//      options.UseSqlite(builder.Configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'SQLite' not found.")));

builder.Services.ConfigureSqliteContext(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<AppDbContext>();

//// Configure JWT authentication
//builder.Services.AddAuthentication(options =>
//{
//    //options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//    //options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(options => {
//    options.Events.OnRedirectToLogin = context =>
//    {
//        // Check if the request is from MVC or API
//        var isApiRequest = context.Request.Path.StartsWithSegments("/api");

//        if (isApiRequest)
//        {
//            // Return an error code (e.g., 401 Unauthorized)
//            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//        }
//        else
//        {
//            // Redirect to the login page
//            context.Response.Redirect("Identity/Account/Login");
//        }

//        return Task.CompletedTask;
//    };

//})
//.AddJwtBearer(options =>
//{
//    // Configure JWT bearer authentication options
//    // ...
//});


//builder.Services.ConfigureIdentity();
//builder.Services.ConfigureJWT(builder.Configuration);

//builder.Services.ConfigureIdentity4(builder.Configuration);

//builder.Services.AddAuthentication()
//    .AddCookie()
//    .AddJwtBearer();





//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy =>
//        policy.RequireRole("Admin","User"));
//});

//builder.Services.AddAuthentication();//TODO: check
//builder.Services.ConfigureIdentity2();
//builder.Services.ConfigureJWT2(builder.Configuration);

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

//builder.Services.AddDefaultIdentity<ApplicationUser>(
//    //options => options.SignIn.RequireConfirmedAccount = true
//    options =>
//    {
//        // Default User settings.
//        options.User.AllowedUserNameCharacters =
//                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//        options.User.RequireUniqueEmail = false;
//        // Use username as the primary identifier

//        //options.SignIn.RequireConfirmedAccount = true;

//        //// Password options
//        //options.Password.RequireDigit = true;
//        //options.Password.RequiredLength = 8;
//        //options.Password.RequireUppercase = true;
//        //options.Password.RequireLowercase = true;
//        //options.Password.RequireNonAlphanumeric = true;

//        //// Lockout options
//        //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
//        //options.Lockout.MaxFailedAccessAttempts = 5;

//        //// User options
//        //options.User.RequireUniqueEmail = true;
//    }
//).AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

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

//app.MapControllers();

app.Run();
