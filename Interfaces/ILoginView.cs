using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Interfaces;

public interface ILoginView
{
    string Usuario { get; }
    string Contrasena { get; }
    RolUsuario RolSeleccionado { get; }
    void MostrarError(string mensaje);
    void NavegarComoEstudiante();
    void NavegarComoAdministrador();
    event EventHandler OnIniciarSesion;
}
