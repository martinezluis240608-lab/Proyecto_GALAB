using Proyecto_GALAB.Models;
using Npgsql;
using System;

namespace Proyecto_GALAB.Services;

internal static class PerfilAdministradorStore
{
    private static PerfilAdministrador _perfilMemoria = new()
    {
        NombreCompleto = "Administrador del sistema",
        Correo = "admin@itsmg.edu.mx"
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

    public static PerfilAdministrador Obtener()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return _perfilMemoria;

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                SELECT nombre, primer_apellido, segundo_apellido, telefono, correo
                FROM administradores
                WHERE id_administrador = @control;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string nom = reader.IsDBNull(0) ? "" : reader.GetString(0);
                string ape1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                string ape2 = reader.IsDBNull(2) ? "" : reader.GetString(2);

                return new PerfilAdministrador
                {
                    NombreCompleto = $"{nom} {ape1} {ape2}".Trim(),
                    Telefono = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Correo = reader.IsDBNull(4) ? "" : reader.GetString(4)
                };
            }
        }
        catch
        {
            // Ignorar error y volver al perfil temporal
        }

        // Fallback
        return new PerfilAdministrador
        {
            NombreCompleto = string.IsNullOrWhiteSpace(_perfilMemoria.NombreCompleto) || _perfilMemoria.NombreCompleto == "Administrador del sistema" ? control : _perfilMemoria.NombreCompleto,
            Correo = _perfilMemoria.Correo,
            Telefono = _perfilMemoria.Telefono
        };
    }

    public static void Guardar(PerfilAdministrador perfil)
    {
        string control = SesionActual.NombreUsuario;
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

            // Guardar directamente en la tabla administradores (Upsert)
            const string sql = """
                INSERT INTO administradores (id_administrador, nombre, primer_apellido, segundo_apellido, 
                                             telefono, correo, activo)
                VALUES (@control, @nombre, @primer, @segundo, 
                        @tel, @correo, true)
                ON CONFLICT (id_administrador) DO UPDATE
                SET nombre = EXCLUDED.nombre,
                    primer_apellido = EXCLUDED.primer_apellido,
                    segundo_apellido = EXCLUDED.segundo_apellido,
                    telefono = EXCLUDED.telefono,
                    correo = EXCLUDED.correo;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            cmd.Parameters.AddWithValue("nombre", nom);
            cmd.Parameters.AddWithValue("primer", ape1);
            cmd.Parameters.AddWithValue("segundo", ape2);
            cmd.Parameters.AddWithValue("tel", perfil.Telefono ?? "");
            cmd.Parameters.AddWithValue("correo", perfil.Correo ?? "");
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // Ignorar errores
        }
    }
}
