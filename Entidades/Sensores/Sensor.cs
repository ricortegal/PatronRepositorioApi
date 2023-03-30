using Entidades.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Sensores
{
    public class Sensor : IBaseSensor
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public double? Valor { get; set; }
        public string UnidadMedida { get; set; }

        public Sensor() { 
            Nombre= string.Empty;
            Id = default;    
            Valor = null;
            UnidadMedida= string.Empty;
        }
    }
}
