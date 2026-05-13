using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Presenters
{
    // El Presenter coordina la Vista y el Model
    // No tiene ninguna referencia a controles de UI
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly Usuario _model;

        public MainPresenter(IMainView view)
        {
            _view = view;
            _model = new Usuario();

            // Suscribirse al evento de la Vista
            _view.OnSaludar += OnSaludar;
        }

        private void OnSaludar(object? sender, EventArgs e)
        {
            _model.Nombre = _view.NombreUsuario;
            string mensaje = _model.ObtenerSaludo();
            _view.MostrarMensaje(mensaje);
        }
    }
}
