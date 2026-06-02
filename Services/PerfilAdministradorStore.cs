using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

internal static class PerfilAdministradorStore
{
    private static PerfilAdministrador _perfil = new()
    {
        NombreCompleto = "Administrador del sistema",
        Correo = "admin@itsmg.edu.mx"
    };

    public static PerfilAdministrador Obtener() => new()
    {
        NombreCompleto = _perfil.NombreCompleto,
        Curp = _perfil.Curp,
        FechaNacimiento = _perfil.FechaNacimiento,
        Genero = _perfil.Genero,
        EstadoCivil = _perfil.EstadoCivil,
        Telefono = _perfil.Telefono,
        Correo = _perfil.Correo,
        NumeroServicioMedico = _perfil.NumeroServicioMedico,
        Calle = _perfil.Calle,
        Colonia = _perfil.Colonia,
        CodigoPostal = _perfil.CodigoPostal,
        Municipio = _perfil.Municipio,
        Estado = _perfil.Estado,
        RutaFotoPerfil = _perfil.RutaFotoPerfil
    };

    public static void Guardar(PerfilAdministrador perfil)
    {
        _perfil = new PerfilAdministrador
        {
            NombreCompleto = perfil.NombreCompleto,
            Curp = perfil.Curp,
            FechaNacimiento = perfil.FechaNacimiento,
            Genero = perfil.Genero,
            EstadoCivil = perfil.EstadoCivil,
            Telefono = perfil.Telefono,
            Correo = perfil.Correo,
            NumeroServicioMedico = perfil.NumeroServicioMedico,
            Calle = perfil.Calle,
            Colonia = perfil.Colonia,
            CodigoPostal = perfil.CodigoPostal,
            Municipio = perfil.Municipio,
            Estado = perfil.Estado,
            RutaFotoPerfil = perfil.RutaFotoPerfil
        };
    }
}
