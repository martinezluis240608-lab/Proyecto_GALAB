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

    private static long ObtenerIdDesdeFolio(string folio)
    {
        if (string.IsNullOrWhiteSpace(folio)) return 0;
        var parts = folio.Split('-');
        if (parts.Length == 3 && long.TryParse(parts[2], out long id))
        {
            return id;
        }
        return 0;
    }

    private static long ObtenerIdSerie(NpgsqlConnection conexion, string nombreEquipo, string tipoEquipamiento)
    {
        if (string.IsNullOrWhiteSpace(nombreEquipo))
            nombreEquipo = "Equipo Genérico";
        if (string.IsNullOrWhiteSpace(tipoEquipamiento))
            tipoEquipamiento = "Otros";

        // 1. Intentar buscar por nombre
        const string sqlSelect = "SELECT id_serie FROM equipamientos WHERE nombre = @nombre LIMIT 1";
        using (var cmdSelect = new NpgsqlCommand(sqlSelect, conexion))
        {
            cmdSelect.Parameters.AddWithValue("nombre", nombreEquipo.Trim());
            var res = cmdSelect.ExecuteScalar();
            if (res != null)
                return Convert.ToInt64(res);
        }

        // 2. Si no existe, crear uno nuevo con un nuevo id_serie
        const string sqlMax = "SELECT COALESCE(MAX(id_serie), 0) FROM equipamientos";
        long newId = 1;
        using (var cmdMax = new NpgsqlCommand(sqlMax, conexion))
        {
            newId = Convert.ToInt64(cmdMax.ExecuteScalar()) + 1;
        }

        const string sqlInsert = """
            INSERT INTO equipamientos (id_serie, nombre, tipo_equipamiento, descripcion)
            VALUES (@id, @nombre, @tipo, @desc);
            """;
        using (var cmdInsert = new NpgsqlCommand(sqlInsert, conexion))
        {
            cmdInsert.Parameters.AddWithValue("id", newId);
            cmdInsert.Parameters.AddWithValue("nombre", nombreEquipo.Trim());
            cmdInsert.Parameters.AddWithValue("tipo", tipoEquipamiento.Trim());
            cmdInsert.Parameters.AddWithValue("desc", "Registrado automáticamente al reportar incidencia.");
            cmdInsert.ExecuteNonQuery();
        }

        return newId;
    }

    private static string ObtenerIdAlumno(NpgsqlConnection conexion, string quienReporta)
    {
        if (string.IsNullOrWhiteSpace(quienReporta))
            return "ALU-001"; // Fallback

        // 1. Intentar buscar por id_alumno, correo o nombre completo
        const string sqlSelect = """
            SELECT id_alumno FROM alumnos 
            WHERE id_alumno = @quien 
               OR correo = @quien 
               OR TRIM(CONCAT(nombre, ' ', primer_apellido, ' ', COALESCE(segundo_apellido, ''))) = @quien 
            LIMIT 1;
            """;
        using (var cmdSelect = new NpgsqlCommand(sqlSelect, conexion))
        {
            cmdSelect.Parameters.AddWithValue("quien", quienReporta.Trim());
            var res = cmdSelect.ExecuteScalar();
            if (res != null)
                return res.ToString() ?? "ALU-001";
        }

        // 2. Si no existe, buscar el primer alumno
        const string sqlFirst = "SELECT id_alumno FROM alumnos LIMIT 1";
        using (var cmdFirst = new NpgsqlCommand(sqlFirst, conexion))
        {
            var res = cmdFirst.ExecuteScalar();
            if (res != null)
                return res.ToString() ?? "ALU-001";
        }

        // 3. Si no hay alumnos, crear uno temporal para la FK
        const string sqlInsert = """
            INSERT INTO alumnos (id_alumno, numero_control, nombre, primer_apellido, segundo_apellido, correo, rol, contrasena, activo)
            VALUES ('ALU-001', 1, 'Estudiante', 'Temporal', '', 'estudiante@galab.com', 'Estudiante', 'estudiante', true);
            """;
        using (var cmdInsert = new NpgsqlCommand(sqlInsert, conexion))
        {
            cmdInsert.ExecuteNonQuery();
        }
        return "ALU-001";
    }

    private static string ObtenerIdAdministrador(NpgsqlConnection conexion, string usuarioAdmin)
    {
        if (string.IsNullOrWhiteSpace(usuarioAdmin))
            return null;

        const string sqlSelect = """
            SELECT id_administrador FROM administradores 
            WHERE id_administrador = @usr 
               OR usuario = @usr 
               OR correo = @usr 
               OR TRIM(CONCAT(nombre, ' ', primer_apellido, ' ', COALESCE(segundo_apellido, ''))) = @usr 
            LIMIT 1;
            """;
        using (var cmdSelect = new NpgsqlCommand(sqlSelect, conexion))
        {
            cmdSelect.Parameters.AddWithValue("usr", usuarioAdmin.Trim());
            var res = cmdSelect.ExecuteScalar();
            if (res != null)
                return res.ToString();
        }
        return null;
    }

    public static IReadOnlyList<IncidenciaListadoItem> ObtenerTodas()
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            const string sql = """
                SELECT 
                    i.id_incidencia,
                    i.fecha_reporte,
                    i.hora_reporte,
                    i.titulo,
                    TRIM(CONCAT(a.nombre, ' ', a.primer_apellido, ' ', COALESCE(a.segundo_apellido, ''))) AS quien_reporta,
                    e.tipo_equipamiento,
                    i.estado,
                    i.descripcion,
                    e.nombre AS nombre_equipo,
                    i.solucion
                FROM incidencias i
                LEFT JOIN alumnos a ON i.id_alumno = a.id_alumno
                LEFT JOIN equipamientos e ON i.id_serie = e.id_serie
                ORDER BY i.fecha_reporte DESC, i.hora_reporte DESC, i.id_incidencia DESC;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            using var reader = cmd.ExecuteReader();
            var incidencias = new List<IncidenciaListadoItem>();
            while (reader.Read())
            {
                long id = reader.GetInt64(0);
                DateTime datePart = reader.IsDBNull(1) ? DateTime.Today : reader.GetDateTime(1);
                TimeSpan timePart = reader.IsDBNull(2) ? TimeSpan.Zero : reader.GetTimeSpan(2);
                DateTime fullDateTime = datePart.Add(timePart);

                incidencias.Add(new IncidenciaListadoItem
                {
                    Folio = $"INC-{datePart.Year}-{id:D4}",
                    Titulo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    QuienReporta = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    TipoIncidencia = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Estado = reader.IsDBNull(6) ? "Activa" : reader.GetString(6),
                    Fecha = fullDateTime,
                    Descripcion = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Equipo = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    DescripcionSolucion = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
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
            long id = ObtenerIdDesdeFolio(folio);
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM incidencias WHERE id_incidencia = @id", conexion);
            cmd.Parameters.AddWithValue("id", id);
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

            long id = ObtenerIdDesdeFolio(actualizado.Folio);
            string idAdmin = ObtenerIdAdministrador(conexion, SesionActual.NombreUsuario);

            const string sql = """
                UPDATE incidencias
                SET estado = @estado,
                    solucion = @solucion,
                    id_administrador = COALESCE(@id_admin, id_administrador),
                    fecha_cierre = CASE WHEN @estado = 'Resuelta' THEN CURRENT_DATE ELSE fecha_cierre END
                WHERE id_incidencia = @id;
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("estado", actualizado.Estado);
            cmd.Parameters.AddWithValue("solucion", actualizado.DescripcionSolucion ?? "");
            cmd.Parameters.AddWithValue("id_admin", (object)idAdmin ?? DBNull.Value);
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

            long id = ObtenerIdDesdeFolio(item.Folio);
            long idSerie = ObtenerIdSerie(conexion, item.Equipo, item.TipoIncidencia);
            string idAlumno = ObtenerIdAlumno(conexion, item.QuienReporta);
            string idAdmin = ObtenerIdAdministrador(conexion, SesionActual.NombreUsuario);

            const string sql = """
                INSERT INTO incidencias
                    (id_incidencia, id_serie, id_alumno, id_administrador, titulo, descripcion, estado, fecha_reporte, hora_reporte, solucion, evidencia_foto)
                VALUES
                    (@id, @id_serie, @id_alumno, @id_admin, @titulo, @descripcion, @estado, @fecha, @hora, @solucion, @evidencia)
                ON CONFLICT (id_incidencia) DO UPDATE
                SET estado = EXCLUDED.estado,
                    solucion = EXCLUDED.solucion,
                    id_administrador = COALESCE(EXCLUDED.id_administrador, incidencias.id_administrador),
                    evidencia_foto = COALESCE(EXCLUDED.evidencia_foto, incidencias.evidencia_foto);
                """;
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("id_serie", idSerie);
            cmd.Parameters.AddWithValue("id_alumno", idAlumno);
            cmd.Parameters.AddWithValue("id_admin", (object)idAdmin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("titulo", item.Titulo);
            cmd.Parameters.AddWithValue("descripcion", item.Descripcion);
            cmd.Parameters.AddWithValue("estado", item.Estado);
            cmd.Parameters.AddWithValue("fecha", item.Fecha.Date);
            cmd.Parameters.AddWithValue("hora", item.Fecha.TimeOfDay);
            cmd.Parameters.AddWithValue("solucion", item.DescripcionSolucion ?? "");
            cmd.Parameters.AddWithValue("evidencia", string.IsNullOrWhiteSpace(rutaEvidencia) ? DBNull.Value : rutaEvidencia);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
