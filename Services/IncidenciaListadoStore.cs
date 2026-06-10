using Proyecto_GALAB.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Repositorio de incidencias contra PostgreSQL (tabla public.incidencias).
/// </summary>
internal static class IncidenciaListadoStore
{
    private static readonly List<IncidenciaListadoItem> Items = new();
    private static long _contadorMemoria = 0;
    private enum ResultadoGuardado
    {
        Guardado,
        ErrorBaseDatos,
        ErrorValidacion
    }

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerTodas()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            bool filtrarPorAlumno = !SesionActual.EsAdministrador;
            string? idAlumnoSesion = filtrarPorAlumno ? ObtenerIdAlumnoActual(conexion) : null;
            if (filtrarPorAlumno && string.IsNullOrWhiteSpace(idAlumnoSesion))
                return ObtenerItemsMemoriaSegunSesion();

            const string sql = """
                SELECT 
                    i.id_incidencia,
                    'INC-' || EXTRACT(YEAR FROM i.fecha_reporte) || '-' || LPAD(i.id_incidencia::text, 4, '0') AS folio,
                    COALESCE(i.titulo, ''),
                    COALESCE(a.nombre || ' ' || a.primer_apellido, i.id_alumno) AS quien_reporta,
                    COALESCE(e.tipo_equipamiento, ''),
                    COALESCE(e.nombre, ''),
                    COALESCE(i.estado, 'Activa'),
                    COALESCE(i.descripcion, ''),
                    (i.fecha_reporte + i.hora_reporte) AS fecha_hora,
                    COALESCE(i.solucion, ''),
                    COALESCE(a.numero_control::text, 'N/A'),
                    COALESCE(a.semestre, 'N/A'),
                    COALESCE(a.grupo, 'N/A'),
                    COALESCE(a.correo, 'N/A'),
                    COALESCE(a.telefono, 'N/A'),
                    i.id_alumno,
                    i.id_serie::text
                FROM incidencias i
                LEFT JOIN alumnos a ON i.id_alumno = a.id_alumno
                LEFT JOIN equipamientos e ON i.id_serie = e.id_serie
                WHERE (@filtrar_alumno = FALSE OR i.id_alumno = @id_alumno)
                ORDER BY i.fecha_reporte DESC, i.hora_reporte DESC, i.id_incidencia DESC;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("filtrar_alumno", filtrarPorAlumno);
            cmd.Parameters.AddWithValue("id_alumno", (object?)idAlumnoSesion ?? DBNull.Value);
            using var reader = cmd.ExecuteReader();
            var lista = new List<IncidenciaListadoItem>();

            while (reader.Read())
            {
                lista.Add(new IncidenciaListadoItem
                {
                    IdReal              = reader.GetInt64(0).ToString(),
                    Folio               = reader.IsDBNull(1) ? "SIN-FOLIO" : reader.GetString(1),
                    Titulo              = reader.GetString(2),
                    QuienReporta        = reader.GetString(3),
                    TipoIncidencia      = reader.GetString(4),
                    Equipo              = reader.GetString(5),
                    Estado              = reader.GetString(6),
                    Descripcion         = reader.GetString(7),
                    Fecha               = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8),
                    DescripcionSolucion = reader.GetString(9),
                    NumeroControl       = reader.GetString(10),
                    Semestre            = reader.GetString(11),
                    Grupo               = reader.GetString(12),
                    Correo              = reader.GetString(13),
                    Telefono            = reader.GetString(14),
                    IdAlumno            = reader.IsDBNull(15) ? "" : reader.GetString(15),
                    NumeroSerie         = reader.IsDBNull(16) ? "" : reader.GetString(16)
                });
            }

            return lista;
        }
        catch
        {
            return ObtenerItemsMemoriaSegunSesion();
        }
    }

    public static (bool exito, string mensaje) RegistrarDesdeIncidencia(Incidencia incidencia)
    {
        var item = new IncidenciaListadoItem
        {
            Folio          = string.Empty,
            Titulo         = incidencia.Titulo,
            QuienReporta   = incidencia.QuienReporta,
            TipoIncidencia = incidencia.TipoIncidencia,
            Estado         = "Activa",
            Fecha          = incidencia.FechaHora == default ? DateTime.Now : incidencia.FechaHora,
            Descripcion    = incidencia.Descripcion,
            Equipo         = incidencia.NombreEquipo,
            NumeroSerie    = NormalizarNumeroSerie(incidencia.NumeroSerie)
        };

        var resultado = GuardarEnBaseDeDatos(item, incidencia, out string error);
        if (resultado == ResultadoGuardado.ErrorValidacion)
        {
            return (false, error);
        }

        if (resultado == ResultadoGuardado.ErrorBaseDatos)
        {
            _contadorMemoria++;
            item.Folio = $"MEM-{DateTime.Now.Year}-{_contadorMemoria:D4}";
            Items.Add(item);
            return (false, error);
        }

        return (true, "Incidencia reportada exitosamente, espere su respuesta ✨");
    }

    public static (bool valido, string mensaje) ValidarNumeroSerieEquipamiento(string numeroSerie)
    {
        string serieNormalizada = NormalizarNumeroSerie(numeroSerie);
        if (string.IsNullOrWhiteSpace(serieNormalizada))
            return (false, "El numero de serie es obligatorio para registrar la incidencia.");

        if (!long.TryParse(serieNormalizada, out long idSerie))
            return (false, "El numero de serie debe contener solo numeros. Puede escribirlo como 123456789 o SN-123456789.");

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM equipamientos WHERE id_serie = @id", conexion);
            cmd.Parameters.AddWithValue("id", idSerie);
            long total = Convert.ToInt64(cmd.ExecuteScalar());

            return total > 0
                ? (true, "OK")
                : (false, "El numero de serie ingresado no corresponde a ningun equipamiento registrado.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[IncidenciaListadoStore] Error al validar equipamiento: {ex.Message}");
            return (false, "No se pudo validar el numero de serie contra la base de datos. Verifique la conexion e intente nuevamente.");
        }
    }

    public static bool Eliminar(string folio)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            
            if (folio.StartsWith("INC-") && folio.Length > 9)
            {
                string idStr = folio.Substring(folio.LastIndexOf('-') + 1);
                if (long.TryParse(idStr, out long idDb))
                {
                    using var cmd = new NpgsqlCommand("DELETE FROM incidencias WHERE id_incidencia = @id", conexion);
                    cmd.Parameters.AddWithValue("id", idDb);
                    if (cmd.ExecuteNonQuery() > 0) return true;
                }
            }
        }
        catch { }

        var item = Items.FirstOrDefault(i => i.Folio == folio);
        if (item == null) return false;
        Items.Remove(item);
        return true;
    }

    public static void Actualizar(IncidenciaListadoItem actualizado)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            if (actualizado.Folio.StartsWith("INC-") && actualizado.Folio.Length > 9)
            {
                string idStr = actualizado.Folio.Substring(actualizado.Folio.LastIndexOf('-') + 1);
                if (long.TryParse(idStr, out long idDb))
                {
                    const string sql = """
                        UPDATE incidencias
                        SET estado           = @estado,
                            solucion         = @solucion,
                            fecha_atencion   = CURRENT_DATE,
                            id_administrador = @admin
                        WHERE id_incidencia = @id;
                        """;

                    using var cmd = new NpgsqlCommand(sql, conexion);
                    cmd.Parameters.AddWithValue("id",       idDb);
                    cmd.Parameters.AddWithValue("estado",   actualizado.Estado);
                    cmd.Parameters.AddWithValue("solucion", actualizado.DescripcionSolucion ?? string.Empty);
                    
                    if (SesionActual.EsAdministrador && !string.IsNullOrWhiteSpace(SesionActual.NombreUsuario))
                    {
                        cmd.Parameters.AddWithValue("admin", SesionActual.NombreUsuario);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("admin", DBNull.Value);
                    }

                    if (cmd.ExecuteNonQuery() > 0) return;
                }
            }
        }
        catch { }

        var idx = Items.FindIndex(i => i.Folio == actualizado.Folio);
        if (idx >= 0) Items[idx] = actualizado;
    }

    public static IncidenciaResumenEstadisticas ObtenerResumen()
    {
        var items = ObtenerTodas();
        return new IncidenciaResumenEstadisticas
        {
            Total     = items.Count,
            Activas   = items.Count(i => i.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) || i.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase)),
            EnProceso = items.Count(i => i.Estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase) || i.Estado.Equals("en_proceso", StringComparison.OrdinalIgnoreCase)),
            Resueltas = items.Count(i => i.Estado.Equals("Resuelta", StringComparison.OrdinalIgnoreCase) || i.Estado.Equals("resuelto", StringComparison.OrdinalIgnoreCase))
        };
    }

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerHistorialPorNumeroSerie(string numeroSerie, string? idActual = null)
    {
        string serieNormalizada = NormalizarNumeroSerie(numeroSerie);
        if (string.IsNullOrWhiteSpace(serieNormalizada))
            return Array.Empty<IncidenciaListadoItem>();

        return ObtenerTodas()
            .Where(i =>
                NormalizarNumeroSerie(i.NumeroSerie).Equals(serieNormalizada, StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrWhiteSpace(idActual) || !i.IdReal.Equals(idActual, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(i => i.Fecha)
            .ToList();
    }

    private static ResultadoGuardado GuardarEnBaseDeDatos(IncidenciaListadoItem item, Incidencia incidencia, out string error)
    {
        error = string.Empty;
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // 1. Extraer o crear id_serie
            long idSerie;
            string numSerieRaw = (incidencia.NumeroSerie ?? "").Replace("SN-", "").Replace("SN", "").Trim();
            if (!long.TryParse(numSerieRaw, out idSerie))
            {
                idSerie = 0; // fallback genérico si no pone número válido
            }

            if (!long.TryParse(numSerieRaw, out _))
            {
                error = "El numero de serie debe contener solo numeros. Puede escribirlo como 123456789 o SN-123456789.";
                return ResultadoGuardado.ErrorValidacion;
            }

            using (var cmdExisteEquipo = new NpgsqlCommand("SELECT COUNT(*) FROM equipamientos WHERE id_serie = @id", conexion))
            {
                cmdExisteEquipo.Parameters.AddWithValue("id", idSerie);
                long totalEquipamientos = Convert.ToInt64(cmdExisteEquipo.ExecuteScalar());
                if (totalEquipamientos == 0)
                {
                    error = "El numero de serie ingresado no corresponde a ningun equipamiento registrado.";
                    return ResultadoGuardado.ErrorValidacion;
                }
            }

            // Insertamos un equipo por defecto si no existe para evitar error de FK
            using (var cmdEq = new NpgsqlCommand("INSERT INTO equipamientos (id_serie, nombre, tipo_equipamiento) VALUES (@id, @nom, @tipo) ON CONFLICT (id_serie) DO NOTHING", conexion))
            {
                cmdEq.Parameters.AddWithValue("id", idSerie);
                cmdEq.Parameters.AddWithValue("nom", string.IsNullOrWhiteSpace(incidencia.NombreEquipo) ? "Desconocido" : incidencia.NombreEquipo);
                cmdEq.Parameters.AddWithValue("tipo", string.IsNullOrWhiteSpace(incidencia.TipoIncidencia) ? "Hardware" : incidencia.TipoIncidencia);
                cmdEq.ExecuteNonQuery();
            }

            // 2. Extraer id_alumno del usuario conectado
            string idAlumno = ObtenerIdAlumnoActual(conexion)
                ?? (string.IsNullOrWhiteSpace(SesionActual.NombreUsuario) ? "admin" : SesionActual.NombreUsuario);

            // Insertamos alumno genérico si es "admin" u otro, solo para que no falle la FK
            // si el alumno fue eliminado de la DB, aunque lo ideal es que esté logueado.
            using (var cmdAl = new NpgsqlCommand("INSERT INTO alumnos (id_alumno, numero_control, nombre, primer_apellido, correo, semestre, grupo, rol, contrasena, activo, fecha_registro) VALUES (@id, 999999999, 'Usuario', 'Generico', 'dummy@test.com', 'N/A', 'N/A', 'Estudiante', '', true, NOW()) ON CONFLICT (id_alumno) DO NOTHING", conexion))
            {
                cmdAl.Parameters.AddWithValue("id", idAlumno);
                cmdAl.ExecuteNonQuery();
            }

            // 3. Insert real
            const string sql = """
                INSERT INTO incidencias
                    (id_serie, id_alumno, titulo, descripcion, estado, 
                     fecha_reporte, hora_reporte, evidencia_foto)
                VALUES
                    (@id_serie, @id_alumno, @titulo, @descripcion, @estado, 
                     @fecha_reporte, @hora_reporte, @evidencia)
                RETURNING id_incidencia;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("id_serie",      idSerie);
            cmd.Parameters.AddWithValue("id_alumno",     idAlumno);
            cmd.Parameters.AddWithValue("titulo",        item.Titulo       ?? string.Empty);
            cmd.Parameters.AddWithValue("descripcion",   item.Descripcion  ?? string.Empty);
            cmd.Parameters.AddWithValue("estado",        "Activa");
            cmd.Parameters.AddWithValue("fecha_reporte", item.Fecha.Date);
            cmd.Parameters.AddWithValue("hora_reporte",  item.Fecha.TimeOfDay);
            cmd.Parameters.AddWithValue("evidencia",
                string.IsNullOrWhiteSpace(incidencia.RutaEvidencia) || incidencia.RutaEvidencia == "Ningún archivo seleccionado"
                    ? DBNull.Value
                    : (object)incidencia.RutaEvidencia);

