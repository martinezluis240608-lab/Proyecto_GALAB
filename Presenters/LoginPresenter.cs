using Npgsql;
using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Presenters;

/// <summary>
/// Autentica contra las tablas reales de la BD.
/// Soporta contraseñas en texto plano Y encriptadas con pgcrypto (bcrypt).
/// 
/// Si contrasena empieza con "$2a$", "$2b$" o "$2y$" → es bcrypt, usa crypt() en SQL.
/// Si no                                              → texto plano, comparación directa.
/// 
/// Esto permite tener alumnos con ambos tipos en la misma BD.
/// </summary>
public class LoginPresenter
{
    private readonly ILoginView _vista;

    public LoginPresenter(ILoginView vista)
    {
        _vista = vista;
        _vista.OnIniciarSesion += (_, _) => IniciarSesion();
    }

    private void IniciarSesion()
    {
        string usuario = _vista.Usuario.Trim();
        string contrasena = _vista.Contrasena;
        RolUsuario rol = _vista.RolSeleccionado;

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            _vista.MostrarError("Por favor ingresa tu usuario y contraseña.");
            return;
        }

        if (rol == RolUsuario.Estudiante)
            AutenticarEstudiante(usuario, contrasena);
        else
            AutenticarAdministrador(usuario, contrasena);
    }

    // ── Estudiante ────────────────────────────────────────────────────────────

    private void AutenticarEstudiante(string usuario, string contrasena)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Paso 1: obtener la contraseña almacenada para detectar el tipo
            const string sqlObtener = """
                SELECT id_alumno, contrasena, activo
                FROM alumnos
                WHERE numero_control::text = @usuario
                   OR usuario = @usuario
                LIMIT 1;
                """;

            using var cmdObtener = new NpgsqlCommand(sqlObtener, conexion);
            cmdObtener.Parameters.AddWithValue("usuario", usuario);

            string? hashAlmacenado = null;
            bool activo = false;

            using (var r = cmdObtener.ExecuteReader())
            {
                if (!r.Read())
                {
                    _vista.MostrarError("Usuario o contraseña incorrectos.");
                    return;
                }
                hashAlmacenado = r.IsDBNull(1) ? null : r.GetString(1);
                activo = !r.IsDBNull(2) && r.GetBoolean(2);
            }

            if (!activo)
            {
                _vista.MostrarError("Tu cuenta está inactiva. Contacta al administrador.");
                return;
            }

            // Paso 2: verificar contraseña según el tipo detectado
            bool passwordOk = VerificarPassword(conexion, contrasena, hashAlmacenado);

            if (!passwordOk)
            {
                _vista.MostrarError("Usuario o contraseña incorrectos.");
                return;
            }
        }
        catch (Exception ex)
        {
            _vista.MostrarError($"Error de conexión: {ex.Message}");
            return;
        }

        SesionActual.Iniciar(usuario, RolUsuario.Estudiante);
        _vista.NavegarComoEstudiante();
    }

    // ── Administrador ─────────────────────────────────────────────────────────

    private void AutenticarAdministrador(string usuario, string contrasena)
    {
        try
        {
            using var conexion = DatabaseService.GetConnection();
            conexion.Open();

            // Paso 1: obtener la contraseña almacenada
            const string sqlObtener = """
                SELECT id_administrador, contrasena, activo
                FROM administradores
                WHERE usuario = @usuario
                LIMIT 1;
                """;

            using var cmdObtener = new NpgsqlCommand(sqlObtener, conexion);
            cmdObtener.Parameters.AddWithValue("usuario", usuario);

            string? hashAlmacenado = null;
            bool activo = false;

            using (var r = cmdObtener.ExecuteReader())
            {
                if (!r.Read())
                {
                    _vista.MostrarError("Usuario o contraseña de administrador incorrectos.");
                    return;
                }
                hashAlmacenado = r.IsDBNull(1) ? null : r.GetString(1);
                activo = !r.IsDBNull(2) && r.GetBoolean(2);
            }

            if (!activo)
            {
                _vista.MostrarError("La cuenta de administrador está inactiva.");
                return;
            }

            // Paso 2: verificar contraseña según el tipo detectado
            bool passwordOk = VerificarPassword(conexion, contrasena, hashAlmacenado);

            if (!passwordOk)
            {
                _vista.MostrarError("Usuario o contraseña de administrador incorrectos.");
                return;
            }
        }
        catch (Exception ex)
        {
            _vista.MostrarError($"Error de conexión: {ex.Message}");
            return;
        }

        SesionActual.Iniciar(usuario, RolUsuario.Administrador);
        _vista.NavegarComoAdministrador();
    }

    // ── Verificación de contraseña (texto plano O bcrypt) ─────────────────────

    /// <summary>
    /// Detecta automáticamente si el hash es bcrypt (pgcrypto) o texto plano.
    /// Bcrypt empieza siempre con $2a$, $2b$ o $2y$ seguido del costo.
    /// </summary>
    private static bool VerificarPassword(NpgsqlConnection conexion,
                                           string contrasenaIngresada,
                                           string? hashAlmacenado)
    {
        if (hashAlmacenado is null)
            return false;

        // Detectar bcrypt por el prefijo estándar
        bool esBcrypt = hashAlmacenado.StartsWith("$2a$")
                     || hashAlmacenado.StartsWith("$2b$")
                     || hashAlmacenado.StartsWith("$2y$");

        if (esBcrypt)
        {
            // Usar la función crypt() de pgcrypto directamente en PostgreSQL
            // crypt(@ingresada, @hash) devuelve el mismo hash si la contraseña es correcta
            const string sql = "SELECT crypt(@ingresada, @hash) = @hash;";
            using var cmd = new NpgsqlCommand(sql, conexion);
            cmd.Parameters.AddWithValue("ingresada", contrasenaIngresada);
            cmd.Parameters.AddWithValue("hash", hashAlmacenado);
            var resultado = cmd.ExecuteScalar();
            return resultado is true;
        }
        else
        {
            // Texto plano: comparación directa
            return hashAlmacenado == contrasenaIngresada;
        }
    }
}
