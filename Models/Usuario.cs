using Npgsql;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Models
{
    public class Usuario
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;

        public bool Autenticar(string usuario, string contrasena, RolUsuario rol)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
                return false;

            string tabla = rol == RolUsuario.Administrador ? "administradores" : "alumnos";
            string columnaId = rol == RolUsuario.Administrador ? "usuario" : "usuario";

            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            string sql = $"SELECT {columnaId}, contrasena FROM {tabla} WHERE usuario = @usuario";
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("usuario", usuario.Trim());

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return false;

            string idUsuario = reader.GetString(0);
            string hash = reader.GetString(1);

            try
            {
                bool valido = BCrypt.Net.BCrypt.Verify(contrasena, hash);
                if (valido)
                    NombreUsuario = idUsuario;

                return valido;
            }
            catch
            {
                return false;
            }
        }
    }
}
