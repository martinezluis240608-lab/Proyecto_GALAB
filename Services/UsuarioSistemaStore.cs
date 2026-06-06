using Proyecto_GALAB.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Gestión de usuarios del sistema usando las tablas reales: alumnos y administradores.
/// </summary>
internal static class UsuarioSistemaStore
{
    private static readonly List<UsuarioSistema> Usuarios = new();
    private static int _contador;

    internal static (string Nombre, string PrimerApellido, string SegundoApellido) DividirNombreCompleto(string nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            return ("", "", "");

        var partes = nombreCompleto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0) return ("", "", "");
        if (partes.Length == 1) return (partes[0], "", "");
        if (partes.Length == 2) return (partes[0], partes[1], "");

        string segundo = partes[partes.Length - 1];
        string primer  = partes[partes.Length - 2];
        string nombre  = string.Join(" ", partes.Take(partes.Length - 2));
        return (nombre, primer, segundo);
    }

    internal static string GenerarNuevoIdAdmin(NpgsqlConnection conexion)
    {
        const string sql = "SELECT id_administrador FROM administradores";
        var ids = new List<int>();
        using (var cmd = new NpgsqlCommand(sql, conexion))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string idStr = reader.GetString(0);
                if (int.TryParse(idStr, out int num))
                    ids.Add(num);
            }
        }
        int nextNum = ids.Any() ? ids.Max() + 1 : 1;
        return nextNum.ToString();
    }

    internal static string GenerarNuevoIdAlumno(NpgsqlConnection conexion)
    {
        const string sql = "SELECT id_alumno FROM alumnos";
        var ids = new List<int>();
        using (var cmd = new NpgsqlCommand(sql, conexion))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    string idStr = reader.GetString(0);
                    if (int.TryParse(idStr, out int num))
                        ids.Add(num);
                }
            }
        }
        int nextNum = ids.Any() ? ids.Max() + 1 : 1;
        return nextNum.ToString();
    }

    public static IReadOnlyList<UsuarioSistema> ObtenerTodos()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            var usuarios = new List<UsuarioSistema>();

            // 1. Administradores — columnas reales de la tabla administradores
            const string sqlAdmin = """
                SELECT id_administrador, nombre, primer_apellido, segundo_apellido,
                       correo, telefono, usuario, contrasena, rol, activo
                FROM administradores
                ORDER BY primer_apellido, nombre;
                """;
            using (var cmd = new NpgsqlCommand(sqlAdmin, conexion))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string id     = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string nom    = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string ape1   = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string ape2   = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    string correo = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    string tel    = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    string usr    = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    string pass   = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    string rol    = reader.IsDBNull(8) ? "Administrador" : reader.GetString(8);
                    bool activo   = !reader.IsDBNull(9) && reader.GetBoolean(9);

                    usuarios.Add(new UsuarioSistema
                    {
                        Id              = id,
                        Nombre          = nom,
                        PrimerApellido  = ape1,
                        SegundoApellido = ape2,
                        NombreCompleto  = $"{nom} {ape1} {ape2}".Trim(),
                        Correo          = correo,
                        Telefono        = tel,
                        Usuario         = usr,
                        Contrasena      = pass,
                        Rol             = rol,
                        Estado          = activo ? "Activo" : "Inactivo"
                    });
                }
            }

            // 2. Alumnos — columnas reales de la tabla alumnos
            const string sqlAlu = """
                SELECT id_alumno, numero_control, nombre, primer_apellido, segundo_apellido,
                       correo, telefono, semestre, grupo, numero_asiento,
                       contrasena, activo
                FROM alumnos
                ORDER BY primer_apellido, nombre;
                """;
            using (var cmdAlu = new NpgsqlCommand(sqlAlu, conexion))
            using (var readerAlu = cmdAlu.ExecuteReader())
            {
                while (readerAlu.Read())
                {
                    string id      = readerAlu.IsDBNull(0) ? "" : readerAlu.GetString(0);
                    long numCtrl   = readerAlu.IsDBNull(1) ? 0 : readerAlu.GetInt64(1);
                    string nom     = readerAlu.IsDBNull(2) ? "" : readerAlu.GetString(2);
                    string ape1    = readerAlu.IsDBNull(3) ? "" : readerAlu.GetString(3);
                    string ape2    = readerAlu.IsDBNull(4) ? "" : readerAlu.GetString(4);
                    string correo  = readerAlu.IsDBNull(5) ? "" : readerAlu.GetString(5);
                    string tel     = readerAlu.IsDBNull(6) ? "" : readerAlu.GetString(6);
                    string sem     = readerAlu.IsDBNull(7) ? "" : readerAlu.GetString(7);
                    string grp     = readerAlu.IsDBNull(8) ? "" : readerAlu.GetString(8);
                    int? asiento   = readerAlu.IsDBNull(9) ? (int?)null : readerAlu.GetInt32(9);
                    string pass    = readerAlu.IsDBNull(10) ? "" : readerAlu.GetString(10);
                    bool activo    = !readerAlu.IsDBNull(11) && readerAlu.GetBoolean(11);

                    usuarios.Add(new UsuarioSistema
                    {
                        Id              = id,
                        NumeroControl   = numCtrl,
                        Nombre          = nom,
                        PrimerApellido  = ape1,
                        SegundoApellido = ape2,
                        NombreCompleto  = $"{nom} {ape1} {ape2}".Trim(),
                        Correo          = correo,
                        Telefono        = tel,
                        Semestre        = sem,
                        Grupo           = grp,
                        NumeroAsiento   = asiento,
                        Contrasena      = pass,
                        Usuario         = id, // id_alumno como usuario
                        Rol             = "Usuario",
                        Estado          = activo ? "Activo" : "Inactivo"
                    });
                }
            }

            return usuarios;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al obtener usuarios: " + ex.Message);
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
            usuario.Id = _contador.ToString();
            Usuarios.Add(usuario);
            return;
        }

        var idx = Usuarios.FindIndex(u => u.Id == usuario.Id);
        if (idx >= 0)
            Usuarios[idx] = usuario;
    }

    public static bool Eliminar(string id, string rol)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            if (rol == "Usuario") // Es un alumno
            {
                // 1. Eliminar todas las incidencias reportadas por este alumno (Cascade delete)
                using (var cmdInc = new NpgsqlCommand("DELETE FROM incidencias WHERE id_alumno = @id", conexion))
                {
                    cmdInc.Parameters.AddWithValue("id", id);
                    cmdInc.ExecuteNonQuery();
                }

                // 2. Eliminar de alumnos
                using (var cmdA = new NpgsqlCommand("DELETE FROM alumnos WHERE id_alumno = @id", conexion))
                {
                    cmdA.Parameters.AddWithValue("id", id);
                    cmdA.ExecuteNonQuery();
                }
            }
            else // Es un administrador u otro rol de sistema
            {
                // 1. Quitar referencia de las incidencias donde este administrador haya resuelto algo
                using (var cmdUnlink = new NpgsqlCommand("UPDATE incidencias SET id_administrador = NULL WHERE id_administrador = @id", conexion))
                {
                    cmdUnlink.Parameters.AddWithValue("id", id);
                    cmdUnlink.ExecuteNonQuery();
                }

                // 2. Eliminar de administradores
                using (var cmdAdmin = new NpgsqlCommand("DELETE FROM administradores WHERE id_administrador = @id", conexion))
                {
                    cmdAdmin.Parameters.AddWithValue("id", id);
                    cmdAdmin.ExecuteNonQuery();
                }
            }

            var u = Usuarios.FirstOrDefault(x => x.Id == id && x.Rol == rol);
            if (u != null) Usuarios.Remove(u);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al eliminar usuario: " + ex.Message);
            return false;
        }
    }

    private static bool GuardarEnBaseDeDatos(UsuarioSistema usuario, bool esNuevo)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            bool isActivo = usuario.Estado == "Activo";

            if (usuario.Rol == "Usuario") // Alumno
            {
                if (esNuevo || string.IsNullOrWhiteSpace(usuario.Id) ||
                    usuario.Id.Equals("Autogenerado", StringComparison.OrdinalIgnoreCase))
                {
                    usuario.Id = GenerarNuevoIdAlumno(conexion);
                }

                const string sql = """
                    INSERT INTO alumnos (
                        id_alumno, numero_control, nombre,
                        primer_apellido, segundo_apellido,
                        semestre, grupo, correo, numero_asiento, telefono,
                        rol, contrasena, activo, fecha_registro
                    )
                    VALUES (
                        @id_alumno, @numero_control, @nombre,
                        @primer_apellido, @segundo_apellido,
                        @semestre, @grupo, @correo, @numero_asiento, @telefono,
                        'Estudiante', @contrasena, @activo, NOW()
                    )
                    ON CONFLICT (id_alumno) DO UPDATE
                    SET numero_control   = EXCLUDED.numero_control,
                        nombre           = EXCLUDED.nombre,
                        primer_apellido  = EXCLUDED.primer_apellido,
                        segundo_apellido = EXCLUDED.segundo_apellido,
                        semestre         = EXCLUDED.semestre,
                        grupo            = EXCLUDED.grupo,
                        correo           = EXCLUDED.correo,
                        numero_asiento   = EXCLUDED.numero_asiento,
                        telefono         = EXCLUDED.telefono,
                        contrasena       = EXCLUDED.contrasena,
                        activo           = EXCLUDED.activo;
                    """;
                using (var cmd = new NpgsqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("id_alumno",        usuario.Id);
                    cmd.Parameters.AddWithValue("numero_control",    usuario.NumeroControl);
                    cmd.Parameters.AddWithValue("nombre",            usuario.Nombre ?? "");
                    cmd.Parameters.AddWithValue("primer_apellido",   usuario.PrimerApellido ?? "");
                    cmd.Parameters.AddWithValue("segundo_apellido",  usuario.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("semestre",          string.IsNullOrWhiteSpace(usuario.Semestre) ? "1" : usuario.Semestre);
                    cmd.Parameters.AddWithValue("grupo",             string.IsNullOrWhiteSpace(usuario.Grupo) ? "A" : usuario.Grupo);
                    cmd.Parameters.AddWithValue("correo",            usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("numero_asiento",    (object?)usuario.NumeroAsiento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("telefono",          usuario.Telefono ?? "");
                    cmd.Parameters.AddWithValue("contrasena",        usuario.Contrasena ?? "");
                    cmd.Parameters.AddWithValue("activo",            isActivo);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            else // Administrador
            {
                if (esNuevo || string.IsNullOrWhiteSpace(usuario.Id))
                    usuario.Id = GenerarNuevoIdAdmin(conexion);

                const string sql = """
                    INSERT INTO administradores (
                        id_administrador, nombre, primer_apellido, segundo_apellido,
                        correo, telefono, usuario, contrasena, rol, activo, fecha_registro
                    )
                    VALUES (
                        @id, @nombre, @primer_apellido, @segundo_apellido,
                        @correo, @telefono, @usuario, @contrasena, 'Administrador', @activo, NOW()
                    )
                    ON CONFLICT (id_administrador) DO UPDATE
                    SET nombre           = EXCLUDED.nombre,
                        primer_apellido  = EXCLUDED.primer_apellido,
                        segundo_apellido = EXCLUDED.segundo_apellido,
                        correo           = EXCLUDED.correo,
                        telefono         = EXCLUDED.telefono,
                        usuario          = EXCLUDED.usuario,
                        contrasena       = EXCLUDED.contrasena,
                        activo           = EXCLUDED.activo;
                    """;
                using (var cmd = new NpgsqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("id",               usuario.Id);
                    cmd.Parameters.AddWithValue("nombre",           usuario.Nombre ?? "");
                    cmd.Parameters.AddWithValue("primer_apellido",  usuario.PrimerApellido ?? "");
                    cmd.Parameters.AddWithValue("segundo_apellido", usuario.SegundoApellido ?? "");
                    cmd.Parameters.AddWithValue("correo",           usuario.Correo ?? "");
                    cmd.Parameters.AddWithValue("telefono",         usuario.Telefono ?? "");
                    cmd.Parameters.AddWithValue("usuario",          string.IsNullOrWhiteSpace(usuario.Usuario) ? usuario.Correo ?? "" : usuario.Usuario);
                    cmd.Parameters.AddWithValue("contrasena",       string.IsNullOrWhiteSpace(usuario.Contrasena) ? "admin" : usuario.Contrasena);
                    cmd.Parameters.AddWithValue("activo",           isActivo);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static bool ObtenerDatosVerificacion(string nombreUsuario, out string rol, out string datoEsperado, out string tipoPregunta)
    {
        rol          = "";
        datoEsperado = "";
        tipoPregunta = "";

        if (string.IsNullOrWhiteSpace(nombreUsuario))
            return false;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Buscar en alumnos por numero_control o correo
            const string sqlAlumno = """
                SELECT correo FROM alumnos 
                WHERE CAST(numero_control AS TEXT) = @usuario OR LOWER(correo) = LOWER(@usuario)
                LIMIT 1
                """;
            using (var cmd = new NpgsqlCommand(sqlAlumno, conexion))
            {
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                var res = cmd.ExecuteScalar();
                if (res != null)
                {
                    rol          = "Estudiante";
                    datoEsperado = res.ToString() ?? "";
                    tipoPregunta = "Escriba el correo electrónico registrado de la cuenta:";
                    return true;
                }
            }

            // Buscar en administradores por usuario o correo
            const string sqlAdmin = """
                SELECT correo FROM administradores 
                WHERE LOWER(usuario) = LOWER(@usuario) OR LOWER(correo) = LOWER(@usuario)
                LIMIT 1
                """;
            using (var cmd = new NpgsqlCommand(sqlAdmin, conexion))
            {
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                var res = cmd.ExecuteScalar();
                if (res != null)
                {
                    rol          = "Administrador";
                    datoEsperado = res.ToString() ?? "";
                    tipoPregunta = "Escriba el correo electrónico del administrador:";
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error en ObtenerDatosVerificacion: " + ex.Message);
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

            if (rol == "Estudiante")
            {
                const string sql = """
                    UPDATE alumnos SET contrasena = @pass 
                    WHERE CAST(numero_control AS TEXT) = @usuario OR LOWER(correo) = LOWER(@usuario)
                    """;
                using var cmd = new NpgsqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("pass",    nuevaContrasena.Trim());
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                return cmd.ExecuteNonQuery() > 0;
            }
            else
            {
                const string sql = """
                    UPDATE administradores SET contrasena = @pass 
                    WHERE LOWER(usuario) = LOWER(@usuario) OR LOWER(correo) = LOWER(@usuario)
                    """;
                using var cmd = new NpgsqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("pass",    nuevaContrasena.Trim());
                cmd.Parameters.AddWithValue("usuario", nombreUsuario.Trim());
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error al actualizar contraseña: " + ex.Message);
        }

        return false;
    }
}
