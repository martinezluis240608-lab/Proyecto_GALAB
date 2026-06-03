using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilUsuarioStore
{
    private static PerfilUsuario _perfilMemoria = new()
    {
        NombreCompleto = "Nombre del usuario",
        Correo = "correo@institucion.edu.mx"
    };

    public static PerfilUsuario Obtener()
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
                       estatus, semestre, carrera, grupo, calle, colonia, codigo_postal, 
                       municipio, estado, ruta_foto_perfil
                FROM alumnos
                WHERE num_control = @control;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new PerfilUsuario
                {
                    ControlNumber = control,
                    NombreCompleto = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    Curp = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    FechaNacimiento = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Genero = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Correo = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Rol = reader.IsDBNull(6) ? "Estudiante" : reader.GetString(6),
                    Semestre = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    Carrera = reader.IsDBNull(8) ? "" : reader.GetString(8),
                    Grupo = reader.IsDBNull(9) ? "" : reader.GetString(9),
                    Calle = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    Colonia = reader.IsDBNull(11) ? "" : reader.GetString(11),
                    CodigoPostal = reader.IsDBNull(12) ? "" : reader.GetString(12),
                    Municipio = reader.IsDBNull(13) ? "" : reader.GetString(13),
                    Estado = reader.IsDBNull(14) ? "" : reader.GetString(14),
                    RutaFotoPerfil = reader.IsDBNull(15) ? "" : reader.GetString(15)
                };
            }
        }
        catch
        {
            // Ignorar error y volver al perfil temporal
        }

        // Fallback en memoria
        return new PerfilUsuario
        {
            ControlNumber = control,
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) || _perfilMemoria.NombreCompleto == "Nombre del usuario" ? control : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Rol = _perfilMemoria.Rol,
            Carrera = _perfilMemoria.Carrera,
            RutaFotoPerfil = _perfilMemoria.RutaFotoPerfil,
            Curp = _perfilMemoria.Curp,
            FechaNacimiento = _perfilMemoria.FechaNacimiento,
            Genero = _perfilMemoria.Genero,
            Telefono = _perfilMemoria.Telefono,
            Semestre = _perfilMemoria.Semestre,
            Grupo = _perfilMemoria.Grupo,
            Calle = _perfilMemoria.Calle,
            Colonia = _perfilMemoria.Colonia,
            CodigoPostal = _perfilMemoria.CodigoPostal,
            Municipio = _perfilMemoria.Municipio,
            Estado = _perfilMemoria.Estado
        };
    }

    public static void Guardar(PerfilUsuario perfil)
    {
        string control = string.IsNullOrWhiteSpace(perfil.ControlNumber) ? SesionActual.NombreUsuario : perfil.ControlNumber;
        if (string.IsNullOrWhiteSpace(control))
        {
            _perfilMemoria = perfil;
            return;
        }

        // Respaldar localmente
        _perfilMemoria = perfil;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Guardar directamente en la tabla alumnos (Upsert) con todos sus campos
            const string sqlAlumno = """
                INSERT INTO alumnos (num_control, nombre_completo, curp, fecha_nacimiento, genero, telefono, correo, 
                                     estatus, semestre, carrera, grupo, calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil)
                VALUES (@control, @nombre_c, @curp, @fecha_nac, @genero, @tel, @correo, 
                        @estatus, @semestre, @carrera, @grupo, @calle, @colonia, @cp, @mun, @est, @foto)
                ON CONFLICT (num_control) DO UPDATE
                SET nombre_completo = EXCLUDED.nombre_completo,
                    curp = EXCLUDED.curp,
                    fecha_nacimiento = EXCLUDED.fecha_nacimiento,
                    genero = EXCLUDED.genero,
                    telefono = EXCLUDED.telefono,
                    correo = EXCLUDED.correo,
                    estatus = EXCLUDED.estatus,
                    semestre = EXCLUDED.semestre,
                    carrera = EXCLUDED.carrera,
                    grupo = EXCLUDED.grupo,
                    calle = EXCLUDED.calle,
                    colonia = EXCLUDED.colonia,
                    codigo_postal = EXCLUDED.codigo_postal,
                    municipio = EXCLUDED.municipio,
                    estado = EXCLUDED.estado,
                    ruta_foto_perfil = EXCLUDED.ruta_foto_perfil;
                """;

            using var cmdA = new NpgsqlCommand(sqlAlumno, conexion);
            cmdA.Parameters.AddWithValue("control", control);
            cmdA.Parameters.AddWithValue("nombre_c", perfil.NombreCompleto ?? "");
            cmdA.Parameters.AddWithValue("curp", perfil.Curp ?? "");
            cmdA.Parameters.AddWithValue("fecha_nac", perfil.FechaNacimiento ?? "");
            cmdA.Parameters.AddWithValue("genero", perfil.Genero ?? "");
            cmdA.Parameters.AddWithValue("tel", perfil.Telefono ?? "");
            cmdA.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmdA.Parameters.AddWithValue("estatus", perfil.Rol ?? "Estudiante");
            cmdA.Parameters.AddWithValue("semestre", perfil.Semestre ?? "");
            cmdA.Parameters.AddWithValue("carrera", perfil.Carrera ?? "");
            cmdA.Parameters.AddWithValue("grupo", perfil.Grupo ?? "");
            cmdA.Parameters.AddWithValue("calle", perfil.Calle ?? "");
            cmdA.Parameters.AddWithValue("colonia", perfil.Colonia ?? "");
            cmdA.Parameters.AddWithValue("cp", perfil.CodigoPostal ?? "");
            cmdA.Parameters.AddWithValue("mun", perfil.Municipio ?? "");
            cmdA.Parameters.AddWithValue("est", perfil.Estado ?? "");
            cmdA.Parameters.AddWithValue("foto", perfil.RutaFotoPerfil ?? "");
            cmdA.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores
        }
    }

    public static void Eliminar()
    {
        _perfilMemoria = new PerfilUsuario
        {
            NombreCompleto = "Sin datos",
            Correo = "Sin datos",
            Rol = "Sin datos",
            Carrera = "Sin datos",
            RutaFotoPerfil = string.Empty
        };
    }
}
