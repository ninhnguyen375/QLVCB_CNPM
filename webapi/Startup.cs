using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using webapi.core.Interfaces;
using webapi.Helpers;
using webapi.infrastructure.Persistance;
using webapi.infrastructure.Persistance.Repositories;
using webapi.core.Mapping;
using webapi.Interfaces;
using webapi.Services;

namespace webapi {
    public class Startup {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors (options => {
                options.AddPolicy (MyAllowSpecificOrigins,
                    builder => {
                        builder.WithOrigins (
                                "http://localhost:3000",
                                "http://localhost:3001"
                            )
                            .AllowAnyHeader ()
                            .AllowAnyMethod ();
                    });
            });
            services.AddControllers ();
            services.AddDbContext<AppDbContext> (options =>
                options.UseSqlite ("Data Source=../webapi.infrastructure/App.db", b => b.MigrationsAssembly ("webapi.infrastructure")));

            services.AddAutoMapper (AppDomain.CurrentDomain.GetAssemblies ());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection ("AppSettings");
            services.Configure<AppSettings> (appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings> ();
            var key = Encoding.ASCII.GetBytes (appSettings.Secret);
            services.AddAuthentication (x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (x => {
                    x.Events = new JwtBearerEvents {
                        OnTokenValidated = context => {
                            var _unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork> ();
                            var userId = int.Parse (context.Principal.Identity.Name);
                            var user = _unitOfWork.Users.GetByAsync (userId);
                            if (user == null) {
                                // return unauthorized if user no longer exists
                                context.Fail ("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IUnitOfWork, UnitOfWork> ();
            services.AddScoped<IAirlineService, AirlineService> ();
            services.AddScoped<IAirportService, AirportService> ();
            services.AddScoped<ICustomerService, CustomerService> ();
            services.AddScoped<ILuggageService, LuggageService> ();
            services.AddScoped<IDateService, DateService> ();
            services.AddScoped<IFlightService, FlightService> ();
            services.AddScoped<ITicketCategoryService, TicketCategoryService> ();
            services.AddScoped<IOrderService, OrderService> ();
            services.AddScoped<IUserService, UserService> ();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseRouting ();

            // global cors policy
            app.UseCors (x => x
                .AllowAnyOrigin ()
                .AllowAnyMethod ()
                .AllowAnyHeader ());

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}