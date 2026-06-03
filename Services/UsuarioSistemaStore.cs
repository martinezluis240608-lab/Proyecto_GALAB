using Proyecto_GALAB.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Usuarios del sistema (personal de soporte, admins y alumnos). Usa PostgreSQL.
/// </summary>
internal static class UsuarioSistemaStore
{
    private static readonly List<UsuarioSistema> Usuarios = new();
    private static int _contador;

    public static IReadOnlyList<UsuarioSistema> ObtenerTodos()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            var usuarios = new List<UsuarioSistema>();

            // 1. Obtener usuarios administrativos de la tabla usuarios_sistema
            const string sqlSys = "SELECT id_usuario, nombre_completo, correo, rol, estado FROM usuarios_sistema ORDER BY nombre_completo";
            using (var cmdSys = new NpgsqlCommand(sqlSys, conexion))
            using (var readerSys = cmdSys.ExecuteReader())
            {
                while (readerSys.Read())
                {
                    usuarios.Add(new UsuarioSistema
                    {
                        Id = readerSys.GetString(0),
                        NombreCompleto = readerSys.GetString(1),
                        Correo = readerSys.GetString(2),
                        Rol = readerSys.GetString(3),
                        Estado = readerSys.GetString(4)
                    });
                }
            }

            // 2. Obtener alumnos de la tabla alumnos
            const string sqlAlu = "SELECT num_control, nombre_completo, correo, estatus FROM alumnos ORDER BY nombre_completo";
            using (var cmdAlu = new NpgsqlCommand(sqlAlu, conexion))
            using (var readerAlu = cmdAlu.ExecuteReader())
            {
                while (readerAlu.Read())
                {
                    usuarios.Add(new UsuarioSistema
                    {
                        Id = readerAlu.GetString(0),
                        NombreCompleto = readerAlu.IsDBNull(1) ? "" : readerAlu.GetString(1),
                        Correo = readerAlu.IsDBNull(2) ? "" : readerAlu.GetString(2),
                        Rol = "Usuario",
                        Estado = readerAlu.IsDBNull(3) || string.IsNullOrWhiteSpace(readerAlu.GetString(3)) ? "Activo" : readerAlu.GetString(3)
                    });
                }
            }

