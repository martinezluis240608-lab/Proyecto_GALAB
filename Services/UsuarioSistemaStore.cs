using Proyecto_GALAB.Models;
using Npgsql;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Usuarios del sistema adaptado a la estructura real de la BD:
///   tabla administradores: id_administrador (PK varchar), nombre, primer_apellido,
///                          segundo_apellido, correo, telefono, usuario, contrasena,
///                          rol, activo, fecha_registro
///   tabla alumnos: id_alumno (PK varchar), numero_control (bigint), nombre,
///                  primer_apellido, segundo_apellido, semestre, grupo, correo,
///                  telefono, rol, contrasena, activo, fecha_registro, usuario
/// </summary>
internal static class UsuarioSistemaStore
{
    private static readonly List<UsuarioSistema> Usuarios = new();

    // ── ObtenerTodos ─────────────────────────────────────────────────────────

    public static IReadOnlyList<UsuarioSistema> ObtenerTodos()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            var lista = new List<UsuarioSistema>();

            // 1. Administradores
            const string sqlAdmin = """
                SELECT id_administrador,
                       nombre || ' ' || primer_apellido ||
                           CASE WHEN segundo_apellido IS NOT NULL AND segundo_apellido <> ''
                                THEN ' ' || segundo_apellido ELSE '' END,
                       correo, rol, activo
                FROM administradores
                ORDER BY nombre;
                """;

