namespace Proyecto_GALAB.Interfaces
{
    // Contrato que la Vista debe cumplir
    // El Presenter solo conoce esta interfaz, nunca el Form directamente
    public interface IMainView
    {
        string NombreUsuario { get; }
        void MostrarMensaje(string mensaje);
        event EventHandler OnSaludar;
    }
}
