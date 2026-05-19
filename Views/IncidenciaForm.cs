using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views;

public partial class IncidenciaForm : Form, IIncidenciaView
{
    private readonly IncidenciaPresenter _presenter;

    public string   QuienReporta   => txtQuienReporta.Text;
    public string   TipoIncidencia => cmbTipo.SelectedItem?.ToString() ?? "";
    public string   NombreEquipo   => txtNombreEquipo.Text;
    public DateTime FechaHora
    {
        get
        {
            var fecha = dtpFecha.Value.Date;
            return fecha.AddHours((double)nudHora.Value)
                        .AddMinutes((double)nudMinuto.Value);
        }
    }
    public string Descripcion    => txtDescripcion.Text;
    public string RutaEvidencia
    {
        get => lblEvidencia.Text;
        set => lblEvidencia.Text = value;
    }

    public event EventHandler? OnEnviarReporte;
    public event EventHandler? OnAdjuntar;

    public void MostrarMensaje(string mensaje) =>
        MessageBox.Show(mensaje, "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public void LimpiarFormulario()
    {
        txtQuienReporta.Clear();
        cmbTipo.SelectedIndex = 0;
        txtNombreEquipo.Clear();
        dtpFecha.Value = DateTime.Now;
        nudHora.Value   = DateTime.Now.Hour;
        nudMinuto.Value = 0;
        txtDescripcion.Clear();
        lblEvidencia.Text = "";
    }

    public IncidenciaForm()
    {
        InitializeComponent();
        _presenter = new IncidenciaPresenter(this);
    }

    private void btnEnviar_Click(object sender, EventArgs e)   => OnEnviarReporte?.Invoke(this, EventArgs.Empty);
    private void btnAdjuntar_Click(object sender, EventArgs e) => OnAdjuntar?.Invoke(this, EventArgs.Empty);
}
