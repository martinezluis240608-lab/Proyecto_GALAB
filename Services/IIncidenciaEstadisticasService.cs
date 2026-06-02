using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Fuente de estadísticas de incidencias.
/// Implementación actual: valores en cero hasta conectar base de datos.
/// </summary>
public interface IIncidenciaEstadisticasService
{
    IncidenciaResumenEstadisticas ObtenerResumen();
}
