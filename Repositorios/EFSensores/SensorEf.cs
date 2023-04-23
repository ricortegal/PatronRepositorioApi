using System;
using System.Collections.Generic;

namespace Repositorios.EFSensores;

public partial class SensorEf
{
    public string Id { get; set; } = null!;
    public string? Nombre { get; set; }
    public double? Valor { get; set; }
    public string? UnidadMedida { get; set; }
}
