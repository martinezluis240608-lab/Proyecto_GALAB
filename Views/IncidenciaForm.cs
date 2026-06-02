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
        UiAssets.PrepararPantallaCompleta(this);
        AgregarBarraLateral();
        _presenter = new IncidenciaPresenter(this);
    }

    private void btnEnviar_Click(object sender, EventArgs e)   => OnEnviarReporte?.Invoke(this, EventArgs.Empty);
    private void btnAdjuntar_Click(object sender, EventArgs e) => OnAdjuntar?.Invoke(this, EventArgs.Empty);

    private void AgregarBarraLateral()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 290,
            BackColor = Color.White
        };

        int y = 56;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("⌂", "Inicio", y, false, () => UiAssets.AbrirCerrandoActual(this, new PrincipalForm())));
        y += 72;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias   ›", y, true, () => UiAssets.NavegarAGestionIncidencias(this)));
        y += 72;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, false, () => UiAssets.AbrirCerrandoActual(this, new PerfilForm())));
        y += 72;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 72;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesión",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 48),
            Location = new Point(40, 575),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        cerrar.FlatAppearance.BorderColor = UiAssets.Borde;
        cerrar.FlatAppearance.BorderSize = 1;
        cerrar.Click += (s, e) => UiAssets.CerrarSesion(this);
        sidebar.Resize += (s, e) => cerrar.Top = sidebar.Height - 78;
        sidebar.Controls.Add(cerrar);

        Controls.Add(sidebar);
        sidebar.BringToFront();
    }
}
