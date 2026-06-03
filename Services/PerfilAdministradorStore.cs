using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilAdministradorStore
{
    private static PerfilAdministrador _perfilMemoria = new()
    {
        NombreCompleto = "Administrador del sistema",
        Correo = "admin@itsmg.edu.mx"
    };

    public static PerfilAdministrador Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                SELECT nombre_completo, curp, fecha_nacimiento, genero, telefono, correo, 
                       calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil
                FROM administradores
                WHERE id_administrador = @control;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new PerfilAdministrador
                {
                    NombreCompleto = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    Curp = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    FechaNacimiento = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Genero = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Correo = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Calle = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    Colonia = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    CodigoPostal = reader.IsDBNull(8) ? "" : reader.GetString(8),
                    Municipio = reader.IsDBNull(9) ? "" : reader.GetString(9),
                    Estado = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    RutaFotoPerfil = reader.IsDBNull(11) ? "" : reader.GetString(11)
                };
            }
        }
        catch
        {
            // Ignorar error y volver al perfil temporal
        }

        // Fallback
        return new PerfilAdministrador
        {
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) || _perfilMemoria.NombreCompleto == "Administrador del sistema" ? control : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Curp = _perfilMemoria.Curp,
            FechaNacimiento = _perfilMemoria.FechaNacimiento,
            Genero = _perfilMemoria.Genero,
            Telefono = _perfilMemoria.Telefono,
            Calle = _perfilMemoria.Calle,
            Colonia = _perfilMemoria.Colonia,
            CodigoPostal = _perfilMemoria.CodigoPostal,
            Municipio = _perfilMemoria.Municipio,
            Estado = _perfilMemoria.Estado,
            RutaFotoPerfil = _perfilMemoria.RutaFotoPerfil
        };
    }

    public static void Guardar(PerfilAdministrador perfil)
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
        {
            _perfilMemoria = perfil;
            return;
        }

        _perfilMemoria = perfil;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Guardar directamente en la tabla administradores (Upsert)
            const string sql = """
                INSERT INTO administradores (id_administrador, nombre_completo, curp, fecha_nacimiento, genero, 
                                             telefono, correo, calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil)
                VALUES (@control, @nombre_c, @curp, @fecha_nac, @genero, 
                        @tel, @correo, @calle, @colonia, @cp, @mun, @est, @foto)
                ON CONFLICT (id_administrador) DO UPDATE
                SET nombre_completo = EXCLUDED.nombre_completo,
                    curp = EXCLUDED.curp,
                    fecha_nacimiento = EXCLUDED.fecha_nacimiento,
                    genero = EXCLUDED.genero,
                    telefono = EXCLUDED.telefono,
                    correo = EXCLUDED.correo,
                    calle = EXCLUDED.calle,
                    colonia = EXCLUDED.colonia,
                    codigo_postal = EXCLUDED.codigo_postal,
                    municipio = EXCLUDED.municipio,
                    estado = EXCLUDED.estado,
                    ruta_foto_perfil = EXCLUDED.ruta_foto_perfil;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            cmd.Parameters.AddWithValue("nombre_c", perfil.NombreCompleto ?? "");
            cmd.Parameters.AddWithValue("curp", perfil.Curp ?? "");
            cmd.Parameters.AddWithValue("fecha_nac", perfil.FechaNacimiento ?? "");
            cmd.Parameters.AddWithValue("genero", perfil.Genero ?? "");
            cmd.Parameters.AddWithValue("tel", perfil.Telefono ?? "");
            cmd.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmd.Parameters.AddWithValue("calle", perfil.Calle ?? "");
            cmd.Parameters.AddWithValue("colonia", perfil.Colonia ?? "");
            cmd.Parameters.AddWithValue("cp", perfil.CodigoPostal ?? "");
            cmd.Parameters.AddWithValue("mun", perfil.Municipio ?? "");
            cmd.Parameters.AddWithValue("est", perfil.Estado ?? "");
            cmd.Parameters.AddWithValue("foto", perfil.RutaFotoPerfil ?? "");
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores
        }
    }
}
