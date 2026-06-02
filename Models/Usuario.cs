namespace Proyecto_GALAB.Models;

public class Usuario
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena   { get; set; } = string.Empty;

    /// <summary>
    /// Autenticación temporal sin base de datos.
    /// TODO(BD): validar contra tabla Usuarios según rol.
    /// </summary>
    public bool Autenticar(string usuario, string contrasena, RolUsuario rol)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            return false;

        return rol switch
        {
            RolUsuario.Estudiante => true, // Cualquier credencial no vacía hasta conectar BD
            RolUsuario.Administrador => usuario == "admin" && contrasena == "admin",
            _ => false
        };
    }
}
