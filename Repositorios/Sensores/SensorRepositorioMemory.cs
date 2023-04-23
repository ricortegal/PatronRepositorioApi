
using Entidades.Base;
using Entidades.Sensores;
using Newtonsoft.Json;
using Repositorios.Base;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repositorios.Sensores
{
    public class SensorRepositorioMemory : IRepositorio<SensorEntity, Guid>
    {
   

        #region campos privados
        private readonly List<SensorEntity> _sensores;
        #endregion


        #region constructor
        public SensorRepositorioMemory(List<SensorEntity> sensores)
        {
            _sensores = sensores;
        }
        #endregion


        #region IRepositorio
        public Guid Guardar(SensorEntity entidad)
        {
            var sensor = _sensores.FirstOrDefault(s => s.Id == entidad.Id);
            if(sensor == null)
            {
                entidad.Id = Guid.NewGuid();
                _sensores.Add(entidad);
                return entidad.Id;
            }
            sensor.Nombre = entidad.Nombre;
            sensor.UnidadMedida = entidad.UnidadMedida;
            sensor.Valor = entidad.Valor;
            return sensor.Id;
        }

        
        public SensorEntity? Obtener(Guid id)
        {
            return _sensores.FirstOrDefault(s => s.Id == id);
        }


        public IList<SensorEntity> Obtener(Expression<Func<SensorEntity, bool>> consulta)
        {
            return _sensores.Where(consulta.Compile()).ToList();
        }


        public IList<SensorEntity> Obtener()
        {
            return _sensores.ToList();
        }

        public bool Borrar(Guid id)
        {
            var sensor = Obtener(id);
            if(sensor != null)
            {
                _sensores.Remove(sensor);
                return true;
            }
            return false;
        }
        #endregion


    }
}
