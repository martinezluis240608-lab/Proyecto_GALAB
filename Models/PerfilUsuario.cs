namespace Proyecto_GALAB.Models;

public class PerfilUsuario
{
    public string NombreCompleto { get; set; } = "Nombre del usuario";
    public string Correo { get; set; } = "correo@institucion.edu.mx";
    public string Rol { get; set; } = "Rol del usuario";
    public string Carrera { get; set; } = "Carrera del usuario";
    public string RutaFotoPerfil { get; set; } = string.Empty;
}
