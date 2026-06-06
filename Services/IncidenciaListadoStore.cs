using Proyecto_GALAB.Models;
using Npgsql;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Listado de incidencias adaptado a la estructura real de la BD:
///   tabla incidencias: id_incidencia (PK bigint), id_serie (FK), id_alumno (FK),
///                      id_administrador (FK nullable), titulo, descripcion, estado,
///                      fecha_reporte (date), hora_reporte (time), fecha_atencion,
///                      fecha_cierre, solucion, evidencia_foto
///   tabla alumnos: id_alumno, numero_control, nombre, primer_apellido
///   tabla equipamientos: id_serie, nombre
///
/// Folio interno se construye como: INC-{año}-{id_incidencia:D4}
/// Estado de la BD: 'pendiente' | 'en_proceso' | 'resuelto'
///   → se mapea a: 'Activa' | 'En proceso' | 'Resuelta'
/// </summary>
internal static class IncidenciaListadoStore
{
    // Fallback en memoria si no hay BD
    private static readonly List<IncidenciaListadoItem> Items = new();

    // ── Mapeos de estado BD ↔ UI ─────────────────────────────────────────────
    private static string EstadoBdAUi(string estadoBd) => estadoBd.ToLowerInvariant() switch
    {
        "pendiente" => "Activa",
        "en_proceso" => "En proceso",
        "resuelto" => "Resuelta",
        _ => "Activa"
    };

    private static string EstadoUiABd(string estadoUi) => estadoUi switch
    {
        "Activa" => "pendiente",
        "En proceso" => "en_proceso",
        "Resuelta" => "resuelto",
        _ => "pendiente"
    };

    // ── ObtenerTodas ─────────────────────────────────────────────────────────

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerTodas()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            bool filtrarPorAlumno = !SesionActual.EsAdministrador;
            string? idAlumnoSesion = filtrarPorAlumno ? ObtenerIdAlumno() : null;
            if (filtrarPorAlumno && string.IsNullOrWhiteSpace(idAlumnoSesion))
                return ObtenerItemsMemoriaSegunSesion();

