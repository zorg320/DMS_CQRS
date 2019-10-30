using System;
using System.Collections.Generic;
using System.Linq;
using EventFlow.DependencyInjection;
using Autofac;
using EventFlow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Zorg320.DMS.DataAccessLayer;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.AspNetCore.Extensions;
using EventFlow.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Zorg320.DMS.Events;
using Zorg320.DMS.Command;
using Zorg320.DMS.CommandHandler;
using Zorg320.DMS.EventHandlers;
using EventFlow.MongoDB.Extensions;

namespace Zorg320.DMS.API.Write
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
            //Ajout du service MVC en compatibilité 2.2
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "DMS API", Version = "v1" }))
                .AddSingleton<IDMSDBContext>(x => new DMSDBContext("mongodb://UserTest:PwdUserTest@127.0.0.1:27017/DbTest", "DbTest"))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddEventFlow()
;

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
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DMS API V1");
            });
            //Ajout de la gestion du MVC
            app.UseMvc();
        }
    }


    public static class StartupExtension
    {
        public static IServiceCollection AddEventFlow(this IServiceCollection services)
        {


            services.AddEventFlow(options =>
                                  options  .AddAspNetCore(o => o.RunBootstrapperOnHostStartup().AddDefaultMetadataProviders())
                                           .AddDefaults(typeof(CategorieCreatedEvent).Assembly)
                                           .AddDefaults(typeof(Startup).Assembly)
                                .ConfigureMongoDb("mongodb://UserTest:PwdUserTest@127.0.0.1:27017/DbTest", "DbTest")
                                .UseMongoDbEventStore()
                                .CreateResolver());

            return services;
        }
        public static IServiceCollection AddDataBase(this IServiceCollection services)
        {
            //Ajout du context bdd


            return services;

        }
    }
}
