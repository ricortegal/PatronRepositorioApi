using Entidades.Sensores;
using Microsoft.AspNetCore.Mvc;
using Repositorios.Base;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PatronRepositorioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensoresController : ControllerBase
    {

        private readonly IRepositorio<SensorEntity, Guid> _repositorio;


        public SensoresController(IRepositorio<SensorEntity, Guid> repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: api/<SensoresController>
        [HttpGet]
        public IEnumerable<SensorEntity> Get()
        {
            return _repositorio.Obtener();
        }

        // GET api/<SensoresController>/5
        [HttpGet("{id}")]
        public SensorEntity? Get(Guid id)
        {
            return _repositorio.Obtener(id);
        }

        // POST api/<SensoresController>
        [HttpPost]
        public SensorEntity? Post([FromBody] SensorEntity value)
        {
            var id = _repositorio.Guardar(value);
            return _repositorio.Obtener(id);
        }

        // PUT api/<SensoresController>
        [HttpPut]
        public SensorEntity? Put([FromBody] SensorEntity value)
        {
            var id = _repositorio.Guardar(value);
            return _repositorio.Obtener(id);
        }

        // DELETE api/<SensoresController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            return _repositorio.Borrar(id);
        }
    }
}
