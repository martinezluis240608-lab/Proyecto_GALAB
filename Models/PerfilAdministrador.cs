namespace Proyecto_GALAB.Models;

/// <summary>
/// Perfil del administrador (sin datos escolares de alumno).
/// </summary>
public class PerfilAdministrador
{
    public string NombreCompleto { get; set; } = "Administrador";
    public string Curp { get; set; } = string.Empty;
    public string FechaNacimiento { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = "admin@itsmg.edu.mx";
    public string Calle { get; set; } = string.Empty;
    public string Colonia { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string RutaFotoPerfil { get; set; } = string.Empty;
}
