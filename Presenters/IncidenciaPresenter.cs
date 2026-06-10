using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

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
            Titulo         = _view.Titulo,
            QuienReporta   = _view.QuienReporta,
            TipoIncidencia = _view.TipoIncidencia,
            NombreEquipo   = _view.NombreEquipo,
            FechaHora      = _view.FechaHora,
            Descripcion    = _view.Descripcion,
            RutaEvidencia  = _view.RutaEvidencia,
            NumeroSerie    = _view.NumeroSerie
        };

        var (valido, mensaje) = incidencia.Validar();
        if (!valido)
        {
            _view.MostrarError(mensaje);
            return;
        }

        var (serieValida, mensajeSerie) = IncidenciaListadoStore.ValidarNumeroSerieEquipamiento(incidencia.NumeroSerie);
        if (!serieValida)
        {
            _view.MostrarError(mensajeSerie);
            return;
        }

        var (exito, mensajeDb) = IncidenciaListadoStore.RegistrarDesdeIncidencia(incidencia);
        if (exito)
        {
            _view.MostrarExito(mensajeDb);
            _view.LimpiarFormulario();
        }
        else
        {
            _view.MostrarError($"No se pudo guardar en la base de datos física, pero se guardó temporalmente en la memoria.\n\nDetalle del error:\n{mensajeDb}");
            _view.LimpiarFormulario();
        }
    }
}
