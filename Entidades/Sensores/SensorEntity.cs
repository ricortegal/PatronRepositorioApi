using Entidades.Base;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Entidades.Sensores
{
    public class SensorEntity : IBaseSensor
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("valor")]
        public double? Valor { get; set; }
        [JsonProperty("unidadmedida")]
        public string UnidadMedida { get; set; }

        public SensorEntity() { 
            Nombre= string.Empty;
            Id = default;    
            Valor = null;
            UnidadMedida= string.Empty;
        }
    }
}
