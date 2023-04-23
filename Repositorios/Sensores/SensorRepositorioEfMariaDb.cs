using Entidades.Sensores;

using Microsoft.EntityFrameworkCore;

using Repositorios.Base;
using Repositorios.EFSensores;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Sensores
{
    public class SensorRepositorioEfMariaDb : IRepositorio<SensorEntity,Guid>
    {

        RepositoriosContext ContextoDb { get; }

        public SensorRepositorioEfMariaDb(RepositoriosContext context)
        {
            ContextoDb = context;
        }

        public bool Borrar(Guid id)
        {
            var sensor = ContextoDb.Sensores.FirstOrDefault(s => s.Id == id.ToString());
            if (sensor == null)
            {
                return false;
            }
            ContextoDb.Entry(sensor).State = EntityState.Deleted;
            ContextoDb.SaveChanges();
            return true;
        }


        public Guid Guardar(SensorEntity entidad)
        {
            var sensorEf = ContextoDb.Sensores.FirstOrDefault(sEf => sEf.Id == entidad.Id.ToString());
            if(sensorEf == null)
            {
                sensorEf = new SensorEf();
                sensorEf.Id = Guid.NewGuid().ToString();
                sensorEf.Nombre = entidad.Nombre;
                sensorEf.UnidadMedida = entidad.UnidadMedida;
                sensorEf.Valor = entidad.Valor;
                ContextoDb.Sensores.Add(sensorEf);
            }
            else
            {
                sensorEf.Nombre = entidad.Nombre;
                sensorEf.UnidadMedida = entidad.UnidadMedida;
                sensorEf.Valor = entidad.Valor;
            }
            ContextoDb.SaveChanges();
            return new Guid(sensorEf.Id);
        }


        public SensorEntity? Obtener(Guid id)
        {
            return Obtener(s => s.Id == id).FirstOrDefault();
        }


        public IList<SensorEntity> Obtener(Expression<Func<SensorEntity, bool>> consulta)
        {
            var sensorEf = ContextoDb
                            .Sensores
                            .Select(sEf => new SensorEntity()
                            {
                                Id = new Guid(sEf.Id),
                                Nombre = sEf.Nombre ?? string.Empty,
                                Valor = sEf.Valor,
                                UnidadMedida = sEf.UnidadMedida ?? string.Empty
                            })
                            .Where(consulta).ToList();
            return sensorEf;
        }


        public IList<SensorEntity> Obtener()
        {
            return Obtener(sEf => true);
        }
    }
}
