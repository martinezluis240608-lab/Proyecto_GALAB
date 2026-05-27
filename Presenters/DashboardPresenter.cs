using Proyecto_GALAB.Interfaces;

namespace Proyecto_GALAB.Presenters;

public class DashboardPresenter
{
    private readonly IDashboardView _view;

    public DashboardPresenter(IDashboardView view)
    {
        _view = view;
        _view.OnCargarDatos         += OnCargarDatos;
        _view.OnRegistrarIncidencia += OnRegistrarIncidencia;
        _view.OnVerLista            += OnVerLista;
    }

    private void OnCargarDatos(object? sender, EventArgs e)
    {
        // Aquí en el futuro conectas la BD
        // Por ahora datos de ejemplo
        _view.TotalEnviadas   = 12;
        _view.TotalEnProgreso = 5;
        _view.TotalResueltas  = 8;

        var notificaciones = new List<(string admin, string estado, string tiempo)>
        {
            ("ADMIN 1", "Recibida",      "Hace 3 min"),
            ("ADMIN 2", "Estamos en eso","Hace 3 días"),
        };
        _view.CargarNotificaciones(notificaciones);
    }

    private void OnRegistrarIncidencia(object? sender, EventArgs e) =>
        _view.NavegarARegistro();

    private void OnVerLista(object? sender, EventArgs e) =>
        _view.NavegarALista();
}
