using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;

namespace Proyecto_GALAB.Presenters;

public class IncidenciaPresenter
{
    private readonly IIncidenciaView _view;

    public IncidenciaPresenter(IIncidenciaView view)
    {
        _view = view;
        _view.OnEnviarReporte += OnEnviarReporte;
        _view.OnAdjuntar      += OnAdjuntar;
    }

    private void OnAdjuntar(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title  = "Seleccionar evidencia",
            Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
            _view.RutaEvidencia = dialog.FileName;
    }

    private void OnEnviarReporte(object? sender, EventArgs e)
    {
        var incidencia = new Incidencia
        {
            QuienReporta   = _view.QuienReporta,
            TipoIncidencia = _view.TipoIncidencia,
            NombreEquipo   = _view.NombreEquipo,
            FechaHora      = _view.FechaHora,
            Descripcion    = _view.Descripcion,
            RutaEvidencia  = _view.RutaEvidencia
        };

        var (valido, mensaje) = incidencia.Validar();
        if (!valido)
        {
            _view.MostrarMensaje(mensaje);
            return;
        }

        // Aquí conectarías la BD en el futuro
        _view.MostrarMensaje("✅ Reporte enviado correctamente.");
        _view.LimpiarFormulario();
    }
}
