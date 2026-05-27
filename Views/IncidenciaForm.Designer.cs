namespace Proyecto_GALAB.Views;

partial class IncidenciaForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel     panelHeader;
    private Label     lblHeaderTitle;
    private Label     lblTitulo;
    private Panel     panelBody;
    private Label     lblQuienReporta;
    private TextBox   txtQuienReporta;
    private Label     lblTipo;
    private ComboBox  cmbTipo;
    private Label     lblNombreEquipo;
    private TextBox   txtNombreEquipo;
    private Label     lblFechaHora;
    private DateTimePicker dtpFecha;
    private NumericUpDown  nudHora;
    private Label     lblDos;
    private NumericUpDown  nudMinuto;
    private Label     lblDescripcion;
    private TextBox   txtDescripcion;
    private Label     lblEvidencias;
    private Button    btnAdjuntar;
    private Label     lblEvidencia;
    private Button    btnEnviar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // ── Form ────────────────────────────────────────────────
        Text            = "REGISTRO DE NUEVA INCIDENCIA";
        Size            = new Size(900, 620);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = true;
        BackColor       = Color.FromArgb(173, 216, 230);
        Font            = new Font("Segoe UI", 10F);

        // ── Header ──────────────────────────────────────────────
        panelHeader = new Panel
        {
            Left      = 20, Top = 10,
            Width     = 840, Height = 60,
            BackColor = Color.FromArgb(240, 235, 245)
        };
        panelHeader.Paint += (s, e) =>
        {
            int r = 12;
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

        // ── Título sección ──────────────────────────────────────
        lblTitulo = new Label
        {
            Text      = "REGISTRO DE NUEVA INCIDENCIA",
            Left      = 20, Top = 80,
            Width     = 840,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 50, 50),
            TextAlign = ContentAlignment.MiddleCenter
        };

        // ── Quién reporta ───────────────────────────────────────
        lblQuienReporta = MkLabel("QUIEN REPORTA", 20, 110);
        txtQuienReporta = MkTextBox("Ingrese nombre", 20, 132, 380);

        // ── Tipo de incidencia ──────────────────────────────────
        lblTipo = MkLabel("TIPO DE INCIDENCIA", 20, 175);
        cmbTipo = new ComboBox
        {
            Left          = 20, Top = 197,
            Width         = 380, Height = 36,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font          = new Font("Segoe UI", 10F),
            BackColor     = Color.White
        };
        cmbTipo.Items.AddRange(new object[]
        {
            "INFRAESTRUCTURA", "SOFTWARE", "HARDWARE", "RED", "OTRO"
        });
        cmbTipo.SelectedIndex = 0;

        // ── Nombre del equipo ───────────────────────────────────
        lblNombreEquipo = MkLabel("NOMBRE DEL EQUIPO", 20, 242);
        txtNombreEquipo = MkTextBox("Ingrese el nombre del equipo", 20, 264, 380);

        // ── Fecha y hora ────────────────────────────────────────
        lblFechaHora = MkLabel("FECHA Y HORA", 20, 309);

        dtpFecha = new DateTimePicker
        {
            Left   = 20, Top = 331,
            Width  = 200, Height = 36,
            Format = DateTimePickerFormat.Short,
            Font   = new Font("Segoe UI", 10F)
        };

        nudHora = new NumericUpDown
        {
            Left     = 232, Top = 331,
            Width    = 65, Height = 36,
            Minimum  = 0, Maximum = 23,
            Value    = DateTime.Now.Hour,
            Font     = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center
        };

        lblDos = new Label
        {
            Text      = ":",
            Left      = 300, Top = 331,
            Width     = 18, Height = 36,
            Font      = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(30, 30, 30)
        };

        nudMinuto = new NumericUpDown
        {
            Left      = 320, Top = 331,
            Width     = 65, Height = 36,
            Minimum   = 0, Maximum = 59,
            Value     = 0,
            Font      = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center
        };

        // ── Descripción ─────────────────────────────────────────
        lblDescripcion = MkLabel("DESCRIPCION", 20, 380);
        txtDescripcion = new TextBox
        {
            Left        = 20, Top = 402,
            Width       = 840, Height = 80,
            Multiline   = true,
            ScrollBars  = ScrollBars.Vertical,
            PlaceholderText = "Ingrese la descripción de la incidencia",
            BorderStyle = BorderStyle.FixedSingle,
            Font        = new Font("Segoe UI", 10F),
            BackColor   = Color.White
        };

        // ── Evidencias ──────────────────────────────────────────
        lblEvidencias = MkLabel("EVIDENCIAS", 20, 495);

        btnAdjuntar = new Button
        {
            Text      = "📎 ADJUNTAR",
            Left      = 20, Top = 517,
            Width     = 140, Height = 38,
            BackColor = Color.FromArgb(200, 40, 40),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnAdjuntar.FlatAppearance.BorderSize = 0;
        btnAdjuntar.Click += btnAdjuntar_Click;

        lblEvidencia = new Label
        {
            Left      = 170, Top = 524,
            Width     = 500, Height = 24,
            ForeColor = Color.FromArgb(60, 60, 60),
            Font      = new Font("Segoe UI", 9F),
            Text      = ""
        };

        // ── Botón Enviar ────────────────────────────────────────
        btnEnviar = new Button
        {
            Text      = "ENVIAR REPORTE",
            Left      = 320, Top = 517,
            Width     = 200, Height = 38,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnEnviar.FlatAppearance.BorderSize = 0;
        btnEnviar.Click += btnEnviar_Click;

        // ── Agregar controles ───────────────────────────────────
        Controls.AddRange(new Control[]
        {
            panelHeader, lblTitulo,
            lblQuienReporta, txtQuienReporta,
            lblTipo, cmbTipo,
            lblNombreEquipo, txtNombreEquipo,
            lblFechaHora, dtpFecha, nudHora, lblDos, nudMinuto,
            lblDescripcion, txtDescripcion,
            lblEvidencias, btnAdjuntar, lblEvidencia, btnEnviar
        });
    }

    // ── Helpers ─────────────────────────────────────────────────
    private static Label MkLabel(string text, int x, int y) => new Label
    {
        Text      = text,
        Left      = x, Top = y,
        AutoSize  = true,
        Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
        ForeColor = Color.FromArgb(50, 50, 50)
    };

    private static TextBox MkTextBox(string placeholder, int x, int y, int w) => new TextBox
    {
        Left            = x, Top = y,
        Width           = w, Height = 36,
        PlaceholderText = placeholder,
        BorderStyle     = BorderStyle.FixedSingle,
        Font            = new Font("Segoe UI", 10F),
        BackColor       = Color.White
    };
}
