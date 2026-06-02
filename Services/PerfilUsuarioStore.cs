using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

internal static class PerfilUsuarioStore
{
    private static PerfilUsuario _perfil = new();

    public static PerfilUsuario Obtener()
    {
        return new PerfilUsuario
        {
            NombreCompleto = _perfil.NombreCompleto,
            Correo = _perfil.Correo,
            Rol = _perfil.Rol,
            Carrera = _perfil.Carrera,
            RutaFotoPerfil = _perfil.RutaFotoPerfil
        };
    }

    public static void Guardar(PerfilUsuario perfil)
    {
        _perfil = new PerfilUsuario
        {
            NombreCompleto = perfil.NombreCompleto,
            Correo = perfil.Correo,
            Rol = perfil.Rol,
            Carrera = perfil.Carrera,
            RutaFotoPerfil = perfil.RutaFotoPerfil
        };
    }

    public static void Eliminar()
    {
        _perfil = new PerfilUsuario
        {
            NombreCompleto = "Sin datos",
            Correo = "Sin datos",
            Rol = "Sin datos",
            Carrera = "Sin datos",
            RutaFotoPerfil = string.Empty
        };
    }
}
