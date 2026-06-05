using Proyecto_GALAB.Models;
using Npgsql;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Lee y guarda el perfil del administrador usando la estructura real de la BD:
///   tabla administradores: id_administrador (PK varchar), nombre, primer_apellido,
///                          segundo_apellido, correo, telefono, usuario, contrasena,
///                          rol, activo, fecha_registro
/// SesionActual.NombreUsuario contiene el campo "usuario" con el que inició sesión.
/// </summary>
internal static class PerfilAdministradorStore
{
    private static PerfilAdministrador _perfilMemoria = new()
    {
        NombreCompleto = "Administrador del sistema",
        Correo = "admin@itsmg.edu.mx"
    };

    // ── Obtener ──────────────────────────────────────────────────────────────

    public static PerfilAdministrador Obtener()
    {
        string usuarioSesion = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(usuarioSesion))
            return _perfilMemoria;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // La tabla usa "usuario" como campo de login, id_administrador como PK propia
            const string sql = """
                SELECT id_administrador, nombre, primer_apellido, segundo_apellido,
                       correo, telefono, usuario, activo
                FROM administradores
                WHERE usuario = @usuario
                LIMIT 1;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("usuario", usuarioSesion);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string nombre = reader.IsDBNull(1) ? "" : reader.GetString(1);
                string apellido1 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                string apellido2 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                string nombreCompleto = string.Join(" ",
                    new[] { nombre, apellido1, apellido2 }
                    .Where(s => !string.IsNullOrWhiteSpace(s)));

                return new PerfilAdministrador
                {
                    NombreCompleto = nombreCompleto,
                    Correo = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Telefono = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    // Campos que no existen en la tabla original → vacíos
                    Curp = "",
                    FechaNacimiento = "",
                    Genero = "",
                    Calle = "",
                    Colonia = "",
                    CodigoPostal = "",
                    Municipio = "",
                    Estado = "",
                    RutaFotoPerfil = ""
                };
            }
        }
        catch
        {
            // Ignorar y usar memoria
        }

        // Fallback
        return new PerfilAdministrador
        {
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) ||
                             _perfilMemoria.NombreCompleto == "Administrador del sistema"
                                ? usuarioSesion : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Telefono = _perfilMemoria.Telefono,
            RutaFotoPerfil = _perfilMemoria.RutaFotoPerfil
        };
    }

    // ── Guardar ──────────────────────────────────────────────────────────────

    public static void Guardar(PerfilAdministrador perfil)
    {
        string usuarioSesion = SesionActual.NombreUsuario;
        _perfilMemoria = perfil;

        if (string.IsNullOrWhiteSpace(usuarioSesion))
            return;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Separar nombre en partes para las columnas reales
            var partes = (perfil.NombreCompleto ?? "").Split(' ',
                StringSplitOptions.RemoveEmptyEntries);
            string nombre = partes.Length > 0 ? partes[0] : "";
            string apellido1 = partes.Length > 1 ? partes[1] : "";
            string apellido2 = partes.Length > 2
                ? string.Join(" ", partes.Skip(2))
                : "";

            const string sql = """
                UPDATE administradores
                SET nombre           = @nombre,
                    primer_apellido  = @apellido1,
                    segundo_apellido = @apellido2,
                    correo           = @correo,
                    telefono         = @telefono
                WHERE usuario = @usuario;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("nombre", nombre);
            cmd.Parameters.AddWithValue("apellido1", apellido1);
            cmd.Parameters.AddWithValue("apellido2", apellido2);
            cmd.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmd.Parameters.AddWithValue("telefono", perfil.Telefono ?? "");
            cmd.Parameters.AddWithValue("usuario", usuarioSesion);
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores de BD
        }
    }
}