            const string sql = """
                SELECT i.id_incidencia,
                       i.titulo,
                       a.nombre || ' ' || a.primer_apellido AS quien_reporta,
                       e.nombre AS nombre_equipo,
                       i.estado,
                       i.fecha_reporte,
                       i.hora_reporte,
                       i.descripcion,
                       i.evidencia_foto
                FROM incidencias i
                LEFT JOIN alumnos       a ON a.id_alumno = i.id_alumno
                LEFT JOIN equipamientos e ON e.id_serie  = i.id_serie
                WHERE (@filtrar_alumno = FALSE OR i.id_alumno = @id_alumno)
                ORDER BY i.fecha_reporte DESC, i.id_incidencia DESC;
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("filtrar_alumno", filtrarPorAlumno);
            cmd.Parameters.AddWithValue("id_alumno", (object?)idAlumnoSesion ?? DBNull.Value);
            using var reader = cmd.ExecuteReader();

            var lista = new List<IncidenciaListadoItem>();
            while (reader.Read())
            {
                long id = reader.GetInt64(0);
                var fecha = reader.GetDateTime(5);
                TimeSpan hora = reader.IsDBNull(6)
                    ? TimeSpan.Zero
                    : reader.GetTimeSpan(6);

                lista.Add(new IncidenciaListadoItem
                {
                    Folio = $"INC-{fecha.Year}-{id:D4}",
                    Titulo = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    QuienReporta = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    // Tipo de incidencia no existe en la BD → se deja vacío
                    TipoIncidencia = "",
                    Equipo = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Estado = reader.IsDBNull(4) ? "Activa" : EstadoBdAUi(reader.GetString(4)),
                    Fecha = fecha.Add(hora),
                    Descripcion = reader.IsDBNull(7) ? "" : reader.GetString(7)
                });
            }
            return lista;
        }
        catch
        {
            return ObtenerItemsMemoriaSegunSesion();
        }
    }

    // ── Registrar nueva incidencia ────────────────────────────────────────────

    public static void RegistrarDesdeIncidencia(Incidencia incidencia)
    {
        // Para insertar necesitamos el id_alumno y el id_serie reales
        string? idAlumno = ObtenerIdAlumno();
        long? idSerie = ObtenerIdSerie(incidencia.NombreEquipo);

        if (idAlumno == null || idSerie == null)
        {
            // Fallback en memoria si no hay datos relacionales
            Items.Add(new IncidenciaListadoItem
            {
                Folio = $"INC-{DateTime.Now.Year}-{Items.Count + 1:D4}",
                Titulo = incidencia.Titulo,
                QuienReporta = incidencia.QuienReporta,
                TipoIncidencia = incidencia.TipoIncidencia,
                Estado = "Activa",
                Fecha = incidencia.FechaHora,
                Descripcion = incidencia.Descripcion,
                Equipo = incidencia.NombreEquipo
            });
            return;
        }

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            const string sql = """
                INSERT INTO incidencias
                    (id_serie, id_alumno, titulo, descripcion, estado,
                     fecha_reporte, hora_reporte, evidencia_foto)
                VALUES
                    (@serie, @alumno, @titulo, @descripcion, @estado,
                     @fecha, @hora, @evidencia);
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("serie", idSerie.Value);
            cmd.Parameters.AddWithValue("alumno", idAlumno);
            cmd.Parameters.AddWithValue("titulo", incidencia.Titulo);
            cmd.Parameters.AddWithValue("descripcion", incidencia.Descripcion);
            cmd.Parameters.AddWithValue("estado", EstadoUiABd("Activa"));
            cmd.Parameters.AddWithValue("fecha", incidencia.FechaHora.Date);
            cmd.Parameters.AddWithValue("hora", incidencia.FechaHora.TimeOfDay);
            cmd.Parameters.AddWithValue("evidencia",
                string.IsNullOrWhiteSpace(incidencia.RutaEvidencia)
                    ? (object)DBNull.Value
                    : incidencia.RutaEvidencia);
            cmd.ExecuteNonQuery();
        }
        catch
        {
            // Guardar en memoria como respaldo
            Items.Add(new IncidenciaListadoItem
            {
                Folio = $"INC-{DateTime.Now.Year}-{Items.Count + 1:D4}",
                Titulo = incidencia.Titulo,
                QuienReporta = incidencia.QuienReporta,
                TipoIncidencia = incidencia.TipoIncidencia,
                Estado = "Activa",
                Fecha = incidencia.FechaHora,
                Descripcion = incidencia.Descripcion,
                Equipo = incidencia.NombreEquipo
            });
        }
    }

    // ── Eliminar ─────────────────────────────────────────────────────────────

    public static bool Eliminar(string folio)
    {
        // El folio tiene formato INC-YYYY-NNNN; extraemos el ID
        long id = ExtraerIdDeFolio(folio);

        if (id > 0)
        {
            try
            {
                using var conexion = DatabaseService.GetConnection();
                conexion.Open();
                using var cmd = new NpgsqlCommand(
                    "DELETE FROM incidencias WHERE id_incidencia = @id", conexion);
                cmd.Parameters.AddWithValue("id", id);
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
            }
            catch { }
        }

        // Fallback en memoria
        var item = Items.FirstOrDefault(i => i.Folio == folio);
        if (item == null) return false;
        Items.Remove(item);
        return true;
    }

    // ── Actualizar ───────────────────────────────────────────────────────────

    public static void Actualizar(IncidenciaListadoItem actualizado)
    {
        long id = ExtraerIdDeFolio(actualizado.Folio);

        if (id > 0)
        {
            try
            {
                using var conexion = DatabaseService.GetConnection();
                conexion.Open();

                const string sql = """
                    UPDATE incidencias
                    SET titulo      = @titulo,
                        descripcion = @descripcion,
                        estado      = @estado
                    WHERE id_incidencia = @id;
                    """;

                using var cmd = new NpgsqlCommand(sql, conexion);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("titulo", actualizado.Titulo);
                cmd.Parameters.AddWithValue("descripcion", actualizado.Descripcion);
                cmd.Parameters.AddWithValue("estado", EstadoUiABd(actualizado.Estado));
                if (cmd.ExecuteNonQuery() > 0)
                    return;
            }
            catch { }
        }

        // Fallback en memoria
        var idx = Items.FindIndex(i => i.Folio == actualizado.Folio);
        if (idx >= 0)
            Items[idx] = actualizado;
    }

    // ── Resumen estadísticas ─────────────────────────────────────────────────

    public static IncidenciaResumenEstadisticas ObtenerResumen()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            bool filtrarPorAlumno = !SesionActual.EsAdministrador;
            string? idAlumnoSesion = filtrarPorAlumno ? ObtenerIdAlumno() : null;
            if (filtrarPorAlumno && string.IsNullOrWhiteSpace(idAlumnoSesion))
                return CrearResumenDesdeItems(ObtenerItemsMemoriaSegunSesion());

            const string sql = """
                SELECT
                    COUNT(*)                                           AS total,
                    COUNT(*) FILTER (WHERE estado = 'pendiente')      AS activas,
                    COUNT(*) FILTER (WHERE estado = 'en_proceso')     AS en_proceso,
                    COUNT(*) FILTER (WHERE estado = 'resuelto')       AS resueltas
                FROM incidencias
                WHERE (@filtrar_alumno = FALSE OR id_alumno = @id_alumno);
                """;

            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("filtrar_alumno", filtrarPorAlumno);
            cmd.Parameters.AddWithValue("id_alumno", (object?)idAlumnoSesion ?? DBNull.Value);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new IncidenciaResumenEstadisticas
                {
                    Total = reader.GetInt32(0),
                    Activas = reader.GetInt32(1),
                    EnProceso = reader.GetInt32(2),
                    Resueltas = reader.GetInt32(3)
                };
            }
        }
        catch { }

        // Fallback calculado desde la lista en memoria
        return CrearResumenDesdeItems(ObtenerItemsMemoriaSegunSesion());
    }

    private static IncidenciaResumenEstadisticas CrearResumenDesdeItems(IReadOnlyList<IncidenciaListadoItem> items)
    {
        return new IncidenciaResumenEstadisticas
        {
            Total = items.Count,
            Activas = items.Count(i => i.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase)),
            EnProceso = items.Count(i => i.Estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase)),
            Resueltas = items.Count(i => i.Estado.Equals("Resuelta", StringComparison.OrdinalIgnoreCase))
        };
    }

    // ── Helpers privados ─────────────────────────────────────────────────────

    /// <summary>Extrae el número de incidencia del folio "INC-YYYY-NNNN".</summary>
    private static long ExtraerIdDeFolio(string folio)
    {
        var partes = folio.Split('-');
        if (partes.Length == 3 && long.TryParse(partes[2], out long id))
            return id;
        return 0;
    }

    /// <summary>Devuelve el id_alumno del alumno actualmente en sesión.</summary>
    private static IReadOnlyList<IncidenciaListadoItem> ObtenerItemsMemoriaSegunSesion()
    {
        if (SesionActual.EsAdministrador)
            return Items.ToList();

        string usuario = SesionActual.NombreUsuario;
        string nombre = PerfilUsuarioStore.Obtener().NombreCompleto;

        return Items
            .Where(i =>
                (!string.IsNullOrWhiteSpace(nombre) &&
                 i.QuienReporta.Equals(nombre, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(usuario) &&
                 i.QuienReporta.Equals(usuario, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    private static string? ObtenerIdAlumno()
    {
        string control = SesionActual.NombreUsuario;
        if (string.IsNullOrWhiteSpace(control))
            return null;
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = """
                SELECT id_alumno FROM alumnos
                WHERE numero_control::text = @control OR usuario = @control
                LIMIT 1;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("control", control);
            var result = cmd.ExecuteScalar();
            return result is null ? null : result.ToString();
        }
        catch { return null; }
    }

    /// <summary>Devuelve el id_serie del equipo por nombre (primera coincidencia).</summary>
    private static long? ObtenerIdSerie(string nombreEquipo)
    {
        if (string.IsNullOrWhiteSpace(nombreEquipo))
        {
            // Si no hay nombre, usar el primer equipo disponible
            try
            {
                using var conexion = DatabaseService.GetConnection();
                conexion.Open();
                using var cmd = new NpgsqlCommand(
                    "SELECT id_serie FROM equipamientos ORDER BY id_serie LIMIT 1", conexion);
                var r = cmd.ExecuteScalar();
                return r is null ? null : Convert.ToInt64(r);
            }
            catch { return null; }
        }

        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = """
                SELECT id_serie FROM equipamientos
                WHERE LOWER(nombre) LIKE LOWER(@nombre)
                ORDER BY id_serie LIMIT 1;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("nombre", $"%{nombreEquipo}%");
            var result = cmd.ExecuteScalar();
            if (result is not null)
                return Convert.ToInt64(result);

            // Si no encontró por nombre, usar el primero disponible
            using var cmd2 = new NpgsqlCommand(
                "SELECT id_serie FROM equipamientos ORDER BY id_serie LIMIT 1", conexion);
            var r2 = cmd2.ExecuteScalar();
            return r2 is null ? null : Convert.ToInt64(r2);
        }
        catch { return null; }
    }
}