            using (var cmd = new NpgsqlCommand(sqlAdmin, conexion))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    lista.Add(new UsuarioSistema
                    {
                        Id = r.GetString(0),
                        NombreCompleto = r.IsDBNull(1) ? "" : r.GetString(1),
                        Correo = r.IsDBNull(2) ? "" : r.GetString(2),
                        Rol = r.IsDBNull(3) ? "Administrador" : r.GetString(3),
                        Estado = (!r.IsDBNull(4) && r.GetBoolean(4)) ? "Activo" : "Inactivo"
                    });
                }
            }

            // 2. Alumnos
            const string sqlAlu = """
                SELECT id_alumno, numero_control,
                       nombre || ' ' || primer_apellido ||
                           CASE WHEN segundo_apellido IS NOT NULL AND segundo_apellido <> ''
                                THEN ' ' || segundo_apellido ELSE '' END,
                       correo, rol, activo
                FROM alumnos
                ORDER BY nombre;
                """;

            using (var cmd = new NpgsqlCommand(sqlAlu, conexion))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string id = r.IsDBNull(1)
                        ? r.GetString(0)
                        : r.GetInt64(1).ToString();

                    lista.Add(new UsuarioSistema
                    {
                        Id = id,
                        NombreCompleto = r.IsDBNull(2) ? "" : r.GetString(2),
                        Correo = r.IsDBNull(3) ? "" : r.GetString(3),
                        Rol = "Usuario",
                        Estado = (!r.IsDBNull(5) && r.GetBoolean(5)) ? "Activo" : "Inactivo"
                    });
                }
            }

            return lista;
        }
        catch
        {
            return Usuarios.ToList();
        }
    }

    // ── Guardar ──────────────────────────────────────────────────────────────

    public static void Guardar(UsuarioSistema usuario, bool esNuevo)
    {
        if (GuardarEnBd(usuario, esNuevo))
            return;

        // Fallback en memoria
        if (esNuevo)
        {
            Usuarios.Add(usuario);
            return;
        }
        var idx = Usuarios.FindIndex(u => u.Id == usuario.Id);
        if (idx >= 0) Usuarios[idx] = usuario;
    }

    // ── Eliminar ─────────────────────────────────────────────────────────────

    public static bool Eliminar(string id)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Intentar en alumnos (el ID es el numero_control)
            using (var cmd = new NpgsqlCommand(
                "DELETE FROM alumnos WHERE numero_control::text = @id OR id_alumno = @id", conexion))
            {
                cmd.Parameters.AddWithValue("id", id);
                if (cmd.ExecuteNonQuery() > 0) return true;
            }

            // Intentar en administradores
            using (var cmd = new NpgsqlCommand(
                "DELETE FROM administradores WHERE id_administrador = @id", conexion))
            {
                cmd.Parameters.AddWithValue("id", id);
                if (cmd.ExecuteNonQuery() > 0) return true;
            }
        }
        catch { }

        var u = Usuarios.FirstOrDefault(x => x.Id == id);
        if (u == null) return false;
        Usuarios.Remove(u);
        return true;
    }

    // ── Helpers privados ─────────────────────────────────────────────────────

    private static bool GuardarEnBd(UsuarioSistema usuario, bool esNuevo)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Separar nombre completo
            var partes = (usuario.NombreCompleto ?? "").Split(' ',
                StringSplitOptions.RemoveEmptyEntries);
            string nombre = partes.Length > 0 ? partes[0] : "";
            string apellido1 = partes.Length > 1 ? partes[1] : "";
            string apellido2 = partes.Length > 2
                ? string.Join(" ", partes.Skip(2)) : "";
            bool activo = usuario.Estado == "Activo";

            if (usuario.Rol == "Usuario")
            {
                // Alumno: verificar que el ID sea numérico (numero_control)
                if (!long.TryParse(usuario.Id, out long numControl))
                    return false;

                if (esNuevo)
                {
                    const string ins = """
                        INSERT INTO alumnos
                            (id_alumno, numero_control, nombre, primer_apellido, segundo_apellido,
                             correo, rol, contrasena, activo, fecha_registro, semestre, grupo)
                        VALUES
                            (gen_random_uuid()::text, @nc, @nombre, @ap1, @ap2,
                             @correo, 'Alumno', @nc::text, @activo, NOW(), '', '')
                        ON CONFLICT (numero_control) DO UPDATE
                        SET nombre          = EXCLUDED.nombre,
                            primer_apellido = EXCLUDED.primer_apellido,
                            correo          = EXCLUDED.correo,
                            activo          = EXCLUDED.activo;
                        """;
                    using var cmd = new NpgsqlCommand(ins, conexion);
                    cmd.Parameters.AddWithValue("nc", numControl);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("ap1", apellido1);
                    cmd.Parameters.AddWithValue("ap2", apellido2);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("activo", activo);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    const string upd = """
                        UPDATE alumnos
                        SET nombre          = @nombre,
                            primer_apellido = @ap1,
                            segundo_apellido= @ap2,
                            correo          = @correo,
                            activo          = @activo
                        WHERE numero_control::text = @id OR id_alumno = @id;
                        """;
                    using var cmd = new NpgsqlCommand(upd, conexion);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("ap1", apellido1);
                    cmd.Parameters.AddWithValue("ap2", apellido2);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("activo", activo);
                    cmd.Parameters.AddWithValue("id", usuario.Id);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            else
            {
                // Administrador
                if (esNuevo)
                {
                    const string ins = """
                        INSERT INTO administradores
                            (id_administrador, nombre, primer_apellido, segundo_apellido,
                             correo, usuario, contrasena, rol, activo, fecha_registro)
                        VALUES
                            (gen_random_uuid()::text, @nombre, @ap1, @ap2,
                             @correo, @usuario, 'admin', @rol, @activo, NOW());
                        """;
                    using var cmd = new NpgsqlCommand(ins, conexion);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("ap1", apellido1);
                    cmd.Parameters.AddWithValue("ap2", apellido2);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("usuario", usuario.Correo ?? usuario.Id);
                    cmd.Parameters.AddWithValue("rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("activo", activo);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    const string upd = """
                        UPDATE administradores
                        SET nombre           = @nombre,
                            primer_apellido  = @ap1,
                            segundo_apellido = @ap2,
                            correo           = @correo,
                            rol              = @rol,
                            activo           = @activo
                        WHERE id_administrador = @id;
                        """;
                    using var cmd = new NpgsqlCommand(upd, conexion);
                    cmd.Parameters.AddWithValue("nombre", nombre);
                    cmd.Parameters.AddWithValue("ap1", apellido1);
                    cmd.Parameters.AddWithValue("ap2", apellido2);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("activo", activo);
                    cmd.Parameters.AddWithValue("id", usuario.Id);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
