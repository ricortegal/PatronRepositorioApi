using Entidades.Sensores;

using Repositorios.Sensores;

using Xunit;

namespace Respositorios.Tests
{
    public class SensorRepositorioFileTest
    {
        [Fact]
        public void GuardarUnSensorNuevo()
        {
            using (SensorRepositorioFile sensorRepositorio = new SensorRepositorioFile("prueba"))
            {
                sensorRepositorio.Guardar(new Sensor()
                {
                    Nombre = "Termometro",
                    UnidadMedida = "ºC",
                    Valor = 21
                });
            }
        }
    }
}