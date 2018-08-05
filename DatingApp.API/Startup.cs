using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        // PRODUCTION
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Specify the DB Context (Sqlite)
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DatingApp.Migrations.Production"))
                 .ConfigureWarnings( warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
                 );
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // Add support for cross origin resource support
            services.AddCors();
        }

        // DEVELOPMENT
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // Specify the DB Context (Sqlite)
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DatingApp.Migrations.Development"))
                 .ConfigureWarnings( warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
                 );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add support for cross origin resource support
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("Environment: " + (env.EnvironmentName).ToUpper());
            Console.WriteLine("=========================================");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();   this should be re-enabled when done!!
            }

            // Cross Origin Resource - Order matters - this must come before UseMvc()
            // TODO: revisit the security implications of allowing all!!!
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            // app.UseHttpsRedirection();    this should be re-enabled when done!!
            app.UseMvc();
        }
    }
}
