namespace Proyecto_GALAB.Views;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel panelIzquierdo;
    private PictureBox picLaboratorio;
    private Label lblIniciarTitulo;
    private Panel panelDerecho;
    private PictureBox picLogo;
    private Label lblGalab;
    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblContrasena;
    private TextBox txtContrasena;
    private Button btnIniciarSesion;
    private LinkLabel lblForgot;

    // Nuevos controles para selección de tipo de usuario
    private Label lblTipoUsuario;
    private Button btnAlumno;
    private Button btnAdmin;
    private FlowLayoutPanel flowTipoUsuarioPanel;
    private Label lblTipoSeleccionado;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text = "GALAB - Iniciar Sesión";
        Size = new Size(1000, 620);
        MinimumSize = new Size(800, 500);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);

        lblIniciarTitulo = new Label
        {
            Text = "INICIAR SESION",
            Left = 0,
            Top = 0,
            Height = 35,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(80, 80, 80),
            BackColor = Color.White,
            Padding = new Padding(15, 8, 0, 0),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        panelIzquierdo = new Panel
        {
            Left = 0,
            Top = 35,
            BackColor = Color.FromArgb(40, 40, 40),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };

        picLaboratorio = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(40, 40, 40)
        };
        try
        {
            picLaboratorio.Image = Image.FromFile(
                @"C:\Users\LENOVO\Downloads\657dd35e-79a6-4145-9891-d15fea1a313c.png");
        }
        catch
        {
            picLaboratorio.BackColor = Color.FromArgb(200, 220, 200);
        }
        panelIzquierdo.Controls.Add(picLaboratorio);

        panelDerecho = new Panel
        {
            Left = 0,
            Top = 35,
            BackColor = Color.FromArgb(173, 216, 230),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        // Logo más grande
        picLogo = new PictureBox
        {
            SizeMode = PictureBoxSizeMode.Zoom,

            Anchor = AnchorStyles.Top | AnchorStyles.None

        };
        try
        {
            picLogo.Image = Image.FromFile(@"C:\Users\LENOVO\Documents\dibujos\logo GALAB_1.png");
        }
        catch
        {
            picLogo.BackColor = Color.Transparent;
        }

        // TEXTO GALAB MÁS GRANDE
        lblGalab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),  // Aumentado de 16 a 28
            ForeColor = Color.FromArgb(30, 30, 80),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── PANEL PARA TIPO DE USUARIO (NUEVO) ───────────────────
        flowTipoUsuarioPanel = new FlowLayoutPanel
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            Height = 50,
            BackColor = Color.Transparent,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        lblTipoUsuario = new Label
        {
            Text = "Tipo de usuario:",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 50, 50),
            Size = new Size(100, 32),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Botón Alumno
        btnAlumno = new Button
        {
            Text = "👨‍🎓 ALUMNO",
            Size = new Size(130, 34),
            BackColor = Color.FromArgb(70, 130, 200),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Tag = "alumno"
        };
        btnAlumno.FlatAppearance.BorderSize = 0;
        btnAlumno.Click += BtnTipoUsuario_Click;

        // Botón Administrador
        btnAdmin = new Button
        {
            Text = "👨‍💼 ADMINISTRADOR",
            Size = new Size(150, 34),
            BackColor = Color.FromArgb(100, 100, 100),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Tag = "admin"
        };
        btnAdmin.FlatAppearance.BorderSize = 0;
        btnAdmin.Click += BtnTipoUsuario_Click;

        flowTipoUsuarioPanel.Controls.Add(lblTipoUsuario);
        flowTipoUsuarioPanel.Controls.Add(btnAlumno);
        flowTipoUsuarioPanel.Controls.Add(btnAdmin);

        // Etiqueta para mostrar tipo seleccionado
        lblTipoSeleccionado = new Label
        {
            Text = "▶ No seleccionado",
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            ForeColor = Color.FromArgb(100, 100, 100),
            Height = 25,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(5, 0, 0, 0),
            BackColor = Color.FromArgb(240, 248, 255),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── CAMPOS EXISTENTES ───────────────────────────────────
        lblUsuario = MkLabel("USUARIO");
        txtUsuario = MkTextBox("INGRESE USUARIO", false);
        lblContrasena = MkLabel("CONTRASEÑA");
        txtContrasena = MkTextBox("INGRESE CONTRASEÑA", true);

        btnIniciarSesion = new Button
        {
            Text = "INICIAR SESION",
            Height = 45,
            BackColor = Color.FromArgb(20, 20, 20),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.Click += btnIniciarSesion_Click;

        lblForgot = new LinkLabel
        {
            Text = "Olvidaste tu contraseña?",
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            LinkColor = Color.FromArgb(30, 30, 80),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        lblForgot.LinkClicked += lblForgot_LinkClicked;

        // Agregar todos los controles al panel derecho (en orden)
        panelDerecho.Controls.AddRange(new Control[]
        {
            picLogo, lblGalab,
            flowTipoUsuarioPanel, lblTipoSeleccionado,
            lblUsuario, txtUsuario,
            lblContrasena, txtContrasena,
            btnIniciarSesion, lblForgot
        });

        Controls.AddRange(new Control[] { lblIniciarTitulo, panelIzquierdo, panelDerecho });

        // Eventos de redimensionamiento
        this.Resize += (s, e) => AjustarLayout();
        this.Load += (s, e) => AjustarLayout();
    }

    private void AjustarLayout()
    {
        int w = ClientSize.Width;
        int h = ClientSize.Height - 35;
        int mitad = w / 2;

        lblIniciarTitulo.Width = w;
        lblIniciarTitulo.Height = 35;

        panelIzquierdo.Left = 0;
        panelIzquierdo.Width = mitad;
        panelIzquierdo.Height = h;
        panelIzquierdo.Top = 35;

        panelDerecho.Left = mitad;
        panelDerecho.Width = w - mitad;
        panelDerecho.Height = h;
        panelDerecho.Top = 35;

        // Posicionar controles dentro del panel derecho
        int pw = panelDerecho.Width;
        int pad = (int)(pw * 0.12);
        int cw = pw - pad * 2;

        // Logo (más grande)
        int logoSize = 200;
        picLogo.Left = (pw - logoSize) / 2;
        picLogo.Top = 25;
        picLogo.Width = logoSize;
        picLogo.Height = logoSize;

        // Texto GALAB más grande
        lblGalab.Left = 0;
        lblGalab.Top = 140;
        lblGalab.Width = pw;
        lblGalab.Height = 50;
        lblGalab.Font = new Font("Segoe UI", 28F, FontStyle.Bold);

        // Panel de tipo de usuario
        flowTipoUsuarioPanel.Left = pad;
        flowTipoUsuarioPanel.Top = 200;
        flowTipoUsuarioPanel.Width = cw;

        // Ajustar ancho de los botones según el espacio disponible
        int espacioDisponible = cw - lblTipoUsuario.Width - 20;
        if (espacioDisponible >= 290)
        {
            btnAlumno.Width = 140;
            btnAdmin.Width = 160;
        }
        else if (espacioDisponible >= 240)
        {
            btnAlumno.Width = 120;
            btnAdmin.Width = 130;
        }
        else
        {
            btnAlumno.Width = 100;
            btnAdmin.Width = 110;
        }

        // Etiqueta de tipo seleccionado
        lblTipoSeleccionado.Left = pad;
        lblTipoSeleccionado.Top = 255;
        lblTipoSeleccionado.Width = cw;

        // Campos de usuario y contraseña
        lblUsuario.Left = pad;
        lblUsuario.Top = 295;

        txtUsuario.Left = pad;
        txtUsuario.Top = 315;
        txtUsuario.Width = cw;
        txtUsuario.Height = 38;

        lblContrasena.Left = pad;
        lblContrasena.Top = 370;

        txtContrasena.Left = pad;
        txtContrasena.Top = 390;
        txtContrasena.Width = cw;
        txtContrasena.Height = 38;

        btnIniciarSesion.Left = pad;
        btnIniciarSesion.Top = 455;
        btnIniciarSesion.Width = cw;
        btnIniciarSesion.Height = 45;

        lblForgot.Left = pad;
        lblForgot.Top = 515;
    }

    // Manejador para selección de tipo de usuario
    private void BtnTipoUsuario_Click(object? sender, EventArgs e)
    {
        Button btn = (Button)sender!;
        string tipoUsuario = btn.Tag?.ToString() ?? "";

        // Resetear colores
        btnAlumno.BackColor = Color.FromArgb(100, 100, 100);
        btnAdmin.BackColor = Color.FromArgb(100, 100, 100);

        if (tipoUsuario == "alumno")
        {
            btnAlumno.BackColor = Color.FromArgb(70, 130, 200);
            lblTipoSeleccionado.Text = "▶ Modo ALUMNO - Acceso a reportes personales";
            lblTipoSeleccionado.ForeColor = Color.FromArgb(70, 130, 200);
            txtUsuario.PlaceholderText = "Ej: 202410001 (código de estudiante)";
        }
        else if (tipoUsuario == "admin")
        {
            btnAdmin.BackColor = Color.FromArgb(60, 180, 100);
            lblTipoSeleccionado.Text = "▶ Modo ADMINISTRADOR - Acceso total al sistema";
            lblTipoSeleccionado.ForeColor = Color.FromArgb(60, 180, 100);
            txtUsuario.PlaceholderText = "Ej: admin";
        }

        txtUsuario.Focus();
    }

    private static Label MkLabel(string text) => new Label
    {
        Text = text,
        AutoSize = true,
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = Color.FromArgb(60, 60, 60)
    };

    private static TextBox MkTextBox(string placeholder, bool password) => new TextBox
    {
        PlaceholderText = placeholder,
        Height = 36,
        PasswordChar = password ? '●' : '\0',
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font("Segoe UI", 10F),
        BackColor = Color.White
    };
}