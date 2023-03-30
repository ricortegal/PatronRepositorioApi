
using Entidades.Base;
using Entidades.Sensores;
using Newtonsoft.Json;
using Repositorios.Base;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repositorios.Sensores
{
    public class SensorRepositorioMemory : IRepositorio<Sensor, Guid>
    {
    ¡


        #region variables privadas
        private static string _archivo = string.Empty;
        private List<Sensor> _sensores= new List<Sensor>();
        private bool disposedValue;
        #endregion


        #region constructor
        public SensorRepositorioMemory(string archivo)
        {
          
        }
        #endregion


        #region IRepositorio
        public Guid Guardar(Sensor entidad)
        {
            var sensor = _sensores.FirstOrDefault(s => s.Id == entidad.Id);
            if(sensor == null)
            {
                entidad.Id  = Guid.NewGuid();
                _sensores.Add(entidad);
                return entidad.Id;
            }
            sensor.Nombre = entidad.Nombre;
            sensor.UnidadMedida = entidad.UnidadMedida;
            sensor.Valor = entidad.Valor;
            return sensor.Id;
        }

        
        public Sensor? Obtener(Guid id)
        {
            return _sensores.FirstOrDefault(s => s.Id == id);
        }

        public IList<Sensor> Obtener(Func<Sensor, bool> consulta)
        {
            return _sensores.Where(consulta).ToList();
        }

        public IList<Sensor> Obtener()
        {
            return _sensores.ToList();
        }
        #endregion


    }
}
