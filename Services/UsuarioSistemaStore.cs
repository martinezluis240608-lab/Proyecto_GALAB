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

    private static (string Nombre, string PrimerApellido, string SegundoApellido) DividirNombreCompleto(string nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            return ("", "", "");

        var partes = nombreCompleto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0)
            return ("", "", "");
        if (partes.Length == 1)
            return (partes[0], "", "");
        if (partes.Length == 2)
            return (partes[0], partes[1], "");

        string segundo = partes[partes.Length - 1];
        string primer = partes[partes.Length - 2];
        string nombre = string.Join(" ", partes.Take(partes.Length - 2));
        return (nombre, primer, segundo);
    }

    private static long ExtraerNumeroControl(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return 0;
        var digits = new string(id.Where(char.IsDigit).ToArray());
        if (long.TryParse(digits, out long val))
            return val;
        return 0;
    }

    private static string GenerarNuevoIdAdmin(NpgsqlConnection conexion)
    {
        const string sql = "SELECT id_administrador FROM administradores WHERE id_administrador LIKE 'USR-%'";
        var ids = new List<int>();
        using (var cmd = new NpgsqlCommand(sql, conexion))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string idStr = reader.GetString(0);
                if (idStr.Length > 4 && int.TryParse(idStr.Substring(4), out int num))
                {
                    ids.Add(num);
                }
            }
        }
        int nextNum = ids.Any() ? ids.Max() + 1 : 1;
        return $"USR-{nextNum:D3}";
    }

    public static IReadOnlyList<UsuarioSistema> ObtenerTodos()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            var usuarios = new List<UsuarioSistema>();

            // 1. Obtener usuarios administrativos de la tabla administradores
            const string sqlSys = "SELECT id_administrador, nombre, primer_apellido, segundo_apellido, correo, rol, activo FROM administradores ORDER BY nombre";
            using (var cmdSys = new NpgsqlCommand(sqlSys, conexion))
            using (var readerSys = cmdSys.ExecuteReader())
            {
                while (readerSys.Read())
                {
                    string nom = readerSys.IsDBNull(1) ? "" : readerSys.GetString(1);
                    string ape1 = readerSys.IsDBNull(2) ? "" : readerSys.GetString(2);
                    string ape2 = readerSys.IsDBNull(3) ? "" : readerSys.GetString(3);
                    usuarios.Add(new UsuarioSistema
                    {
                        Id = readerSys.GetString(0),
                        NombreCompleto = $"{nom} {ape1} {ape2}".Trim(),
                        Correo = readerSys.IsDBNull(4) ? "" : readerSys.GetString(4),
                        Rol = readerSys.IsDBNull(5) ? "Administrador" : readerSys.GetString(5),
                        Estado = readerSys.IsDBNull(6) || readerSys.GetBoolean(6) ? "Activo" : "Inactivo"
                    });
                }
            }

            // 2. Obtener alumnos de la tabla alumnos
            const string sqlAlu = "SELECT id_alumno, nombre, primer_apellido, segundo_apellido, correo, rol, activo FROM alumnos ORDER BY nombre";
            using (var cmdAlu = new NpgsqlCommand(sqlAlu, conexion))
            using (var readerAlu = cmdAlu.ExecuteReader())
            {
                while (readerAlu.Read())
                {
                    string nom = readerAlu.IsDBNull(1) ? "" : readerAlu.GetString(1);
                    string ape1 = readerAlu.IsDBNull(2) ? "" : readerAlu.GetString(2);
                    string ape2 = readerAlu.IsDBNull(3) ? "" : readerAlu.GetString(3);
                    usuarios.Add(new UsuarioSistema
                    {
                        Id = readerAlu.GetString(0),
                        NombreCompleto = $"{nom} {ape1} {ape2}".Trim(),
                        Correo = readerAlu.IsDBNull(4) ? "" : readerAlu.GetString(4),
                        Rol = "Usuario",
                        Estado = readerAlu.IsDBNull(6) || readerAlu.GetBoolean(6) ? "Activo" : "Inactivo"
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

            // Intentar eliminar de alumnos si existe
            using (var cmdA = new NpgsqlCommand("DELETE FROM alumnos WHERE id_alumno = @id", conexion))
            {
                cmdA.Parameters.AddWithValue("id", id);
                if (cmdA.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }

            // Intentar eliminar de administradores
            using (var cmd = new NpgsqlCommand("DELETE FROM administradores WHERE id_administrador = @id", conexion))
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

            var (nom, ape1, ape2) = DividirNombreCompleto(usuario.NombreCompleto);
            bool isActivo = usuario.Estado == "Activo";

            if (usuario.Rol == "Usuario")
            {
                long numControl = ExtraerNumeroControl(usuario.Id);
                const string sqlAlumno = """
                    INSERT INTO alumnos (id_alumno, numero_control, nombre, primer_apellido, segundo_apellido, correo, rol, contrasena, activo)
                    VALUES (@id, @num_control, @nombre, @primer, @segundo, @correo, 'Estudiante', @pass, @activo)
                    ON CONFLICT (id_alumno) DO UPDATE
                    SET nombre = EXCLUDED.nombre,
                        primer_apellido = EXCLUDED.primer_apellido,
                        segundo_apellido = EXCLUDED.segundo_apellido,
                        correo = EXCLUDED.correo,
                        activo = EXCLUDED.activo;
                    """;
                using (var cmd = new NpgsqlCommand(sqlAlumno, conexion))
                {
                    cmd.Parameters.AddWithValue("id", usuario.Id);
                    cmd.Parameters.AddWithValue("num_control", numControl);
                    cmd.Parameters.AddWithValue("nombre", nom);
                    cmd.Parameters.AddWithValue("primer", ape1);
                    cmd.Parameters.AddWithValue("segundo", ape2);
                    cmd.Parameters.AddWithValue("correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("pass", usuario.Id); // Por defecto es su número de control/ID
                    cmd.Parameters.AddWithValue("activo", isActivo);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            else
            {
                if (esNuevo || string.IsNullOrWhiteSpace(usuario.Id))
                {
                    usuario.Id = GenerarNuevoIdAdmin(conexion);
                }

                const string sqlAdmin = """
                    INSERT INTO administradores (id_administrador, usuario, contrasena, nombre, primer_apellido, segundo_apellido, correo, rol, activo)
                    VALUES (@id, @usuario, @pass, @nombre, @primer, @segundo, @correo, @rol, @activo)
                    ON CONFLICT (id_administrador) DO UPDATE
                    SET usuario = EXCLUDED.usuario,
                        nombre = EXCLUDED.nombre,
                        primer_apellido = EXCLUDED.primer_apellido,
                        segundo_apellido = EXCLUDED.segundo_apellido,
                        correo = EXCLUDED.correo,
                        rol = EXCLUDED.rol,
                        activo = EXCLUDED.activo;
                    """;
                using (var cmdAdmin = new NpgsqlCommand(sqlAdmin, conexion))
                {
                    cmdAdmin.Parameters.AddWithValue("id", usuario.Id);
                    cmdAdmin.Parameters.AddWithValue("usuario", usuario.Correo); // Correo sirve como nombre de usuario de login
                    cmdAdmin.Parameters.AddWithValue("pass", "admin"); // Contraseña por defecto
                    cmdAdmin.Parameters.AddWithValue("nombre", nom);
                    cmdAdmin.Parameters.AddWithValue("primer", ape1);
                    cmdAdmin.Parameters.AddWithValue("segundo", ape2);
                    cmdAdmin.Parameters.AddWithValue("correo", usuario.Correo);
                    cmdAdmin.Parameters.AddWithValue("rol", usuario.Rol);
                    cmdAdmin.Parameters.AddWithValue("activo", isActivo);
                    cmdAdmin.ExecuteNonQuery();
                }

                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool ObtenerDatosVerificacion(string nombreUsuario, out string rol, out string datoEsperado, out string tipoPregunta)
    {
        rol = "";
        datoEsperado = "";
        tipoPregunta = "";

        if (string.IsNullOrWhiteSpace(nombreUsuario))
            return false;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // 1. Buscar en alumnos (estudiantes usan su ID de alumno)
            const string sqlAlumno = "SELECT correo FROM alumnos WHERE id_alumno = @usuario LIMIT 1";
            using (var cmd = new NpgsqlCommand(sqlAlumno, conexion))
            {
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                var res = cmd.ExecuteScalar();
                if (res != null)
                {
                    rol = "Estudiante";
                    datoEsperado = res.ToString() ?? "";
                    tipoPregunta = "Escriba el correo electrónico registrado de la cuenta:";
                    return true;
                }
            }

            // 2. Buscar en administradores (personal/administradores usan su usuario o correo)
            const string sqlSys = "SELECT nombre, primer_apellido, segundo_apellido, rol FROM administradores WHERE usuario = @usuario OR correo = @usuario OR id_administrador = @usuario LIMIT 1";
            using (var cmd = new NpgsqlCommand(sqlSys, conexion))
            {
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nom = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        string ape1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string ape2 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        rol = reader.IsDBNull(3) ? "Administrador" : reader.GetString(3);
                        datoEsperado = $"{nom} {ape1} {ape2}".Trim();
                        tipoPregunta = "Escriba el nombre completo del administrador/usuario:";
                        return true;
                    }
                }
            }
        }
        catch
        {
            // Fallback local
        }

        return false;
    }

    public static bool ActualizarContrasena(string nombreUsuario, string nuevaContrasena, string rol)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(nuevaContrasena))
            return false;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            if (rol == "Administrador" || rol == "Soporte")
            {
                const string sqlUpdate = "UPDATE administradores SET contrasena = @pass WHERE usuario = @nombre OR correo = @nombre OR id_administrador = @nombre";
                using (var cmd = new NpgsqlCommand(sqlUpdate, conexion))
                {
                    cmd.Parameters.AddWithValue("pass", nuevaContrasena.Trim());
                    cmd.Parameters.AddWithValue("nombre", nombreUsuario.Trim());
                    if (cmd.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            else
            {
                const string sqlUpdate = "UPDATE alumnos SET contrasena = @pass WHERE id_alumno = @nombre OR correo = @nombre";
                using (var cmd = new NpgsqlCommand(sqlUpdate, conexion))
                {
                    cmd.Parameters.AddWithValue("pass", nuevaContrasena.Trim());
                    cmd.Parameters.AddWithValue("nombre", nombreUsuario.Trim());
                    if (cmd.ExecuteNonQuery() > 0)
                        return true;
                }
            }
        }
        catch
        {
            // Ignorar errores
        }

        return false;
    }
}
