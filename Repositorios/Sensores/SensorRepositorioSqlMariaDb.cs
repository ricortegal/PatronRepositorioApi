using Entidades.Sensores;

using Microsoft.Extensions.Logging;

using MySqlConnector;

using Repositorios.Base;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Sensores
{
    public class SensorRepositorioSqlMariaDb : IRepositorio<SensorEntity,Guid>
    {
        //HACER EL ACCESO MEDIANTE CONSULTAS SQL DEBE SER ALGO EXCEPCIONAL EN .NET!!!!!!!!

        private const string SQL_SELECT_ALL = "select Id,Nombre,Valor,UnidadMedida from sensores";
        private const string SQL_SELECT_ID = "select Id,Nombre,Valor,UnidadMedida from sensores where Id=@id";
        private const string SQL_UPDATE = "update sensores set Nombre=@nombre,Valor=@valor,UnidadMedida=@unidadMedida where Id=@id";
        private const string SQL_INSERT = "insert into sensores(Id,Nombre,Valor,UnidadMedida) values (@id,@nombre,@valor,@unidadMedida)";
        private const string SQL_DELETE = "delete sensores where Id=@id";

        private MySqlConnection Connection { get; }
        private ILogger Logger { get; }

        public SensorRepositorioSqlMariaDb(MySqlConnection connection, 
            ILogger<SensorRepositorioSqlMariaDb> logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public SensorEntity? Obtener(Guid id)
        {
            SensorEntity entity = new SensorEntity();

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                var command = Connection.CreateCommand();
                command.CommandText =  SQL_SELECT_ID;
                command.Parameters.Add("@id", MySqlDbType.VarChar, 45).Value = id;
                var reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                if(reader.Read())
                {
                    entity.Id = reader.GetGuid("Id");
                    entity.Nombre = reader.GetString("Nombre");
                    entity.Valor = reader.GetDouble("Valor");
                    entity.UnidadMedida = reader.GetString("UnidadMedida");
                }
                else
                {
                    return null;
                }

            }
            catch(Exception ex)
            {
                Logger.LogError(ex, "Error ");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }

            return entity;

        }


        public IList<SensorEntity> Obtener(Expression<Func<SensorEntity, bool>> consulta)
        {
            //TO DO Analizar el árbol de expresión para generar una consulta sql
            //Lo siguiente es una ñapa obtenemos todo para implementar la interfaz
            List<SensorEntity> entities = new List<SensorEntity>();

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                var command = Connection.CreateCommand();
                command.CommandText = SQL_SELECT_ALL;
                command.Parameters.Add("@id", MySqlDbType.VarChar, 45);
                var reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                while (reader.Read())
                {
                    var entity = new SensorEntity();
                    entity.Id = reader.GetGuid("Id");
                    entity.Nombre = reader.GetString("Nombre");
                    entity.Valor = reader.GetDouble("Valor");
                    entity.UnidadMedida = reader.GetString("UnidadMedida");
                    entities.Add(entity);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error ");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }

            return entities.Where(consulta.Compile()).ToList();

        }


        public IList<SensorEntity> Obtener()
        {
            return Obtener(s => true);
        }

        public Guid Guardar(SensorEntity entidad)
        {

            var entidadDb = Obtener(entidad.Id);
            if (entidadDb?.Id == null)
            {
                InsertaBd(entidad);
            }
            else
            {
                entidadDb.Nombre = entidad.Nombre;
                entidadDb.UnidadMedida = entidad.UnidadMedida;
                entidadDb.Valor = entidad.Valor;
                ActualizarBd(entidadDb);
            }

            return entidad.Id;

        }


        public bool Borrar(Guid id)
        {
  
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                var command = Connection.CreateCommand();
                command.CommandText = SQL_DELETE;
                command.Parameters.Add("@id", MySqlDbType.VarChar, 45);
                var reader = command.ExecuteNonQuery();
                return reader > 0;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error ");
                return false;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
        }


        private Guid ActualizarBd(SensorEntity entity)
        {
          
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                var command = Connection.CreateCommand();
                command.CommandText = SQL_UPDATE;
                command.Parameters.Add("@id", MySqlDbType.VarChar, 45).Value = entity.Id;
                command.Parameters.Add("@nombre", MySqlDbType.VarChar, 45).Value = entity.Nombre;
                command.Parameters.Add("@valor", MySqlDbType.Float).Value = entity.Valor;
                command.Parameters.Add("@UnidadMedida", MySqlDbType.VarChar, 45).Value = entity.UnidadMedida;
                var reader = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error ");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            return entity.Id;
        }


        private Guid InsertaBd(SensorEntity entity)
        {

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                entity.Id = Guid.NewGuid(); 

                var command = Connection.CreateCommand();
                command.CommandText = SQL_INSERT;
                command.Parameters.Add("@id", MySqlDbType.VarChar, 45).Value = entity.Id;
                command.Parameters.Add("@nombre", MySqlDbType.VarChar, 45).Value = entity.Nombre;
                command.Parameters.Add("@valor", MySqlDbType.Float).Value = entity.Valor;
                command.Parameters.Add("@UnidadMedida", MySqlDbType.VarChar, 45).Value = entity.UnidadMedida;

                var reader = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error ");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
            return entity.Id;
        }
    }
}
