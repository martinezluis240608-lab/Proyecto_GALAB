using Proyecto_GALAB.Interfaces;

namespace Proyecto_GALAB.Presenters;

public class PrincipalPresenter
{
    private readonly IPrincipalView _view;

    public PrincipalPresenter(IPrincipalView view)
    {
        _view = view;
        _view.OnPerfil             += (s, e) => _view.NavegarAPerfil();
        _view.OnGestionIncidencias += (s, e) => _view.NavegarAGestion();
        _view.OnAyuda              += (s, e) => _view.MostrarAyuda();
        _view.OnContacto           += (s, e) => _view.MostrarContacto();
    }
}
