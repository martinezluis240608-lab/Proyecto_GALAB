using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Services;

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
        var resumen = new IncidenciaEstadisticasService().ObtenerResumen();
        _view.TotalEnviadas   = resumen.Activas;
        _view.TotalEnProgreso = resumen.EnProceso;
        _view.TotalResueltas  = resumen.Resueltas;

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
