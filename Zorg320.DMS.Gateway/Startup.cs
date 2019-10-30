using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

namespace Zorg320.DMS.Gateway
{
    /// <summary>
    /// Classe de configuration du Host
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration de l'application
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructeur de la classe de configuration
        /// </summary>
        /// <param name="configuration">Injection de la configuration de l'application</param>
        public Startup(IConfiguration configuration)
        {
            //Configuration
            _configuration = configuration;
        }
        /// <summary>
        /// Classe appelé par le runtime core , permet d'ajouter des service dans le conteneur
        /// </summary>
        /// <param name="services">Collection de service </param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Ajout du service Ocelot
            services.AddOcelot(_configuration);
            //Ajout de swagger pour ocelot
            services.AddSwaggerForOcelot(_configuration);
            //Ajout du service MVC en compatibilité 2.2
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// Appelé par le runtime, permet de configurer le pipeline HTTP
        /// </summary>
        /// <param name="app">Constructeur de l'application</param>
        /// <param name="env">Environnement d'hébergement</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseOcelot().Wait();

            app.UseMvc();
            app.UseSerilogRequestLogging();
   
        }
    }
}
