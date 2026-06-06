namespace Proyecto_GALAB.Interfaces;

public interface IIncidenciaView
{
    string Titulo { get; }
    string QuienReporta { get; }
    string TipoIncidencia { get; }
    string NombreEquipo { get; }
    DateTime FechaHora { get; }
    string Descripcion { get; }
    string RutaEvidencia { get; set; }
    string NumeroSerie { get; }
    void MostrarExito(string mensaje);
    void MostrarError(string mensaje);
    void MostrarMensaje(string mensaje);
    void LimpiarFormulario();
    event EventHandler OnEnviarReporte;
    event EventHandler OnAdjuntar;
}
