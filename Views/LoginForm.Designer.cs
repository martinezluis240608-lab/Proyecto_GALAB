namespace Proyecto_GALAB.Views;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel      panelFondo;
    private Panel      panelCard;
    private Label      lblGalab;
    private Label      lblUsuario;
    private TextBox    txtUsuario;
    private Label      lblContrasena;
    private TextBox    txtContrasena;
    private Button     btnIniciarSesion;
    private LinkLabel  lblForgot;
    private PictureBox picMonitor;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // ── Form ────────────────────────────────────────────────
        Text            = "GALAB - Iniciar Sesión";
        Size            = new Size(900, 560);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;
        BackColor       = Color.FromArgb(173, 216, 230);
        Font            = new Font("Segoe UI", 10F);

        // ── Fondo completo ──────────────────────────────────────
        panelFondo = new Panel
        {
            Dock      = DockStyle.Fill,
            BackColor = Color.FromArgb(173, 216, 230)
        };

        // ── Imagen monitor izquierda ────────────────────────────
        picMonitor = new PictureBox
        {
            Left      = 40,
            Top       = 120,
            Width     = 320,
            Height    = 260,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };

        // ── Card login derecha ──────────────────────────────────
        panelCard = new Panel
        {
            Left      = 460,
            Top       = 80,
            Width     = 360,
            Height    = 370,
            BackColor = Color.White
        };
        panelCard.Paint += (s, e) =>
        {
            var r = new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), r);
        };

        // Título GALAB en la card
        lblGalab = new Label
        {
            Text      = "GALAB",
            Font      = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            Left      = 0, Top = 20,
            Width     = 360,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Usuario
        lblUsuario = new Label
        {
            Text      = "USUARIO",
            Left      = 30, Top = 80,
            AutoSize  = true,
            Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80)
        };

        txtUsuario = new TextBox
        {
            Left         = 30, Top = 100,
            Width        = 300, Height = 36,
            PlaceholderText = "INGRESE USUARIO",
            BorderStyle  = BorderStyle.FixedSingle,
            Font         = new Font("Segoe UI", 10F)
        };

        // Contraseña
        lblContrasena = new Label
        {
            Text      = "CONTRASEÑA",
            Left      = 30, Top = 150,
            AutoSize  = true,
            Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80)
        };

        txtContrasena = new TextBox
        {
            Left            = 30, Top = 170,
            Width           = 300, Height = 36,
            PlaceholderText = "INGRESE CONTRASEÑA",
            PasswordChar    = '●',
            BorderStyle     = BorderStyle.FixedSingle,
            Font            = new Font("Segoe UI", 10F)
        };

        // Botón iniciar sesión
        btnIniciarSesion = new Button
        {
            Text      = "INICIAR SESION",
            Left      = 30, Top = 230,
            Width     = 300, Height = 42,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.Click += btnIniciarSesion_Click;

        // Forgot password
        lblForgot = new LinkLabel
        {
            Text      = "Forgot password?",
            Left      = 30, Top = 295,
            AutoSize  = true,
            Font      = new Font("Segoe UI", 9F),
            LinkColor = Color.FromArgb(30, 30, 80)
        };
        lblForgot.LinkClicked += lblForgot_LinkClicked;

        panelCard.Controls.AddRange(new Control[]
        {
            lblGalab, lblUsuario, txtUsuario,
            lblContrasena, txtContrasena,
            btnIniciarSesion, lblForgot
        });

        panelFondo.Controls.AddRange(new Control[] { picMonitor, panelCard });
        Controls.Add(panelFondo);
    }
}
