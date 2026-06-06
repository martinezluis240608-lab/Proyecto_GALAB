using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilAdministradorStore
{
    private static PerfilAdministrador _perfilMemoria = new()
    {
        Nombre = "Administrador",
        PrimerApellido = "",
        SegundoApellido = ""
    };

    public static PerfilAdministrador Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        return ObtenerPorId(control);
    }

    public static PerfilAdministrador ObtenerPorId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new PerfilAdministrador();

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Columnas reales de la tabla administradores del respaldo:
            // id_administrador, nombre, primer_apellido, segundo_apellido,
            // correo, telefono, usuario, contrasena, rol, activo, fecha_registro
            const string sql = """
                SELECT id_administrador, nombre, primer_apellido, segundo_apellido,
                       correo, telefono, usuario, rol, activo, fecha_registro
                FROM administradores
                WHERE id_administrador = @id
                   OR LOWER(usuario) = LOWER(@id)
                   OR LOWER(correo)  = LOWER(@id)
                LIMIT 1;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("id", id.Trim());
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string idAdmin  = reader.IsDBNull(0) ? "" : reader.GetString(0);
                string nombre   = reader.IsDBNull(1) ? "" : reader.GetString(1);
                string ape1     = reader.IsDBNull(2) ? "" : reader.GetString(2);
                string ape2     = reader.IsDBNull(3) ? "" : reader.GetString(3);
                string correo   = reader.IsDBNull(4) ? "" : reader.GetString(4);
                string telefono = reader.IsDBNull(5) ? "" : reader.GetString(5);
                string usuario  = reader.IsDBNull(6) ? "" : reader.GetString(6);
                string rol      = reader.IsDBNull(7) ? "Administrador" : reader.GetString(7);
                bool activo     = !reader.IsDBNull(8) && reader.GetBoolean(8);
                DateTime fecha  = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);

                return new PerfilAdministrador
                {
                    IdAdministrador = idAdmin,
                    Nombre          = nombre,
                    PrimerApellido  = ape1,
                    SegundoApellido = ape2,
                    NombreCompleto  = $"{nombre} {ape1} {ape2}".Trim(),
                    Correo          = correo,
                    Telefono        = telefono,
                    Usuario         = usuario,
                    Rol             = rol,
                    Activo          = activo,
                    FechaRegistro   = fecha
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al obtener perfil administrador: " + ex.Message);
        }

        return new PerfilAdministrador { IdAdministrador = id };
    }

    public static void Guardar(PerfilAdministrador perfil)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            if (string.IsNullOrWhiteSpace(perfil.IdAdministrador))
                perfil.IdAdministrador = UsuarioSistemaStore.GenerarNuevoIdAdmin(conexion);

            _perfilMemoria = perfil;

            // La contraseña NO se modifica desde el perfil (seguridad).
            // Solo se inserta vacía en registros nuevos.
            const string sql = """
                INSERT INTO administradores (
                    id_administrador, nombre, primer_apellido, segundo_apellido,
                    correo, telefono, usuario, contrasena, rol, activo, fecha_registro
                )
                VALUES (
                    @id, @nombre, @primer_apellido, @segundo_apellido,
                    @correo, @telefono, @usuario, '', @rol, @activo, NOW()
                )
                ON CONFLICT (id_administrador) DO UPDATE
                SET nombre           = EXCLUDED.nombre,
                    primer_apellido  = EXCLUDED.primer_apellido,
                    segundo_apellido = EXCLUDED.segundo_apellido,
                    correo           = EXCLUDED.correo,
                    telefono         = EXCLUDED.telefono,
                    usuario          = EXCLUDED.usuario,
                    rol              = EXCLUDED.rol,
                    activo           = EXCLUDED.activo;
                    -- contrasena NO se actualiza desde el perfil
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("id",               perfil.IdAdministrador.Trim());
            cmd.Parameters.AddWithValue("nombre",           perfil.Nombre ?? "");
            cmd.Parameters.AddWithValue("primer_apellido",  perfil.PrimerApellido ?? "");
            cmd.Parameters.AddWithValue("segundo_apellido", perfil.SegundoApellido ?? "");
            cmd.Parameters.AddWithValue("correo",           perfil.Correo ?? "");
            cmd.Parameters.AddWithValue("telefono",         perfil.Telefono ?? "");
            cmd.Parameters.AddWithValue("usuario",          string.IsNullOrWhiteSpace(perfil.Usuario) ? perfil.Correo ?? "" : perfil.Usuario);
            cmd.Parameters.AddWithValue("rol",              perfil.Rol ?? "Administrador");
            cmd.Parameters.AddWithValue("activo",           perfil.Activo);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(
                "Error al guardar administrador en la base de datos:\n\n" + ex.Message,
                "Error de Base de Datos",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
