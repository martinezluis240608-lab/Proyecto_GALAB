namespace Proyecto_GALAB.Models;

public class PerfilUsuario
{
    public string NombreCompleto { get; set; } = "Nombre del usuario";
    public string Correo { get; set; } = "correo@institucion.edu.mx";
    public string Rol { get; set; } = "Rol del usuario";
    public string Carrera { get; set; } = "Carrera del usuario";
    public string RutaFotoPerfil { get; set; } = string.Empty;

    // Campos adicionales para el formulario de perfil del alumno
    public string Curp { get; set; } = string.Empty;
    public string FechaNacimiento { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string ControlNumber { get; set; } = string.Empty;
    public string Semestre { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string Calle { get; set; } = string.Empty;
    public string Colonia { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
