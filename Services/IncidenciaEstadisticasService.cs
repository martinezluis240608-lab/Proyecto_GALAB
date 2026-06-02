using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Servicio temporal sin base de datos.
/// Cuando exista conexión, reemplazar consultas aquí (o crear IncidenciaEstadisticasServiceDb).
/// </summary>
public sealed class IncidenciaEstadisticasService : IIncidenciaEstadisticasService
{
    public IncidenciaResumenEstadisticas ObtenerResumen()
    {
        // TODO(BD): consultar COUNT por estado desde SQL Server / API.
        return IncidenciaListadoStore.ObtenerResumen();
    }
}
