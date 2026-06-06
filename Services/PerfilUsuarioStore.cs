using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilUsuarioStore
{
    private static PerfilUsuario _perfilMemoria = new()
    {
        Nombre = "Usuario",
        PrimerApellido = "Sistema",
        Correo = "correo@institucion.edu.mx"
    };

    public static PerfilUsuario Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        return ObtenerPorControl(control);
    }

    public static PerfilUsuario ObtenerPorControl(string control)
    {
        if (string.IsNullOrWhiteSpace(control))
            return new PerfilUsuario { ControlNumber = "N/A" };

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Columnas reales del respaldo (tabla alumnos):
            // id_alumno (PK), numero_control (UNIQUE), nombre, primer_apellido,
            // segundo_apellido, semestre, grupo, correo (UNIQUE), numero_asiento,
            // telefono, rol, contrasena, activo, fecha_registro
            const string sql = """
                SELECT a.id_alumno, a.numero_control, a.nombre,
                       a.primer_apellido, a.segundo_apellido,
                       a.semestre, a.grupo, a.correo, a.numero_asiento, a.telefono,
                       a.rol, a.activo, a.fecha_registro
                FROM alumnos a
                WHERE CAST(a.numero_control AS TEXT) = @control
                   OR LOWER(a.correo) = LOWER(@control)
                   OR a.id_alumno = @control
                LIMIT 1;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control.Trim());
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string idAlumno   = reader.IsDBNull(0) ? "" : reader.GetString(0);
                long numControlVal = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);

                return new PerfilUsuario
                {
                    ControlNumber   = idAlumno,
                    NumeroControl   = numControlVal,
                    Nombre          = reader.IsDBNull(2)  ? "" : reader.GetString(2),
                    PrimerApellido  = reader.IsDBNull(3)  ? "" : reader.GetString(3),
                    SegundoApellido = reader.IsDBNull(4)  ? "" : reader.GetString(4),
                    Semestre        = reader.IsDBNull(5)  ? "" : reader.GetString(5),
                    Grupo           = reader.IsDBNull(6)  ? "" : reader.GetString(6),
                    Correo          = reader.IsDBNull(7)  ? "" : reader.GetString(7),
                    NumeroAsiento   = reader.IsDBNull(8)  ? (int?)null : reader.GetInt32(8),
                    Telefono        = reader.IsDBNull(9)  ? "" : reader.GetString(9),
                    Rol             = reader.IsDBNull(10) ? "Estudiante" : reader.GetString(10),
                    Activo          = !reader.IsDBNull(11) && reader.GetBoolean(11),
                    FechaRegistro   = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                    Usuario         = idAlumno // el id_alumno sirve como usuario
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al obtener perfil: " + ex.Message);
        }

        // Fallback en memoria
        return new PerfilUsuario
        {
            ControlNumber   = control,
            Nombre          = _perfilMemoria.Nombre,
            PrimerApellido  = _perfilMemoria.PrimerApellido,
            SegundoApellido = _perfilMemoria.SegundoApellido,
            Correo          = _perfilMemoria.Correo,
            Rol             = _perfilMemoria.Rol,
            Semestre        = _perfilMemoria.Semestre,
            Grupo           = _perfilMemoria.Grupo,
            Telefono        = _perfilMemoria.Telefono,
            Contrasena      = _perfilMemoria.Contrasena,
            Usuario         = _perfilMemoria.Usuario
        };
    }

    public static void Guardar(PerfilUsuario perfil)
    {
        string idAlumno = string.IsNullOrWhiteSpace(perfil.ControlNumber)
            ? SesionActual.NombreUsuario
            : perfil.ControlNumber;

        _perfilMemoria = perfil;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Si el id_alumno está vacío o es el usuario de fallback "admin", generar uno automáticamente
            if (string.IsNullOrWhiteSpace(idAlumno) || idAlumno == "admin")
            {
                idAlumno = UsuarioSistemaStore.GenerarNuevoIdAlumno(conexion);
                perfil.ControlNumber = idAlumno;
                SesionActual.Iniciar(idAlumno, RolUsuario.Estudiante);
            }

            // La contraseña NO se modifica desde el perfil (seguridad).
            // Solo se inserta un valor vacío en registros nuevos.
            const string sqlAlumno = """
                INSERT INTO alumnos (
                    id_alumno, numero_control, nombre,
                    primer_apellido, segundo_apellido,
                    semestre, grupo, correo, numero_asiento, telefono,
                    rol, contrasena, activo, fecha_registro
                )
                VALUES (
                    @id_alumno, @numero_control, @nombre,
                    @primer_apellido, @segundo_apellido,
                    @semestre, @grupo, @correo, @numero_asiento, @telefono,
                    @rol, '', @activo, @fecha_registro
                )
                ON CONFLICT (id_alumno) DO UPDATE
                SET numero_control   = EXCLUDED.numero_control,
                    nombre           = EXCLUDED.nombre,
                    primer_apellido  = EXCLUDED.primer_apellido,
                    segundo_apellido = EXCLUDED.segundo_apellido,
                    semestre         = EXCLUDED.semestre,
                    grupo            = EXCLUDED.grupo,
                    correo           = EXCLUDED.correo,
                    numero_asiento   = EXCLUDED.numero_asiento,
                    telefono         = EXCLUDED.telefono,
                    rol              = EXCLUDED.rol,
                    activo           = EXCLUDED.activo;
                    -- contrasena NO se actualiza desde el perfil
                """;

            using var cmdA = new NpgsqlCommand(sqlAlumno, conexion);
            cmdA.Parameters.AddWithValue("id_alumno",        idAlumno.Trim());
            cmdA.Parameters.AddWithValue("numero_control",   perfil.NumeroControl);
            cmdA.Parameters.AddWithValue("nombre",           perfil.Nombre ?? "");
            cmdA.Parameters.AddWithValue("primer_apellido",  perfil.PrimerApellido ?? "");
            cmdA.Parameters.AddWithValue("segundo_apellido", perfil.SegundoApellido ?? "");
            cmdA.Parameters.AddWithValue("semestre",         perfil.Semestre ?? "");
            cmdA.Parameters.AddWithValue("grupo",            perfil.Grupo ?? "");
            cmdA.Parameters.AddWithValue("correo",           perfil.Correo ?? "");
            cmdA.Parameters.AddWithValue("numero_asiento",   (object?)perfil.NumeroAsiento ?? DBNull.Value);
            cmdA.Parameters.AddWithValue("telefono",         perfil.Telefono ?? "");
            cmdA.Parameters.AddWithValue("rol",              perfil.Rol ?? "Estudiante");
            cmdA.Parameters.AddWithValue("activo",           perfil.Activo);
            cmdA.Parameters.AddWithValue("fecha_registro",   perfil.FechaRegistro == default ? DateTime.Now : perfil.FechaRegistro);
            cmdA.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static void Eliminar()
    {
        _perfilMemoria = new PerfilUsuario
        {
            Nombre         = "Sin",
            PrimerApellido = "datos",
            Correo         = "Sin datos",
            Rol            = "Sin datos"
        };
    }

    public static PerfilUsuario ObtenerPorNombre(string nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            return new PerfilUsuario { Nombre = "N/A" };

        var partes = nombreCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string nombre = partes.Length > 0 ? partes[0] : nombreCompleto;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                SELECT a.id_alumno, a.numero_control, a.nombre,
                       a.primer_apellido, a.segundo_apellido,
                       a.semestre, a.grupo, a.correo, a.numero_asiento, a.telefono,
                       a.rol, a.activo, a.fecha_registro
                FROM alumnos a
                WHERE LOWER(CONCAT(a.nombre, ' ', a.primer_apellido, ' ', a.segundo_apellido)) = LOWER(@nombreCompleto)
                   OR LOWER(a.nombre) = LOWER(@nombre)
                LIMIT 1;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("nombreCompleto", nombreCompleto.Trim());
            cmd.Parameters.AddWithValue("nombre", nombre);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string idAlumno    = reader.IsDBNull(0) ? "" : reader.GetString(0);
                long numControlVal = reader.IsDBNull(1) ? 0 : reader.GetInt64(1);

                return new PerfilUsuario
                {
                    ControlNumber   = idAlumno,
                    NumeroControl   = numControlVal,
                    Nombre          = reader.IsDBNull(2)  ? "" : reader.GetString(2),
                    PrimerApellido  = reader.IsDBNull(3)  ? "" : reader.GetString(3),
                    SegundoApellido = reader.IsDBNull(4)  ? "" : reader.GetString(4),
                    Semestre        = reader.IsDBNull(5)  ? "" : reader.GetString(5),
                    Grupo           = reader.IsDBNull(6)  ? "" : reader.GetString(6),
                    Correo          = reader.IsDBNull(7)  ? "" : reader.GetString(7),
                    NumeroAsiento   = reader.IsDBNull(8)  ? (int?)null : reader.GetInt32(8),
                    Telefono        = reader.IsDBNull(9)  ? "" : reader.GetString(9),
                    Rol             = reader.IsDBNull(10) ? "Estudiante" : reader.GetString(10),
                    Activo          = !reader.IsDBNull(11) && reader.GetBoolean(11),
                    FechaRegistro   = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                    Usuario         = idAlumno
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al obtener perfil por nombre: " + ex.Message);
        }

        return new PerfilUsuario
        {
            Nombre = nombreCompleto,
            Rol    = "Estudiante"
        };
    }
}
