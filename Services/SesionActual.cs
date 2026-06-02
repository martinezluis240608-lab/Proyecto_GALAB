using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Datos de la sesión actual (memoria). Sustituir por claims/token cuando haya backend.
/// </summary>
public static class SesionActual
{
    public static string NombreUsuario { get; private set; } = string.Empty;
    public static RolUsuario Rol { get; private set; } = RolUsuario.Estudiante;
    public static bool EsAdministrador => Rol == RolUsuario.Administrador;

    public static void Iniciar(string nombreUsuario, RolUsuario rol)
    {
        NombreUsuario = nombreUsuario;
        Rol = rol;
    }

    public static void Cerrar()
    {
        NombreUsuario = string.Empty;
        Rol = RolUsuario.Estudiante;
    }
}