            long newId = Convert.ToInt64(cmd.ExecuteScalar());
            item.Folio = $"INC-{item.Fecha.Year}-{newId:D4}";

            return ResultadoGuardado.Guardado;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            System.Diagnostics.Debug.WriteLine($"[IncidenciaListadoStore] Error al guardar: {ex.Message}");
            return ResultadoGuardado.ErrorBaseDatos;
        }
    }

    private static string NormalizarNumeroSerie(string? numeroSerie)
    {
        string valor = (numeroSerie ?? string.Empty).Trim();
        if (valor.StartsWith("SN-", StringComparison.OrdinalIgnoreCase))
            valor = valor[3..];
        else if (valor.StartsWith("SN", StringComparison.OrdinalIgnoreCase))
            valor = valor[2..];

        return valor.Trim();
    }

    private static string? ObtenerIdAlumnoActual(NpgsqlConnection conexion)
    {
        string usuario = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(usuario))
            return null;

        const string sql = """
            SELECT id_alumno
            FROM alumnos
            WHERE id_alumno = @usuario
               OR numero_control::text = @usuario
               OR usuario = @usuario
            LIMIT 1;
            """;

        using var cmd = new NpgsqlCommand(sql, conexion);
        cmd.Parameters.AddWithValue("usuario", usuario);
        var result = cmd.ExecuteScalar();
        return result?.ToString();
    }

    private static IReadOnlyList<IncidenciaListadoItem> ObtenerItemsMemoriaSegunSesion()
    {
        if (SesionActual.EsAdministrador)
            return Items.ToList();

        string usuario = SesionActual.NombreUsuario;
        string nombre = PerfilUsuarioStore.Obtener().NombreCompleto;

        return Items
            .Where(i =>
                (!string.IsNullOrWhiteSpace(usuario) &&
                 (i.IdAlumno.Equals(usuario, StringComparison.OrdinalIgnoreCase) ||
                  i.NumeroControl.Equals(usuario, StringComparison.OrdinalIgnoreCase))) ||
                (!string.IsNullOrWhiteSpace(nombre) &&
                 i.QuienReporta.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