            return usuarios;
        }
        catch
        {
            return Usuarios.ToList();
        }
    }

    public static void Guardar(UsuarioSistema usuario, bool esNuevo)
    {
        if (GuardarEnBaseDeDatos(usuario, esNuevo))
            return;

        if (esNuevo)
        {
            _contador++;
            usuario.Id = usuario.Rol == "Usuario" ? $"ALU-{_contador:D3}" : $"USR-{_contador:D3}";
            Usuarios.Add(usuario);
            return;
        }

        var idx = Usuarios.FindIndex(u => u.Id == usuario.Id);
        if (idx >= 0)
            Usuarios[idx] = usuario;
    }

    public static bool Eliminar(string id)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Eliminar de alumnos si existe
            using (var cmdA = new NpgsqlCommand("DELETE FROM alumnos WHERE num_control = @id", conexion))
            {
                cmdA.Parameters.AddWithValue("id", id);
                if (cmdA.ExecuteNonQuery() > 0)
                {
                    // También eliminar credenciales
                    using (var cmdU = new NpgsqlCommand("DELETE FROM usuarios WHERE nombre = @id", conexion))
                    {
                        cmdU.Parameters.AddWithValue("id", id);
                        cmdU.ExecuteNonQuery();
                    }
                    return true;
                }
            }

            // Eliminar de usuarios_sistema
            using (var cmd = new NpgsqlCommand("DELETE FROM usuarios_sistema WHERE id_usuario = @id", conexion))
            {
                cmd.Parameters.AddWithValue("id", id);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
        }
        catch
        {
            // Respaldo local
        }

        var u = Usuarios.FirstOrDefault(x => x.Id == id);
        if (u == null) return false;
        Usuarios.Remove(u);
        return true;
    }

    private static bool GuardarEnBaseDeDatos(UsuarioSistema usuario, bool esNuevo)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            if (usuario.Rol == "Usuario")
            {
                // Guardar en alumnos (Upsert)
                const string sqlAlumno = """
                    INSERT INTO alumnos (num_control, nombre_completo, correo, estatus)
                    VALUES (@control, @nombre, @correo, @estatus)
                    ON CONFLICT (num_control) DO UPDATE
                    SET nombre_completo = EXCLUDED.nombre_completo,
                        correo = EXCLUDED.correo,
                        estatus = EXCLUDED.estatus;
                    """;
                using (var cmd = new NpgsqlCommand(sqlAlumno, conexion))
                {
                    cmd.Parameters.AddWithValue("control", usuario.Id);
                    cmd.Parameters.AddWithValue("nombre", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("estatus", usuario.Estado);
                    cmd.ExecuteNonQuery();
                }

                // Generar/actualizar credenciales de inicio de sesión en usuarios
                const string sqlCred = """
                    INSERT INTO usuarios (nombre, password, rol, activo)
                    VALUES (@nombre, @pass, @rol, @activo)
                    ON CONFLICT (nombre) DO UPDATE
                    SET rol = EXCLUDED.rol,
                        activo = EXCLUDED.activo;
                    """;
                using (var cmdCred = new NpgsqlCommand(sqlCred, conexion))
                {
                    cmdCred.Parameters.AddWithValue("nombre", usuario.Id);
                    cmdCred.Parameters.AddWithValue("pass", usuario.Id); // La contraseña por defecto es su número de control
                    cmdCred.Parameters.AddWithValue("rol", "Estudiante");
                    cmdCred.Parameters.AddWithValue("activo", usuario.Estado == "Activo");
                    cmdCred.ExecuteNonQuery();
                }

                return true;
            }
            else
            {
                // Guardar en usuarios_sistema
                if (esNuevo || string.IsNullOrWhiteSpace(usuario.Id))
                {
                    const string insert = """
                        INSERT INTO usuarios_sistema (nombre_completo, correo, rol, estado)
                        VALUES (@nombre, @correo, @rol, @estado)
                        RETURNING id_usuario;
                        """;
                    using (var cmd = new NpgsqlCommand(insert, conexion))
                    {
                        cmd.Parameters.AddWithValue("nombre", usuario.NombreCompleto);
                        cmd.Parameters.AddWithValue("correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("rol", usuario.Rol);
                        cmd.Parameters.AddWithValue("estado", usuario.Estado);
                        usuario.Id = Convert.ToString(cmd.ExecuteScalar()) ?? usuario.Id;
                    }
                }
                else
                {
                    const string update = """
                        UPDATE usuarios_sistema
                        SET nombre_completo = @nombre,
                            correo = @correo,
                            rol = @rol,
                            estado = @estado
                        WHERE id_usuario = @id;
                        """;
                    using (var updateCmd = new NpgsqlCommand(update, conexion))
                    {
                        updateCmd.Parameters.AddWithValue("id", usuario.Id);
                        updateCmd.Parameters.AddWithValue("nombre", usuario.NombreCompleto);
                        updateCmd.Parameters.AddWithValue("correo", usuario.Correo);
                        updateCmd.Parameters.AddWithValue("rol", usuario.Rol);
                        updateCmd.Parameters.AddWithValue("estado", usuario.Estado);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                // Generar/actualizar credenciales de inicio de sesión en usuarios
                const string sqlCredSys = """
                    INSERT INTO usuarios (nombre, password, rol, activo)
                    VALUES (@nombre, @pass, @rol, @activo)
                    ON CONFLICT (nombre) DO UPDATE
                    SET rol = EXCLUDED.rol,
                        activo = EXCLUDED.activo;
                    """;
                using (var cmdCredSys = new NpgsqlCommand(sqlCredSys, conexion))
                {
                    cmdCredSys.Parameters.AddWithValue("nombre", usuario.Correo); // Nombre de usuario es el correo
                    cmdCredSys.Parameters.AddWithValue("pass", "admin"); // Contraseña por defecto
                    cmdCredSys.Parameters.AddWithValue("rol", usuario.Rol == "Administrador" ? "Administrador" : "Estudiante");
                    cmdCredSys.Parameters.AddWithValue("activo", usuario.Estado == "Activo");
                    cmdCredSys.ExecuteNonQuery();
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
