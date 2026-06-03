using Npgsql;

namespace Proyecto_GALAB.Services
{
    public static class DatabaseService
    {
        private const string CadenaConexionDefault =
            "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=proyecto_galab";

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
    }
}
