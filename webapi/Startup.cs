using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webapi.core.Interfaces;
using webapi.infrastructure.Persistance;
using webapi.infrastructure.Persistance.Repositories;

namespace webapi {
    public class Startup {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ().AddNewtonsoftJson ();
            services.AddDbContext<AppDbContext> (options =>
                options.UseSqlite ("Data Source=../webapi.infrastructure/App.db", b => b.MigrationsAssembly ("webapi.infrastructure")));

            services.AddScoped<IUnitOfWork, UnitOfWork> ();
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseCors (MyAllowSpecificOrigins);

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}