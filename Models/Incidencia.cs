namespace Proyecto_GALAB.Models;

public class Incidencia
{
    public string Titulo { get; set; } = string.Empty;
    public string QuienReporta { get; set; } = string.Empty;
    public string TipoIncidencia { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Descripcion { get; set; } = string.Empty;
    public string RutaEvidencia { get; set; } = string.Empty;

    public (bool valido, string mensaje) Validar()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            return (false, "El campo 'Titulo de la incidencia' es obligatorio.");
        if (string.IsNullOrWhiteSpace(QuienReporta))
            return (false, "El campo 'Quien reporta' es obligatorio.");
        if (string.IsNullOrWhiteSpace(TipoIncidencia) ||
            TipoIncidencia.StartsWith("Selecciona", StringComparison.OrdinalIgnoreCase))
            return (false, "Seleccione un tipo de incidencia.");
        if (string.IsNullOrWhiteSpace(NombreEquipo))
            return (false, "El campo 'Nombre del equipo' es obligatorio.");
        if (string.IsNullOrWhiteSpace(Descripcion))
            return (false, "La descripcion es obligatoria.");
        return (true, "OK");
    }
}
