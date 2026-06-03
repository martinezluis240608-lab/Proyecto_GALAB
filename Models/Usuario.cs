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

            // 🔹 Validación temporal
            if (rol == RolUsuario.Administrador && usuario == "admin" && contrasena == "admin")
                return true;
            if (rol == RolUsuario.Estudiante)
                return true;

            // 🔹 Validación real contra BD
            using (var conexion = DatabaseService.GetConnection())
            {
                conexion.Open();
                string sql = "SELECT COUNT(*) FROM usuarios WHERE nombre=@usuario AND password=@clave AND rol=@rol";
                using (var cmd = new NpgsqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("usuario", usuario);
                    cmd.Parameters.AddWithValue("clave", contrasena);
                    cmd.Parameters.AddWithValue("rol", rol.ToString());

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}

