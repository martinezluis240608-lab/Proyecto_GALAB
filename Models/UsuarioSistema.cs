namespace Proyecto_GALAB.Models;

public class UsuarioSistema
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = "Usuario";
    public string Estado { get; set; } = "Activo";

    // Campos detallados adicionales
    public string Nombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty; // nombre de usuario para login
    public string Contrasena { get; set; } = string.Empty;
    public string Semestre { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public int? NumeroAsiento { get; set; }
    public long NumeroControl { get; set; }
}
