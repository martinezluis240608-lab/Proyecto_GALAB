using Proyecto_GALAB.Models;
using Npgsql;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Lee y guarda el perfil de alumno usando la estructura real de la BD:
///   tabla alumnos: id_alumno (PK), numero_control (bigint), nombre, primer_apellido,
///                  segundo_apellido, semestre, grupo, correo, telefono, rol, contrasena,
///                  activo, fecha_registro, usuario
/// El login usa SesionActual.NombreUsuario que contiene el numero_control como string.
/// </summary>
internal static class PerfilUsuarioStore
{
    private static PerfilUsuario _perfilMemoria = new()
    {
        NombreCompleto = "Nombre del usuario",
        Correo = "correo@institucion.edu.mx"
    };

    // ── Obtener ──────────────────────────────────────────────────────────────

    public static PerfilUsuario Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Buscar por numero_control (el valor que se usa como username en el login)
            // o por usuario (campo alternativo de login)
            const string sql = """
                SELECT id_alumno, numero_control, nombre, primer_apellido, segundo_apellido,
                       semestre, grupo, correo, telefono, rol, activo
                FROM alumnos
                WHERE numero_control::text = @control
                   OR usuario = @control
                LIMIT 1;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string nombre = reader.IsDBNull(2) ? "" : reader.GetString(2);
                string apellido1 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                string apellido2 = reader.IsDBNull(4) ? "" : reader.GetString(4);
                string nombreCompleto = string.Join(" ",
                    new[] { nombre, apellido1, apellido2 }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));

                bool activo = !reader.IsDBNull(10) && reader.GetBoolean(10);

                return new PerfilUsuario
                {
                    ControlNumber = reader.IsDBNull(1) ? control : reader.GetInt64(1).ToString(),
                    NombreCompleto = nombreCompleto,
                    Correo = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    Telefono = reader.IsDBNull(8) ? "" : reader.GetString(8),
                    Rol = reader.IsDBNull(9) ? "Estudiante" : reader.GetString(9),
                    Semestre = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Grupo = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    // Campos no presentes en la BD original → vacíos
                    Curp = "",
                    FechaNacimiento = "",
                    Genero = "",
                    Carrera = "",
                    Calle = "",
                    Colonia = "",
                    CodigoPostal = "",
                    Municipio = "",
                    Estado = activo ? "Activo" : "Inactivo",
                    RutaFotoPerfil = ""
                };
            }
        }
        catch
        {
            // Ignorar y devolver memoria
        }

        // Fallback en memoria
        return new PerfilUsuario
        {
            ControlNumber = control,
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) ||
                             _perfilMemoria.NombreCompleto == "Nombre del usuario"
                                ? control : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Rol = _perfilMemoria.Rol,
            Semestre = _perfilMemoria.Semestre,
            Grupo = _perfilMemoria.Grupo,
            Telefono = _perfilMemoria.Telefono,
            RutaFotoPerfil = _perfilMemoria.RutaFotoPerfil
        };
    }

    // ── Guardar ──────────────────────────────────────────────────────────────

    public static void Guardar(PerfilUsuario perfil)
    {
        string control = string.IsNullOrWhiteSpace(perfil.ControlNumber)
            ? SesionActual.NombreUsuario
            : perfil.ControlNumber;

        _perfilMemoria = perfil;

        if (string.IsNullOrWhiteSpace(control))
            return;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Separar nombre completo en partes para guardar en las columnas reales
            var partes = (perfil.NombreCompleto ?? "").Split(' ',
                StringSplitOptions.RemoveEmptyEntries);
            string nombre = partes.Length > 0 ? partes[0] : "";
            string apellido1 = partes.Length > 1 ? partes[1] : "";
            string apellido2 = partes.Length > 2
                ? string.Join(" ", partes.Skip(2))
                : "";

            // Actualizar los campos que sí existen en la tabla alumnos
            const string sql = """
                UPDATE alumnos
                SET nombre           = @nombre,
                    primer_apellido  = @apellido1,
                    segundo_apellido = @apellido2,
                    correo           = @correo,
                    telefono         = @telefono,
                    semestre         = @semestre,
                    grupo            = @grupo
                WHERE numero_control::text = @control
                   OR usuario = @control;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("nombre", nombre);
            cmd.Parameters.AddWithValue("apellido1", apellido1);
            cmd.Parameters.AddWithValue("apellido2", apellido2);
            cmd.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmd.Parameters.AddWithValue("telefono", perfil.Telefono ?? "");
            cmd.Parameters.AddWithValue("semestre", perfil.Semestre ?? "");
            cmd.Parameters.AddWithValue("grupo", perfil.Grupo ?? "");
            cmd.Parameters.AddWithValue("control", control);
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores de BD
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
