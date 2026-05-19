namespace Proyecto_GALAB.Models;

public class Incidencia
{
    public string QuienReporta   { get; set; } = string.Empty;
    public string TipoIncidencia { get; set; } = string.Empty;
    public string NombreEquipo   { get; set; } = string.Empty;
    public DateTime FechaHora    { get; set; } = DateTime.Now;
    public string Descripcion    { get; set; } = string.Empty;
    public string RutaEvidencia  { get; set; } = string.Empty;

    public (bool valido, string mensaje) Validar()
    {
        if (string.IsNullOrWhiteSpace(QuienReporta))
            return (false, "El campo 'Quién reporta' es obligatorio.");
        if (string.IsNullOrWhiteSpace(NombreEquipo))
            return (false, "El campo 'Nombre del equipo' es obligatorio.");
        if (string.IsNullOrWhiteSpace(Descripcion))
            return (false, "La descripción es obligatoria.");
        return (true, "OK");
    }
}
