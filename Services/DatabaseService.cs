using Npgsql;

namespace Proyecto_GALAB.Services
{
    public static class DatabaseService
    {
        private const string CadenaConexionDefault =
            "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=galab";

        public static string CadenaConexion =>
            Environment.GetEnvironmentVariable("GALAB_DB_CONNECTION") ?? CadenaConexionDefault;

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(CadenaConexion);
        }

        public static bool ProbarConexion()
        {
            try
            {
                using var conexion = GetConnection();
                conexion.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void InicializarBaseDeDatos()
        {
            try
            {
                using var conexion = GetConnection();
                conexion.Open();

                // 1. Crear la secuencia si no existe
                string sqlSeq = @"
                    CREATE SEQUENCE IF NOT EXISTS public.incidencias_id_incidencia_seq
                        START WITH 10
                        INCREMENT BY 1
                        NO MINVALUE
                        NO MAXVALUE
                        CACHE 1;
                ";
                using (var cmdSeq = new NpgsqlCommand(sqlSeq, conexion))
                {
                    cmdSeq.ExecuteNonQuery();
                }

                // 2. Asegurar el tipo y valor predeterminado para id_incidencia
                string sqlAlter = @"
                    ALTER TABLE public.incidencias ALTER COLUMN id_incidencia SET DATA TYPE bigint;
                    ALTER TABLE public.incidencias ALTER COLUMN id_incidencia SET NOT NULL;
                    ALTER TABLE public.incidencias ALTER COLUMN id_incidencia SET DEFAULT nextval('public.incidencias_id_incidencia_seq'::regclass);
                ";
                using (var cmdAlter = new NpgsqlCommand(sqlAlter, conexion))
                {
                    cmdAlter.ExecuteNonQuery();
                }

                // 3. Modificar la restricción CHECK de estado para admitir valores en español
                string sqlConstraint = @"
                    ALTER TABLE public.incidencias DROP CONSTRAINT IF EXISTS incidencias_estado_check;
                    ALTER TABLE public.incidencias ADD CONSTRAINT incidencias_estado_check 
                        CHECK (estado::text = ANY (ARRAY['pendiente'::text, 'en_proceso'::text, 'resuelto'::text, 'Activa'::text, 'En proceso'::text, 'Resuelta'::text]));
                ";
                using (var cmdConstraint = new NpgsqlCommand(sqlConstraint, conexion))
                {
                    cmdConstraint.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DatabaseService] Error al inicializar base de datos: {ex.Message}");
            }
        }
    }
}
