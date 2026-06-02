using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Listado en memoria de incidencias (admin y reportes).
/// TODO(BD): reemplazar por repositorio SQL.
/// </summary>
internal static class IncidenciaListadoStore
{
    private static readonly List<IncidenciaListadoItem> Items = new();
    private static int _contador;

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerTodas() => Items.ToList();

    public static void RegistrarDesdeIncidencia(Incidencia incidencia)
    {
        _contador++;
        Items.Add(new IncidenciaListadoItem
        {
            Folio = $"INC-{DateTime.Now.Year}-{_contador:D4}",
            Titulo = incidencia.QuienReporta,
            TipoIncidencia = incidencia.TipoIncidencia,
            Estado = "Activa",
            Fecha = incidencia.FechaHora,
            Descripcion = incidencia.Descripcion,
            Equipo = incidencia.NombreEquipo
        });
    }

    public static bool Eliminar(string folio)
    {
        var item = Items.FirstOrDefault(i => i.Folio == folio);
        if (item == null) return false;
        Items.Remove(item);
        return true;
    }

    public static void Actualizar(IncidenciaListadoItem actualizado)
    {
        var idx = Items.FindIndex(i => i.Folio == actualizado.Folio);
        if (idx >= 0)
            Items[idx] = actualizado;
    }

    public static IncidenciaResumenEstadisticas ObtenerResumen()
    {
        return new IncidenciaResumenEstadisticas
        {
            Total = Items.Count,
            Activas = Items.Count(i => i.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase)),
            EnProceso = Items.Count(i => i.Estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase)),
            Resueltas = Items.Count(i => i.Estado.Equals("Resuelta", StringComparison.OrdinalIgnoreCase))
        };
    }
}
