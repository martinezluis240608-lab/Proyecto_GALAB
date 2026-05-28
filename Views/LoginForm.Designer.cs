namespace Proyecto_GALAB.Views;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel      panelIzquierdo;
    private PictureBox picLaboratorio;
    private Label      lblIniciarTitulo;
    private Panel      panelDerecho;
    private PictureBox picLogo;
    private Label      lblGalab;
    private Label      lblUsuario;
    private TextBox    txtUsuario;
    private Label      lblContrasena;
    private TextBox    txtContrasena;
    private Button     btnIniciarSesion;
    private LinkLabel  lblForgot;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "GALAB - Iniciar Sesión";
        Size            = new Size(900, 560);
        MinimumSize     = new Size(700, 440);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox     = true;
        BackColor       = Color.White;
        Font            = new Font("Segoe UI", 10F);

        lblIniciarTitulo = new Label
        {
            Text      = "INICIAR SESION",
            Left      = 0, Top = 0,
            Height    = 30,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80),
            BackColor = Color.White,
            Padding   = new Padding(10, 6, 0, 0),
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        panelIzquierdo = new Panel
        {
            Left      = 0, Top = 30,
            BackColor = Color.FromArgb(40, 40, 40),
            Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };

        picLaboratorio = new PictureBox
        {
            Dock      = DockStyle.Fill,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(40, 40, 40)
        };
        picLaboratorio.Paint += (s, e) =>
        {
            e.Graphics.Clear(Color.FromArgb(50, 50, 50));
            using var f  = new Font("Segoe UI", 11F);
            var msg = "[ Foto del Laboratorio ]";
            var sz  = e.Graphics.MeasureString(msg, f);
            e.Graphics.DrawString(msg, f, Brushes.Gray,
                (picLaboratorio.Width  - sz.Width)  / 2,
                (picLaboratorio.Height - sz.Height) / 2);
        };
        panelIzquierdo.Controls.Add(picLaboratorio);

        panelDerecho = new Panel
        {
            Left      = 0, Top = 30,
            BackColor = Color.FromArgb(173, 216, 230),
            Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right
        };

        picLogo = new PictureBox
        {
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor    = AnchorStyles.Top | AnchorStyles.None
        };
        picLogo.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(200, 220, 200)), 0, 0, picLogo.Width-1, picLogo.Height-1);
            e.Graphics.DrawEllipse(new Pen(Color.FromArgb(100,140,100), 2), 0, 0, picLogo.Width-1, picLogo.Height-1);
            using var f = new Font("Segoe UI", 7F);
            e.Graphics.DrawString("LOGO", f, Brushes.DarkGreen, 28, 38);
        };

        lblGalab = new Label
        {
            Text      = "GALAB",
            Font      = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        lblUsuario    = MkLabel("USUARIO");
        txtUsuario    = MkTextBox("INGRESE USUARIO", false);
        lblContrasena = MkLabel("CONTRASEÑA");
        txtContrasena = MkTextBox("INGRESE CONTRASEÑA", true);

        btnIniciarSesion = new Button
        {
            Text      = "INICIAR SESION",
            Height    = 40,
            BackColor = Color.FromArgb(20, 20, 20),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor    = Cursors.Hand,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.Click += btnIniciarSesion_Click;

        lblForgot = new LinkLabel
        {
            Text      = "Olvidaste tu contraseña?",
            AutoSize  = true,
            Font      = new Font("Segoe UI", 9F),
            LinkColor = Color.FromArgb(30, 30, 80),
            Anchor    = AnchorStyles.Top | AnchorStyles.Left
        };
        lblForgot.LinkClicked += lblForgot_LinkClicked;

        panelDerecho.Controls.AddRange(new Control[]
        {
            picLogo, lblGalab,
            lblUsuario, txtUsuario,
            lblContrasena, txtContrasena,
            btnIniciarSesion, lblForgot
        });

        Controls.AddRange(new Control[] { lblIniciarTitulo, panelIzquierdo, panelDerecho });

        // Ajuste dinámico al redimensionar
        this.Resize += (s, e) => AjustarLayout();
        this.Load   += (s, e) => AjustarLayout();
    }

    private void AjustarLayout()
    {
        int w = ClientSize.Width;
        int h = ClientSize.Height - 30;
        int mitad = w / 2;

        lblIniciarTitulo.Width = w;

        panelIzquierdo.Left   = 0;
        panelIzquierdo.Width  = mitad;
        panelIzquierdo.Height = h;

        panelDerecho.Left   = mitad;
        panelDerecho.Width  = w - mitad;
        panelDerecho.Height = h;

        // Posicionar controles dentro del panel derecho
        int pw = panelDerecho.Width;
        int pad = (int)(pw * 0.13);
        int cw  = pw - pad * 2;

        picLogo.Left   = (pw - 90) / 2; picLogo.Top = 30;
        picLogo.Width  = 90; picLogo.Height = 90;

        lblGalab.Left  = 0; lblGalab.Top = 128; lblGalab.Width = pw;

        lblUsuario.Left    = pad; lblUsuario.Top    = 185;
        txtUsuario.Left    = pad; txtUsuario.Top    = 205; txtUsuario.Width = cw;

        lblContrasena.Left = pad; lblContrasena.Top = 248;
        txtContrasena.Left = pad; txtContrasena.Top = 268; txtContrasena.Width = cw;

        btnIniciarSesion.Left  = pad; btnIniciarSesion.Top = 320; btnIniciarSesion.Width = cw;
        lblForgot.Left         = pad; lblForgot.Top        = 372;
    }

    private static Label MkLabel(string text) => new Label
    {
        Text      = text,
        AutoSize  = true,
        Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
        ForeColor = Color.FromArgb(60, 60, 60)
    };

    private static TextBox MkTextBox(string placeholder, bool password) => new TextBox
    {
        PlaceholderText = placeholder,
        Height          = 32,
        PasswordChar    = password ? '●' : '\0',
        BorderStyle     = BorderStyle.FixedSingle,
        Font            = new Font("Segoe UI", 10F),
        BackColor       = Color.White
    };
}
