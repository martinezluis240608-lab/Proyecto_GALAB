namespace Proyecto_GALAB.Models;

public class UsuarioSistema
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = "Usuario";
    public string Estado { get; set; } = "Activo";
}
