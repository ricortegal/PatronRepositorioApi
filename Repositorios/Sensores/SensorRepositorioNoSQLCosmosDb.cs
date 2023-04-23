using Entidades.Sensores;

using Microsoft.Azure.Cosmos;

using Repositorios.Base;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Container = Microsoft.Azure.Cosmos.Container;

namespace Repositorios.Sensores
{
    public class SensorRepositorioNoSQLCosmosDb : IRepositorio<SensorEntity, Guid>
    {

        private CosmosClient Client { get; }
        private Container Container { get; }

        public SensorRepositorioNoSQLCosmosDb(CosmosClient client)
        {
            Client = client;
            Container = client.GetContainer("minierp", "sensores");
        }

        public bool Borrar(Guid id)
        {
            try
            {
                if(Obtener(id) == null)
                {
                    return false;
                }

                Container.DeleteItemAsync<SensorEntity>(id: id.ToString(),
                                partitionKey: new PartitionKey(id.ToString())).Wait();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Guid Guardar(SensorEntity entidad)
        {
            var entidadDb = Obtener(entidad.Id);
            if(entidadDb == null)
            {
                entidad.Id = Guid.NewGuid();
                Container.CreateItemAsync<SensorEntity>(entidad).Wait();
            }
            else
            {
                Container.UpsertItemAsync<SensorEntity>(entidad);
            }
            return entidad.Id;
            
        }

        public SensorEntity? Obtener(Guid id)
        {
           return Obtener(s => s.Id == id).FirstOrDefault();
        }

        public IList<SensorEntity> Obtener(Expression<Func<SensorEntity, bool>> consulta)
        {
            try
            {
                var setIterator = Container.GetItemLinqQueryable<SensorEntity>(allowSynchronousQueryExecution: true);
                return setIterator.Where(consulta).ToList();
            }
            catch(Exception ex)
            {
                return new List<SensorEntity>();
            }
        }

        public IList<SensorEntity> Obtener()
        {
            return Obtener(s => true);
        }
    }
}
