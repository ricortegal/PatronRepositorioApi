using Entidades.Sensores;

using Servicios.Serializacion;

namespace PatronRepositorioApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var folderExec = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var archivoSensores = $"{folderExec}{Path.DirectorySeparatorChar}sensores.json";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<List<Sensor>>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHostedService<SerializarArchivoWorker>( services =>
            {
                var sensoresLista = services.GetService<List<Sensor>>()
                return new SerializarArchivoWorker(archivoSensores);
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();


            app.Run();
        }
    }
}