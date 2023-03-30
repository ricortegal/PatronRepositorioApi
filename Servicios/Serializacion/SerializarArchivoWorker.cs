using Entidades.Sensores;

using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Serializacion
{
    public class SerializarArchivoWorker : IHostedService, IDisposable
    {
        private bool disposedValue;

        private List<Sensor> Sensores { get; set; }
        private string Archivo { get; }
        private FileStream Flujo { get; set; }

        private Timer? _timer = null;

        public SerializarArchivoWorker(string? archivo, List<Sensor>? sensores)
        {
            if (string.IsNullOrEmpty(archivo) || sensores != null)
                throw new Exception("el archivo no puede ser nulo");
           Sensores = sensores;
           Flujo = File.Open(archivo, FileMode.Create, FileAccess.ReadWrite);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Serializa, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            Serializa(null);
            return Task.CompletedTask;
        }


        private void Serializa(object? state)
        {
            using (var sw = new StreamWriter(Flujo))
            {
                Flujo.Position = 0;
                var json = JsonConvert.SerializeObject(Sensores);
                sw.Write(json);
            }
        }

        protected virtual async Task Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Serializa(null);
                }
                disposedValue = true;
            }
        }


        void IDisposable.Dispose()
        {
            Dispose(disposing: true).Wait();
            GC.SuppressFinalize(this);
        }
    }
}
