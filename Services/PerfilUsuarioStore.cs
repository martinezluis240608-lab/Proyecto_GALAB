using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilUsuarioStore
{
    private static PerfilUsuario _perfilMemoria = new()
    {
        NombreCompleto = "Nombre del usuario",
        Correo = "correo@institucion.edu.mx"
    };

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

    public static PerfilUsuario Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        return ObtenerPorControl(control);
    }

    public static PerfilUsuario ObtenerPorControl(string control)
    {
        if (string.IsNullOrWhiteSpace(control))
            return new PerfilUsuario { ControlNumber = "N/A" };

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                SELECT nombre, primer_apellido, segundo_apellido, telefono, correo, 
                       rol, semestre, grupo
                FROM alumnos
                WHERE id_alumno = @control;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nom = reader.IsDBNull(0) ? "" : reader.GetString(0);
                string ape1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                string ape2 = reader.IsDBNull(2) ? "" : reader.GetString(2);

                return new PerfilUsuario
                {
                    ControlNumber = control,
                    NombreCompleto = $"{nom} {ape1} {ape2}".Trim(),
                    Telefono = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Correo = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Rol = reader.IsDBNull(5) ? "Estudiante" : reader.GetString(5),
                    Semestre = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    Grupo = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    Carrera = "Sistemas"
                };
            }
        }
        catch
        {
            // Ignorar error y volver al perfil temporal
        }

        // Fallback en memoria
        return new PerfilUsuario
        {
            ControlNumber = control,
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) || _perfilMemoria.NombreCompleto == "Nombre del usuario" ? control : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Rol = _perfilMemoria.Rol,
            Carrera = _perfilMemoria.Carrera,
            Telefono = _perfilMemoria.Telefono,
            Semestre = _perfilMemoria.Semestre,
            Grupo = _perfilMemoria.Grupo
        };
    }

    public static void Guardar(PerfilUsuario perfil)
    {
        string control = string.IsNullOrWhiteSpace(perfil.ControlNumber) ? SesionActual.NombreUsuario : perfil.ControlNumber;
        if (string.IsNullOrWhiteSpace(control))
        {
            _perfilMemoria = perfil;
            return;
        }

        _perfilMemoria = perfil;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            var (nom, ape1, ape2) = DividirNombreCompleto(perfil.NombreCompleto);

            const string sqlAlumno = """
                INSERT INTO alumnos (id_alumno, numero_control, nombre, primer_apellido, segundo_apellido, telefono, correo, 
                                     rol, semestre, grupo, activo)
                VALUES (@control, @num_control, @nombre, @primer, @segundo, @tel, @correo, 
                        @rol, @semestre, @grupo, true)
                ON CONFLICT (id_alumno) DO UPDATE
                SET nombre = EXCLUDED.nombre,
                    primer_apellido = EXCLUDED.primer_apellido,
                    segundo_apellido = EXCLUDED.segundo_apellido,
                    telefono = EXCLUDED.telefono,
                    correo = EXCLUDED.correo,
                    rol = EXCLUDED.rol,
                    semestre = EXCLUDED.semestre,
                    grupo = EXCLUDED.grupo;
                """;

            // Extraer o generar numero_control
            long numControl = 0;
            var digits = new string(control.Where(char.IsDigit).ToArray());
            long.TryParse(digits, out numControl);

            using var cmdA = new NpgsqlCommand(sqlAlumno, conexion);
            cmdA.Parameters.AddWithValue("control", control);
            cmdA.Parameters.AddWithValue("num_control", numControl);
            cmdA.Parameters.AddWithValue("nombre", nom);
            cmdA.Parameters.AddWithValue("primer", ape1);
            cmdA.Parameters.AddWithValue("segundo", ape2);
            cmdA.Parameters.AddWithValue("tel", perfil.Telefono ?? "");
            cmdA.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmdA.Parameters.AddWithValue("rol", perfil.Rol ?? "Estudiante");
            cmdA.Parameters.AddWithValue("semestre", perfil.Semestre ?? "");
            cmdA.Parameters.AddWithValue("grupo", perfil.Grupo ?? "");
            cmdA.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores
        }
    }

    public static void Eliminar()
    {
        _perfilMemoria = new PerfilUsuario
        {
            NombreCompleto = "Sin datos",
            Correo = "Sin datos",
            Rol = "Sin datos",
            Carrera = "Sin datos",
            RutaFotoPerfil = string.Empty
        };
    }

    public static PerfilUsuario ObtenerPorNombre(string nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            return new PerfilUsuario { NombreCompleto = "N/A" };

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                SELECT id_alumno, nombre, primer_apellido, segundo_apellido, telefono, correo, 
                       rol, semestre, grupo
                FROM alumnos
                WHERE TRIM(CONCAT(nombre, ' ', primer_apellido, ' ', COALESCE(segundo_apellido, ''))) = @nombre;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("nombre", nombreCompleto.Trim());
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string id = reader.GetString(0);
                string nom = reader.IsDBNull(1) ? "" : reader.GetString(1);
                string ape1 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                string ape2 = reader.IsDBNull(3) ? "" : reader.GetString(3);

                return new PerfilUsuario
                {
                    ControlNumber = id,
                    NombreCompleto = $"{nom} {ape1} {ape2}".Trim(),
                    Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Correo = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Rol = reader.IsDBNull(6) ? "Estudiante" : reader.GetString(6),
                    Semestre = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    Grupo = reader.IsDBNull(8) ? "" : reader.GetString(8),
                    Carrera = "Sistemas"
                };
            }
        }
        catch
        {
            // Ignorar errores
        }

        return new PerfilUsuario
        {
            NombreCompleto = nombreCompleto,
            Rol = "Estudiante"
        };
    }
}
