using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Base
{
    public interface IBaseSensor : IBaseEntity<Guid> 
    {
        string Nombre {get; set;}
        string UnidadMedida { get; set;}
    }
}
