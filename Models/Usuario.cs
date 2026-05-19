namespace Proyecto_GALAB.Models;

public class Usuario
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena   { get; set; } = string.Empty;

    // Validación simple (aquí conectarías la BD en el futuro)
    public bool Autenticar(string usuario, string contrasena)
    {
        return usuario == "admin" && contrasena == "1234";
    }
}
