using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views;

public partial class IncidenciaForm : Form, IIncidenciaView
{
    private readonly IncidenciaPresenter _presenter;

    public string   Titulo         => txtQuienReporta.Text;
    public string   QuienReporta
    {
        get
        {
            if (Services.SesionActual.EsAdministrador)
            {
                return Services.PerfilAdministradorStore.Obtener().NombreCompleto;
            }
            return Services.PerfilUsuarioStore.Obtener().NombreCompleto;
        }
    }
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
    public string NumeroSerie    => txtEtiquetas.Text;

    public event EventHandler? OnEnviarReporte;
    public event EventHandler? OnAdjuntar;

    public void MostrarExito(string mensaje) =>
        NotificacionExitoForm.Mostrar(this, "¡Éxito!", mensaje);

    public void MostrarError(string mensaje) =>
        NotificacionForm.Mostrar(this, "Atención", mensaje, "Verifique los datos");

    public void MostrarMensaje(string mensaje) =>
        NotificacionExitoForm.Mostrar(this, "Información", mensaje);

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
        ConfigurarAutocompletado();
    }

    private void ConfigurarAutocompletado()
    {
        txtQuienReporta.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtQuienReporta.AutoCompleteSource = AutoCompleteSource.CustomSource;

        txtNombreEquipo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtNombreEquipo.AutoCompleteSource = AutoCompleteSource.CustomSource;

        var sourceTitulos = new AutoCompleteStringCollection();
        var sourceEquipos = new AutoCompleteStringCollection();

        // Sugerencias masivas por defecto (Catálogo de palabras de TI e incidencias)
        sourceTitulos.AddRange(new[] { 
            "Actualización requerida", "Aire acondicionado averiado", "Antivirus caducado", "Apagones constantes", 
            "Aplicación no responde", "Audio no se escucha", "Bloqueo de cuenta", "Cable HDMI dañado", 
            "Cable de red desconectado", "Cable VGA defectuoso", "Cámara no funciona", "Conector de corriente roto", 
            "Contraseña olvidada", "Computadora lenta", "Computadora no enciende", "Computadora se congela", 
            "Cortocircuito en contacto", "CPU sobrecalentado", "Disco duro lleno", "Error de base de datos", 
            "Error de red", "Error de sistema operativo", "Error de software", "Falla en proyector", 
            "Falta cable de video", "Falta extensión eléctrica", "Falta limpieza de equipo", "Falta mouse", 
            "Falta teclado", "Impresora atascada", "Impresora sin tinta/tóner", "Lámpara fundida", 
            "Licencia expirada", "Mantenimiento preventivo", "Micrófono no sirve", "Monitor apagado", 
            "Monitor con rayas", "Monitor no da video", "Mouse no funciona", "No hay acceso a internet", 
            "No hay conexión WiFi", "No hay red LAN", "Pantalla azul (BSOD)", "Pantalla rota", 
            "Pérdida de datos", "Proyector con imagen borrosa", "Proyector no enciende", "Proyector sin señal", 
            "Puerto USB dañado", "Puerto de red dañado", "Recuperación de archivo", "Ruido extraño en CPU", 
            "Silla rota", "Sistema inestable", "Teclado desconfigurado", "Teclado roto", "Teclas faltantes", 
            "Ventilador ruidoso", "Virus/Malware detectado"
        });

        sourceEquipos.AddRange(new[] { 
            "Aire Acondicionado Sala A", "Aire Acondicionado Sala B", "Bocinas", "Cableado Estructurado", 
            "Cámara de Seguridad", "Computadora de Docente", "Contacto Eléctrico", "Impresora Principal", 
            "Mesa de Trabajo 1", "Mesa de Trabajo 2", "Mesa de Trabajo 3", "Mesa de Trabajo 4", "Mesa de Trabajo 5", 
            "Monitor", "Mouse", "PC-LAB1-01", "PC-LAB1-02", "PC-LAB1-03", "PC-LAB1-04", "PC-LAB1-05", 
            "PC-LAB1-06", "PC-LAB1-07", "PC-LAB1-08", "PC-LAB1-09", "PC-LAB1-10", "PC-LAB2-01", "PC-LAB2-02", 
            "PC-LAB2-03", "PC-LAB2-04", "PC-LAB2-05", "Proyector Aula 1", "Proyector Aula 2", "Proyector Aula 3", 
            "Rack Principal", "Regulador de Voltaje", "Router Principal", "Servidor Local", "Switch Laboratorio", 
            "Teclado", "Teléfono IP"
        });

        try
        {
            var incidencias = Services.IncidenciaListadoStore.ObtenerTodas();
            foreach (var inc in incidencias)
            {
                if (!string.IsNullOrWhiteSpace(inc.Titulo) && !sourceTitulos.Contains(inc.Titulo))
                    sourceTitulos.Add(inc.Titulo);
                
                if (!string.IsNullOrWhiteSpace(inc.Equipo) && !sourceEquipos.Contains(inc.Equipo))
                    sourceEquipos.Add(inc.Equipo);
            }
        }
        catch { /* Ignorar si no hay conexión */ }

        txtQuienReporta.AutoCompleteCustomSource = sourceTitulos;
        txtNombreEquipo.AutoCompleteCustomSource = sourceEquipos;
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
