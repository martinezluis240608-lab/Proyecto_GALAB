namespace Proyecto_GALAB.Interfaces;

public interface ILoginView
{
    string Usuario { get; }
    string Contrasena { get; }
    void MostrarError(string mensaje);
    void NavegarARegistro();
    event EventHandler OnIniciarSesion;
}
