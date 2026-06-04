namespace Proyecto_GALAB.Models;

/// <summary>
/// Fila del listado administrativo de incidencias.
/// </summary>
public class IncidenciaListadoItem
{
    public string Folio { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string QuienReporta { get; set; } = string.Empty;
    public string TipoIncidencia { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activa";
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Descripcion { get; set; } = string.Empty;
    public string Equipo { get; set; } = string.Empty;
    public string DescripcionSolucion { get; set; } = string.Empty;
}
