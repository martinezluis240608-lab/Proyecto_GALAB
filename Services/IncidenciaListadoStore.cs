using Proyecto_GALAB.Models;
using Npgsql;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Listado en memoria de incidencias (admin y reportes).
/// TODO(BD): reemplazar por repositorio SQL.
/// </summary>
internal static class IncidenciaListadoStore
{
    private static readonly List<IncidenciaListadoItem> Items = new();
    private static int _contador;

    static IncidenciaListadoStore()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = "SELECT COALESCE(MAX(id_incidencia), 0) FROM incidencias";
            using var cmd = new NpgsqlCommand(sql, conexion);
            _contador = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            _contador = 0;
        }
    }

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerTodas()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = """
                SELECT folio, titulo, quien_reporta, tipo_incidencia, estado, fecha_hora, descripcion, nombre_equipo
                FROM incidencias
                ORDER BY fecha_hora DESC, id_incidencia DESC;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            using var reader = cmd.ExecuteReader();
            var incidencias = new List<IncidenciaListadoItem>();
            while (reader.Read())
            {
                incidencias.Add(new IncidenciaListadoItem
                {
                    Folio = reader.GetString(0),
                    Titulo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    QuienReporta = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    TipoIncidencia = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Estado = reader.IsDBNull(4) ? "Activa" : reader.GetString(4),
                    Fecha = reader.GetDateTime(5),
                    Descripcion = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Equipo = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                });
            }

            return incidencias;
        }
        catch
        {
            return Items.ToList();
        }
    }

    public static void RegistrarDesdeIncidencia(Incidencia incidencia)
    {
        _contador++;
        var item = new IncidenciaListadoItem
        {
            Folio = $"INC-{DateTime.Now.Year}-{_contador:D4}",
            Titulo = incidencia.Titulo,
            QuienReporta = incidencia.QuienReporta,
            TipoIncidencia = incidencia.TipoIncidencia,
            Estado = "Activa",
            Fecha = incidencia.FechaHora,
            Descripcion = incidencia.Descripcion,
            Equipo = incidencia.NombreEquipo
        };

        if (GuardarEnBaseDeDatos(item, incidencia.RutaEvidencia))
            return;

        Items.Add(item);
    }

    public static bool Eliminar(string folio)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM incidencias WHERE folio = @folio", conexion);
            cmd.Parameters.AddWithValue("folio", folio);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
        }
        catch
        {
            // Si no hay base disponible, se intenta eliminar de la lista local.
        }

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
            const string sql = """
                UPDATE incidencias
                SET titulo = @titulo,
                    tipo_incidencia = @tipo,
                    estado = @estado,
                    fecha_hora = @fecha,
                    descripcion = @descripcion,
                    nombre_equipo = @equipo,
                    actualizado_en = NOW()
                WHERE folio = @folio;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("folio", actualizado.Folio);
            cmd.Parameters.AddWithValue("titulo", actualizado.Titulo);
            cmd.Parameters.AddWithValue("tipo", actualizado.TipoIncidencia);
            cmd.Parameters.AddWithValue("estado", actualizado.Estado);
            cmd.Parameters.AddWithValue("fecha", actualizado.Fecha);
            cmd.Parameters.AddWithValue("descripcion", actualizado.Descripcion);
            cmd.Parameters.AddWithValue("equipo", actualizado.Equipo);
            if (cmd.ExecuteNonQuery() > 0)
                return;
        }
        catch
        {
            // Respaldo en memoria.
        }

        var idx = Items.FindIndex(i => i.Folio == actualizado.Folio);
        if (idx >= 0)
            Items[idx] = actualizado;
    }

    public static IncidenciaResumenEstadisticas ObtenerResumen()
    {
        var items = ObtenerTodas();
        return new IncidenciaResumenEstadisticas
        {
            Total = items.Count,
            Activas = items.Count(i => i.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase)),
            EnProceso = items.Count(i => i.Estado.Equals("En proceso", StringComparison.OrdinalIgnoreCase)),
            Resueltas = items.Count(i => i.Estado.Equals("Resuelta", StringComparison.OrdinalIgnoreCase))
        };
    }

    private static bool GuardarEnBaseDeDatos(IncidenciaListadoItem item, string rutaEvidencia)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = """
                INSERT INTO incidencias
                    (folio, titulo, quien_reporta, tipo_incidencia, nombre_equipo, fecha_hora, descripcion, ruta_evidencia, estado)
                VALUES
                    (@folio, @titulo, @quien_reporta, @tipo, @equipo, @fecha, @descripcion, @ruta, @estado);
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("folio", item.Folio);
            cmd.Parameters.AddWithValue("titulo", item.Titulo);
            cmd.Parameters.AddWithValue("quien_reporta", item.QuienReporta);
            cmd.Parameters.AddWithValue("tipo", item.TipoIncidencia);
            cmd.Parameters.AddWithValue("equipo", item.Equipo);
            cmd.Parameters.AddWithValue("fecha", item.Fecha);
            cmd.Parameters.AddWithValue("descripcion", item.Descripcion);
            cmd.Parameters.AddWithValue("ruta", string.IsNullOrWhiteSpace(rutaEvidencia) ? DBNull.Value : rutaEvidencia);
            cmd.Parameters.AddWithValue("estado", item.Estado);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
