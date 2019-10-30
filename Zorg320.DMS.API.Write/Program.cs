using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace Zorg320.DMS.API.Write
{
    /// <summary>
    /// Lancement de l'API web
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configuration de l'application
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
         .AddEnvironmentVariables()
         .Build();

        /// <summary>
        /// Démarrage
        /// </summary>
        /// <param name="args">Arguements</param>
        public static int Main(string[] args)
        {
                //Création du logger Serilog
                Log.Logger = new LoggerConfiguration()
                     .ReadFrom.Configuration(Configuration)
                     .Enrich.FromLogContext()
                     .WriteTo.Debug()
                     .WriteTo.Console(
                         outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                     .CreateLogger();

                try
                {
                    Log.Information("Démarrage de l'application");
                    //Création du host + build + run
                    CreateWebHostBuilder(args).Build().Run();
                    //Retour dans un cas ou tout se passe bien
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Erreur fatal !");
                    return 1;
                }
                finally
                {
                    Log.CloseAndFlush();
                } 
            }
            /// <summary>
            /// Constructeur du host
            /// </summary>
            /// <param name="args">Arguments</param>
            /// <returns>Le host à builder et a runner</returns>
            public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog();
        }
    }
