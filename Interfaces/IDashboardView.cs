namespace Proyecto_GALAB.Interfaces;

public interface IDashboardView
{
    int TotalEnviadas    { get; set; }
    int TotalEnProgreso  { get; set; }
    int TotalResueltas   { get; set; }
    void CargarNotificaciones(List<(string admin, string estado, string tiempo)> notificaciones);
    void NavegarARegistro();
    void NavegarALista();
    event EventHandler OnRegistrarIncidencia;
    event EventHandler OnVerLista;
    event EventHandler OnCargarDatos;
}
