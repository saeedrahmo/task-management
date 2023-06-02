using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.Core;
using TaskManagement.Data.EF;

namespace TaskManagement.Extentions
{
    public static class ServiceExtension
    {
        public static void ConfigureSqliteContext(this IServiceCollection services, IConfiguration configuration) =>
      services.AddDbContext<AppDbContext>(options =>
      options.UseSqlite(configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'SQLite' not found.")));


        public static void ConfigureIdentity4(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<Core.Models.ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<AppDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Configure ASP.NET Core Identity options
                // ...
            });

            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                //options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options => {                
                options.Events.OnRedirectToLogin = context =>
                {
                    // Check if the request is from MVC or API
                    var isApiRequest = context.Request.Path.StartsWithSegments("/api");

                    if (isApiRequest)
                    {
                        // Return an error code (e.g., 401 Unauthorized)
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                    else
                    {
                        // Redirect to the login page
                        context.Response.Redirect("Identity/Account/Login");
                    }

                    return Task.CompletedTask;
                };

            })
            .AddJwtBearer(options =>
            {
                // Configure JWT bearer authentication options
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }


        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<Core.Models.ApplicationUser, IdentityRole>()
    //            .AddDefaultIdentity<Models.ApplicationUser>(options =>
    //        {
    //            // Configure identity options
    //        })
    //.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options=> {
                options.Events.OnRedirectToLogin = context =>
                {
                    // Check if the request is from MVC or API
                    var isApiRequest = context.Request.Path.StartsWithSegments("/api");

                    if (isApiRequest)
                    {
                        // Return an error code (e.g., 401 Unauthorized)
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;                        
                    }
                    else
                    {
                        // Redirect to the login page
                        context.Response.Redirect("Identity/Account/Login");
                    }

                    return Task.CompletedTask;
                };

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }


        public static void ConfigureIdentity3(this IServiceCollection services)
        {
            services.AddDefaultIdentity<Core.Models.ApplicationUser>(
                options =>
                {
                    // Default User settings.
                    options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                }
            ).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                //.AddDefaultTokenProviders()
                ;
        }

        public static void ConfigureIdentity2(this IServiceCollection services)
        {
            var builder = services.AddIdentity<Core.Models.ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT3(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = jwtConfig["validIssuer"],
            //        ValidAudience = jwtConfig["validAudience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            //    };
            //});

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"],
                ValidAudience = jwtConfig["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
        }

        public static void ConfigureJWT2(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}
