namespace Proyecto_GALAB.Models;

/// <summary>
/// Contadores mostrados en tarjetas de gestión de incidencias.
/// </summary>
public class IncidenciaResumenEstadisticas
{
    public int Total { get; set; }
    public int Activas { get; set; }
    public int EnProceso { get; set; }
    public int Resueltas { get; set; }
}
