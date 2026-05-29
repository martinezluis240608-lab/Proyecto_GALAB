namespace Proyecto_GALAB.Views;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null!;

    private Label      lblIniciarTitulo;
    private Panel      panelIzquierdo;
    private PictureBox picLaboratorio;
    private Panel      panelDerecho;
    private PictureBox picLogo;
    private Label      lblGalab;
    // Botones de rol
    private Panel      panelRol;
    private Button     btnRolAlumno;
    private Button     btnRolAdmin;
    private Label      lblRolActivo;
    // Campos
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
        Size            = new Size(960, 600);
        MinimumSize     = new Size(720, 480);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox     = true;
        BackColor       = Color.White;
        Font            = new Font("Segoe UI", 10F);

        // ── Etiqueta superior ────────────────────────────────────
        lblIniciarTitulo = new Label
        {
            Text      = "INICIAR SESION",
            Left = 0, Top = 0, Height = 28,
            Font      = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(100, 100, 100),
            BackColor = Color.White,
            Padding   = new Padding(8, 6, 0, 0),
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── Panel izquierdo ──────────────────────────────────────
        panelIzquierdo = new Panel
        {
            Left = 0, Top = 28,
            BackColor = Color.FromArgb(30, 30, 30),
            Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };

        picLaboratorio = new PictureBox
        {
            Dock     = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor= Color.FromArgb(30, 30, 30)
        };
        try { picLaboratorio.Image = Properties.Resources.ima_L; }
        catch { picLaboratorio.BackColor = Color.FromArgb(50, 50, 50); }
        panelIzquierdo.Controls.Add(picLaboratorio);

        // ── Panel derecho ────────────────────────────────────────
        panelDerecho = new Panel
        {
            Left = 0, Top = 28,
            BackColor = Color.FromArgb(173, 216, 230),
            Anchor    = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right
        };

        // Logo grande
        picLogo = new PictureBox
        {
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };
        try { picLogo.Image = Properties.Resources.logo_GALAB_1; }
        catch { picLogo.BackColor = Color.FromArgb(200, 220, 200); }

        lblGalab = new Label
        {
            Text      = "GALAB",
            Font      = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.FromArgb(25, 25, 80),
            TextAlign = ContentAlignment.MiddleCenter
        };

        // ── Panel selector de rol ────────────────────────────────
        panelRol = new Panel { BackColor = Color.Transparent };

        btnRolAlumno = new Button
        {
            Text      = "Alumno",
            Tag       = "alumno",
            Height    = 34,
            BackColor = Color.FromArgb(25, 25, 80),   // activo por defecto
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnRolAlumno.FlatAppearance.BorderSize = 0;
        btnRolAlumno.Click += BtnRol_Click;

        btnRolAdmin = new Button
        {
            Text      = "Administrador",
            Tag       = "admin",
            Height    = 34,
            BackColor = Color.FromArgb(200, 215, 230),
            ForeColor = Color.FromArgb(40, 40, 40),
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F),
            Cursor    = Cursors.Hand
        };
        btnRolAdmin.FlatAppearance.BorderSize = 0;
        btnRolAdmin.Click += BtnRol_Click;

        lblRolActivo = new Label
        {
            Text      = "Ingresando como: Alumno",
            Font      = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.FromArgb(50, 50, 100),
            AutoSize  = true
        };

        panelRol.Controls.AddRange(new Control[] { btnRolAlumno, btnRolAdmin, lblRolActivo });

        // ── Campos ───────────────────────────────────────────────
        lblUsuario    = MkLabel("USUARIO");
        txtUsuario    = MkTextBox("INGRESE USUARIO", false);
        lblContrasena = MkLabel("CONTRASEÑA");
        txtContrasena = MkTextBox("INGRESE CONTRASEÑA", true);

        btnIniciarSesion = new Button
        {
            Text      = "INICIAR SESION",
            Height    = 42,
            BackColor = Color.FromArgb(20, 20, 20),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor    = Cursors.Hand
        };
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.Click += btnIniciarSesion_Click;

        lblForgot = new LinkLabel
        {
            Text      = "Olvidaste tu contraseña?",
            AutoSize  = true,
            Font      = new Font("Segoe UI", 9F),
            LinkColor = Color.FromArgb(25, 25, 80)
        };
        lblForgot.LinkClicked += lblForgot_LinkClicked;

        panelDerecho.Controls.AddRange(new Control[]
        {
            picLogo, lblGalab, panelRol,
            lblUsuario, txtUsuario,
            lblContrasena, txtContrasena,
            btnIniciarSesion, lblForgot
        });

        Controls.AddRange(new Control[] { lblIniciarTitulo, panelIzquierdo, panelDerecho });

        this.Resize += (s, e) => AjustarLayout();
        this.Load   += (s, e) => AjustarLayout();
    }

    // ── Ajuste responsivo ────────────────────────────────────────
    private void AjustarLayout()
    {
        int w    = ClientSize.Width;
        int h    = ClientSize.Height - 28;
        int mitad= w / 2;

        lblIniciarTitulo.Width = w;

        panelIzquierdo.Width  = mitad;
        panelIzquierdo.Height = h;

        panelDerecho.Left   = mitad;
        panelDerecho.Width  = w - mitad;
        panelDerecho.Height = h;

        int pw  = panelDerecho.Width;
        int pad = Math.Max(20, (int)(pw * 0.10));
        int cw  = pw - pad * 2;

        // Logo grande: 35% del alto del panel
        int logoH = Math.Max(80, (int)(h * 0.30));
        int logoW = logoH;
        picLogo.Left   = (pw - logoW) / 2;
        picLogo.Top    = 18;
        picLogo.Width  = logoW;
        picLogo.Height = logoH;

        int y = picLogo.Top + logoH + 6;

        lblGalab.Left  = 0; lblGalab.Top = y; lblGalab.Width = pw; lblGalab.Height = 28;
        y += 32;

        // Panel rol
        int btnW = cw / 2 - 4;
        panelRol.Left   = pad; panelRol.Top = y;
        panelRol.Width  = cw;  panelRol.Height = 72;

        btnRolAlumno.Left = 0;         btnRolAlumno.Top = 0; btnRolAlumno.Width = btnW;
        btnRolAdmin.Left  = btnW + 8;  btnRolAdmin.Top  = 0; btnRolAdmin.Width  = cw - btnW - 8;
        lblRolActivo.Left = 0;         lblRolActivo.Top = 42;
        y += 80;

        lblUsuario.Left = pad; lblUsuario.Top = y; y += 20;
        txtUsuario.Left = pad; txtUsuario.Top = y; txtUsuario.Width = cw; y += 40;

        lblContrasena.Left = pad; lblContrasena.Top = y; y += 20;
        txtContrasena.Left = pad; txtContrasena.Top = y; txtContrasena.Width = cw; y += 50;

        btnIniciarSesion.Left = pad; btnIniciarSesion.Top = y; btnIniciarSesion.Width = cw; y += 50;
        lblForgot.Left = pad; lblForgot.Top = y;
    }

    // ── Cambio de rol ────────────────────────────────────────────
    private void BtnRol_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        bool esAlumno = btn.Tag?.ToString() == "alumno";

        btnRolAlumno.BackColor = esAlumno
            ? Color.FromArgb(25, 25, 80) : Color.FromArgb(200, 215, 230);
        btnRolAlumno.ForeColor = esAlumno ? Color.White : Color.FromArgb(40,40,40);
        btnRolAlumno.Font      = new Font("Segoe UI", 9F, esAlumno ? FontStyle.Bold : FontStyle.Regular);

        btnRolAdmin.BackColor = !esAlumno
            ? Color.FromArgb(25, 25, 80) : Color.FromArgb(200, 215, 230);
        btnRolAdmin.ForeColor = !esAlumno ? Color.White : Color.FromArgb(40,40,40);
        btnRolAdmin.Font      = new Font("Segoe UI", 9F, !esAlumno ? FontStyle.Bold : FontStyle.Regular);

        lblRolActivo.Text = esAlumno
            ? "Ingresando como: Alumno"
            : "Ingresando como: Administrador";

        // Exponer el rol para que el Presenter lo use
        RolSeleccionado = btn.Tag?.ToString() ?? "alumno";
    }

    private static Label MkLabel(string text) => new Label
    {
        Text      = text,
        AutoSize  = true,
        Font      = new Font("Segoe UI", 8F, FontStyle.Bold),
        ForeColor = Color.FromArgb(50, 50, 50)
    };

    private static TextBox MkTextBox(string ph, bool pwd) => new TextBox
    {
        PlaceholderText = ph,
        Height          = 34,
        PasswordChar    = pwd ? '●' : '\0',
        BorderStyle     = BorderStyle.FixedSingle,
        Font            = new Font("Segoe UI", 10F),
        BackColor       = Color.White
    };
}
