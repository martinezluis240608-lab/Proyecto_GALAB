using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Presenters;

public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly Usuario    _model;

    public LoginPresenter(ILoginView view)
    {
        _view  = view;
        _model = new Usuario();
        _view.OnIniciarSesion += OnIniciarSesion;
    }

    private void OnIniciarSesion(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_view.Usuario) ||
            string.IsNullOrWhiteSpace(_view.Contrasena))
        {
            _view.MostrarError("Por favor complete todos los campos.");
            return;
        }

        if (!_model.Autenticar(_view.Usuario, _view.Contrasena, _view.RolSeleccionado))
        {
            _view.MostrarError(_view.RolSeleccionado == RolUsuario.Administrador
                ? "Usuario o contraseña de administrador incorrectos."
                : "Usuario o contraseña incorrectos.");
            return;
        }

        SesionActual.Iniciar(_view.Usuario.Trim(), _view.RolSeleccionado);

        if (_view.RolSeleccionado == RolUsuario.Estudiante)
            _view.NavegarComoEstudiante();
        else
            _view.NavegarComoAdministrador();
    }
}
