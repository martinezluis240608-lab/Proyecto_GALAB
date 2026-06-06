using Npgsql;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Models
{
    public class Usuario
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Autentica al usuario contra la base de datos.
        /// Para Estudiante: busca en tabla 'alumnos' por numero_control o correo + contrasena.
        /// Para Administrador: busca en tabla 'administradores' por usuario o correo + contrasena.
        /// Devuelve el id_alumno / id_administrador si es correcto, o null si falla.
        /// </summary>
        public string? Autenticar(string usuario, string contrasena, RolUsuario rol)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            try
            {
                using var conexion = DatabaseService.GetConnection();
                conexion.Open();

                string sql;

                if (rol == RolUsuario.Administrador)
                {
                    // Buscar en tabla administradores por usuario o correo + contrasena
                    sql = """
                        SELECT id_administrador
                        FROM administradores
                        WHERE (LOWER(usuario) = LOWER(@usuario) OR LOWER(correo) = LOWER(@usuario))
                          AND contrasena = @clave
                          AND activo = true
                        LIMIT 1;
                        """;
                }
                else
                {
                    // Buscar en tabla alumnos por numero_control o correo + contrasena
                    sql = """
                        SELECT id_alumno
                        FROM alumnos
                        WHERE (CAST(numero_control AS TEXT) = @usuario OR LOWER(correo) = LOWER(@usuario))
                          AND contrasena = @clave
                          AND activo = true
                        LIMIT 1;
                        """;
                }

                using var cmd = new NpgsqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("usuario", usuario.Trim());
                cmd.Parameters.AddWithValue("clave",   contrasena.Trim());

                var res = cmd.ExecuteScalar();
                if (res != null && res != DBNull.Value)
                    return res.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Login] Error de autenticación: " + ex.Message);
            }

            // ── Fallbacks ──────────────────────────────────────────────────
            // Administrador: acceso directo admin / admin
            if (rol == RolUsuario.Administrador && usuario == "admin" && contrasena == "admin")
                return "admin";

            // Estudiante: acceso directo admin / admin
            if (rol == RolUsuario.Estudiante && usuario == "admin" && contrasena == "admin")
                return "admin";

            return null;
        }
    }
}
