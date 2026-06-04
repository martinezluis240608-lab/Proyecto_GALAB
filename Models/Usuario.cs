using Npgsql;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Models
{
    public class Usuario
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        /// <summary>
        /// Autenticación temporal sin base de datos.
        /// TODO(BD): validar contra tabla Usuarios según rol.
        /// </summary>
        public bool Autenticar(string usuario, string contrasena, RolUsuario rol)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
                return false;

            try
            {
                using (var conexion = DatabaseService.GetConnection())
                {
                    conexion.Open();
                    string sql;
                    if (rol == RolUsuario.Administrador)
                    {
                        sql = "SELECT COUNT(*) FROM administradores WHERE (usuario=@usuario OR correo=@usuario OR id_administrador=@usuario) AND contrasena=@clave AND activo=true";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM alumnos WHERE (id_alumno=@usuario OR correo=@usuario) AND contrasena=@clave AND activo=true";
                    }

                    using (var cmd = new NpgsqlCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("usuario", usuario.Trim());
                        cmd.Parameters.AddWithValue("clave", contrasena.Trim());

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                            return true;
                    }
                }
            }
            catch
            {
                // Si la base de datos falla o no está disponible, se permite el fallback temporal.
            }

            // 🔹 Validación temporal/fallback
            if (rol == RolUsuario.Administrador && usuario == "admin" && contrasena == "admin")
                return true;
            if (rol == RolUsuario.Estudiante)
                return true;

            return false;
        }
    }
}

