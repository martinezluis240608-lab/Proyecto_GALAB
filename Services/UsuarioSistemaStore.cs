using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Services;

/// <summary>
/// Usuarios del sistema en memoria.
/// TODO(BD): persistir en base de datos.
/// </summary>
internal static class UsuarioSistemaStore
{
    private static readonly List<UsuarioSistema> Usuarios = new();
    private static int _contador;

    static UsuarioSistemaStore()
    {
        // Sin datos hasta conectar BD; lista vacía.
    }

    public static IReadOnlyList<UsuarioSistema> ObtenerTodos() => Usuarios.ToList();

    public static void Guardar(UsuarioSistema usuario, bool esNuevo)
    {
        if (esNuevo)
        {
            _contador++;
            usuario.Id = $"USR-{_contador:D3}";
            Usuarios.Add(usuario);
            return;
        }

        var idx = Usuarios.FindIndex(u => u.Id == usuario.Id);
        if (idx >= 0)
            Usuarios[idx] = usuario;
    }

    public static bool Eliminar(string id)
    {
        var u = Usuarios.FirstOrDefault(x => x.Id == id);
        if (u == null) return false;
        Usuarios.Remove(u);
        return true;
    }
}
