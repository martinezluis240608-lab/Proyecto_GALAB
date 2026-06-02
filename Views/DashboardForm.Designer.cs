namespace Proyecto_GALAB.Views;

partial class DashboardForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel  panelHeader;
    private Label  lblHeaderTitle;
    private PictureBox picLogoInstituto;
    private Label  lblTituloSeccion;
    private Panel  panelGrafica;
    private Panel  panelNotificaciones;
    private Button btnRegistrar;
    private Button btnVerLista;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // ── Form ────────────────────────────────────────────────
        Text            = "GALAB - Dashboard";
        Size            = new Size(1280, 760);
        MinimumSize     = new Size(1050, 680);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox     = true;
        BackColor       = Color.FromArgb(173, 216, 230);
        Font            = new Font("Segoe UI", 10F);

        // ── Header ──────────────────────────────────────────────
        panelHeader = new Panel
        {
            Left      = 20, Top = 10,
            Width     = 1180, Height = 76,
            BackColor = Color.FromArgb(240, 235, 245)
        };
        panelHeader.Paint += (s, e) =>
        {
            int r = 30;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(panelHeader.Width - r, 0, r, r, 270, 90);
            path.AddArc(panelHeader.Width - r, panelHeader.Height - r, r, r, 0, 90);
            path.AddArc(0, panelHeader.Height - r, r, r, 90, 90);
            path.CloseAllFigures();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(240, 235, 245)), path);
        };

        lblHeaderTitle = new Label
        {
            Text      = "GALAB",
            Font      = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelHeader.Controls.Add(lblHeaderTitle);
        picLogoInstituto = new PictureBox
        {
            Image = UiAssets.CargarLogoInstitucion(),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Dock = DockStyle.Right,
            Width = 72
        };
        panelHeader.Controls.Add(picLogoInstituto);

        // ── Título sección ───────────────────────────────────────
        lblTituloSeccion = new Label
        {
            Text      = "INCIDENCIAS REPORTADAS",
            Left      = 20, Top = 98,
            Width     = 1180,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 50, 50),
            TextAlign = ContentAlignment.MiddleCenter
        };

        // ── Panel gráfica ────────────────────────────────────────
        panelGrafica = new Panel
        {
            Left      = 20, Top = 128,
            Width     = 760, Height = 420,
            BackColor = Color.White
        };
        panelGrafica.Paint += PanelGrafica_Paint;

        // ── Panel notificaciones ─────────────────────────────────
        panelNotificaciones = new Panel
        {
            Left        = 805, Top = 128,
            Width       = 380, Height = 420,
            BackColor   = Color.Transparent,
            AutoScroll  = true
        };

        // ── Botón Registrar ──────────────────────────────────────
        btnRegistrar = new Button
        {
            Text      = "REGISTRAR NUEVA INCIDENCIA",
            Left      = 20, Top = 570,
            Width     = 260, Height = 40,
            BackColor = Color.FromArgb(37, 99, 235),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnRegistrar.FlatAppearance.BorderSize = 0;
        btnRegistrar.Click += btnRegistrar_Click;

        // ── Botón Ver lista ──────────────────────────────────────
        btnVerLista = new Button
        {
            Text      = "VER LISTA COMPLETA",
            Left      = 300, Top = 570,
            Width     = 200, Height = 40,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnVerLista.FlatAppearance.BorderSize = 0;
        btnVerLista.Click += btnVerLista_Click;

        // ── Agregar controles ────────────────────────────────────
        Controls.AddRange(new Control[]
        {
            panelHeader, lblTituloSeccion,
            panelGrafica, panelNotificaciones,
            btnRegistrar, btnVerLista
        });

        Resize += (s, e) =>
        {
            int ancho = ClientSize.Width - 40;
            panelHeader.Width = ancho;
            lblTituloSeccion.Width = ancho;
            panelGrafica.Width = Math.Max(520, (int)(ancho * 0.66));
            panelGrafica.Height = Math.Max(320, ClientSize.Height - 220);
            panelNotificaciones.Left = panelGrafica.Right + 24;
            panelNotificaciones.Width = Math.Max(260, ancho - panelGrafica.Width - 24);
            panelNotificaciones.Height = panelGrafica.Height;
            btnRegistrar.Top = panelGrafica.Bottom + 22;
            btnVerLista.Top = btnRegistrar.Top;
        };
    }
}
