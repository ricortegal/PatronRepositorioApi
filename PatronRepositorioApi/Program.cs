using Entidades.Sensores;

using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

using MySqlConnector;

using Repositorios.Base;
using Repositorios.EFSensores;
using Repositorios.Sensores;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PatronRepositorioApi
{
    public class Program
    {

        private static WebApplication? application;
        private static IServiceCollection? services;
        

        public static void Main(string[] args)
        {
            var directorioBase = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var mysqlConnectionString = "server=localhost;uid=root;pwd=alberto01;database=repositorios";

            var cosmosDbConnectionString = "AccountEndpoint=https://cosmosnerp.documents.azure.com:443/;AccountKey=ylKaOVYuKe4l8uT2UvCtKuqgVVAPrUmXepUWG7QyAqSKYj5YdMBozGVffqj8JlgzjzCRTl3LQTvlv5nWHtLGcw==;";

            var builder = WebApplication.CreateBuilder(args);

            services = builder.Services;

            builder.Services.AddSingleton<List<SensorEntity>>();

            builder.Services.AddDbContext<RepositoriosContext>(dbContextOptions => dbContextOptions
                                                                    .UseMySql(mysqlConnectionString, new MySqlServerVersion(new Version(8, 0, 30)))
                                                                    .LogTo(Console.WriteLine, LogLevel.Information)
                                                                    .EnableSensitiveDataLogging()
                                                                    .EnableDetailedErrors());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<MySqlConnection>((provider) =>
            {
                return new MySqlConnection(mysqlConnectionString);
            });


            builder.Services.AddScoped<CosmosClient>((provider)
                => new CosmosClient(cosmosDbConnectionString));
                

            //builder.Services.AddScoped<IRepositorio<SensorEntity, Guid>, SensorRepositorioMemory>();
            //builder.Services.AddScoped<IRepositorio<SensorEntity, Guid>, SensorRepositorioEfMariaDb>();
            //builder.Services.AddScoped<IRepositorio<SensorEntity, Guid>, SensorRepositorioSqlMariaDb>();
            builder.Services.AddScoped<IRepositorio<SensorEntity, Guid>, SensorRepositorioNoSQLCosmosDb>();


            application = builder.Build();

            if (application.Environment.IsDevelopment())
            {
                application.UseSwagger();
                application.UseSwaggerUI();
            }

            application.UseAuthorization();
            application.MapControllers();


            application.Lifetime.ApplicationStarted.Register(OnAppStarted);
            application.Lifetime.ApplicationStopping.Register(OnAppStopping);
            application.Lifetime.ApplicationStopped.Register(OnAppStopped);

            AppDomain.CurrentDomain.ProcessExit += AppDomain_ProcessExit;

            application.Run();
        }

        private static void AppDomain_ProcessExit(object? sender, EventArgs e)
        {

        }

        public static void OnAppStarted()
        {
            if(application == null)
            {
                return;
            }
           var sensores = application.Services.GetService<List<SensorEntity>>();
        }

        public static void OnAppStopping()    
        {
            if(application == null)
            {
                return;
            }
            var sensores = application.Services.GetService<List<SensorEntity>>();
        }

        public static void OnAppStopped()
        {
           
        }
    }

    
}