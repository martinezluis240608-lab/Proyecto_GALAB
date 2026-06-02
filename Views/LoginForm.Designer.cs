using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel panelImagen;
    private Panel panelLogin;
    private Panel panelTopAzul;
    private Panel panelFooter;
    private PictureBox picLogo;
    private Label lblGalab;
    private Label lblSubtitulo;
    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblContrasena;
    private TextBox txtContrasena;
    private Button btnIniciarSesion;
    private LinkLabel lblForgot;
    private Button btnEstudiante;
    private Button btnAdministrador;
    private Button btnVerPassword;
    private Label lblFooter;

    private readonly Color azulPrincipal = Color.FromArgb(0, 96, 210);
    private readonly Color azulOscuro = Color.FromArgb(10, 34, 78);
    private readonly Color azulClaro = Color.FromArgb(211, 231, 255);
    private readonly Color bordeCampo = Color.FromArgb(210, 218, 232);
    private bool passwordVisible;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text = "GALAB - Iniciar Sesion";
        Size = new Size(1280, 760);
        MinimumSize = new Size(980, 620);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);

        panelImagen = new Panel
        {
            BackColor = Color.FromArgb(235, 238, 242),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };
        panelImagen.Paint += PanelImagen_Paint;

        panelLogin = new Panel
        {
            BackColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        panelTopAzul = new Panel
        {
            Height = 40,
            BackColor = Color.FromArgb(164, 205, 255),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        picLogo = new PictureBox
        {
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };
        CargarLogo();

        lblGalab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 30F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblSubtitulo = new Label
        {
            Text = "Sistema de Gestion de Incidencias",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = azulOscuro,
            TextAlign = ContentAlignment.MiddleCenter
        };

        btnEstudiante = CrearBotonTipo("▱  Estudiante", true);
        btnAdministrador = CrearBotonTipo("▣  Administrador", false);
        btnEstudiante.Click += (s, e) => SeleccionarTipoUsuario(true);
        btnAdministrador.Click += (s, e) => SeleccionarTipoUsuario(false);

        lblUsuario = CrearLabelCampo("Usuario");
        txtUsuario = CrearTextBox("Ingrese su numero de control", false);

        lblContrasena = CrearLabelCampo("Contraseña");
        txtContrasena = CrearTextBox("Ingrese su contraseña", true);

        btnVerPassword = new Button
        {
            Text = "◉",
            Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnVerPassword.FlatAppearance.BorderSize = 0;
        btnVerPassword.Click += (s, e) =>
        {
            passwordVisible = !passwordVisible;
            txtContrasena.PasswordChar = passwordVisible ? '\0' : '●';
            btnVerPassword.Text = passwordVisible ? "●" : "◉";
        };

        btnIniciarSesion = new Button
        {
            Text = "🔒  Iniciar sesión",
            Height = 46,
            BackColor = azulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.Click += btnIniciarSesion_Click;

        lblForgot = new LinkLabel
        {
            Text = "¿Olvidaste tu contraseña?",
            AutoSize = true,
            Font = new Font("Segoe UI", 11F),
            LinkColor = azulPrincipal,
            ActiveLinkColor = azulOscuro,
            LinkBehavior = LinkBehavior.HoverUnderline
        };
        lblForgot.LinkClicked += lblForgot_LinkClicked;

        panelFooter = new Panel
        {
            BackColor = azulClaro,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
        };

        lblFooter = new Label
        {
            Text = "Instituto Tecnológico Superior de San Miguel el Grande",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = azulOscuro,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelFooter.Controls.Add(lblFooter);

        panelLogin.Controls.AddRange(new Control[]
        {
            panelTopAzul, picLogo, lblGalab, lblSubtitulo,
            btnEstudiante, btnAdministrador,
            lblUsuario, txtUsuario,
            lblContrasena, txtContrasena, btnVerPassword,
            btnIniciarSesion, lblForgot, panelFooter
        });

        Controls.AddRange(new Control[] { panelImagen, panelLogin });

        Resize += (s, e) => AjustarLayout();
        Load += (s, e) => AjustarLayout();
    }

    private void AjustarLayout()
    {
        int margen = 14;
        int alto = ClientSize.Height - margen * 2;
        int anchoImagen = Math.Max(420, ClientSize.Width / 2);

        panelImagen.Left = margen;
        panelImagen.Top = margen;
        panelImagen.Width = anchoImagen;
        panelImagen.Height = alto;

        panelLogin.Left = panelImagen.Right;
        panelLogin.Top = margen;
        panelLogin.Width = ClientSize.Width - panelLogin.Left - margen;
        panelLogin.Height = alto;

        panelTopAzul.Left = 0;
        panelTopAzul.Top = 0;
        panelTopAzul.Width = panelLogin.Width;

        int pw = panelLogin.Width;
        int formWidth = Math.Min(480, Math.Max(360, pw - 180));
        int left = (pw - formWidth) / 2;

        // Centrado vertical dinámico
        int contentHeight = 610;
        int startY = Math.Max(76, (panelLogin.Height - 78 - contentHeight) / 2);

        int txtH = formWidth > 420 ? 48 : 44;
        int btnH = formWidth > 420 ? 52 : 48;
        txtUsuario.Font = new Font("Segoe UI", formWidth > 420 ? 12F : 11F);
        txtContrasena.Font = new Font("Segoe UI", formWidth > 420 ? 12F : 11F);
        btnIniciarSesion.Font = new Font("Segoe UI", formWidth > 420 ? 13F : 12F, FontStyle.Bold);

        int picH = 110;
        int tabH = 48;
        int lblH = 24;

        picLogo.SetBounds((pw - 140) / 2, startY, 140, picH);
        lblGalab.SetBounds(0, startY + 115, pw, 50);
        lblSubtitulo.SetBounds(0, startY + 165, pw, 32);

        int tabY = startY + 225;
        int tabW = formWidth / 2;
        btnEstudiante.SetBounds(left, tabY, tabW, tabH);
        btnAdministrador.SetBounds(left + tabW, tabY, tabW, tabH);

        int lblUsrY = tabY + tabH + 35;
        lblUsuario.SetBounds(left, lblUsrY, formWidth, lblH);
        int txtUsrY = lblUsrY + lblH + 4;
        txtUsuario.SetBounds(left, txtUsrY, formWidth, txtH);

        int lblPwdY = txtUsrY + txtH + 20;
        lblContrasena.SetBounds(left, lblPwdY, formWidth, lblH);
        int txtPwdY = lblPwdY + lblH + 4;
        txtContrasena.SetBounds(left, txtPwdY, formWidth, txtH);
        btnVerPassword.SetBounds(left + formWidth - 44, txtPwdY + (txtH - 34) / 2, 36, 34);

        int btnY = txtPwdY + txtH + 28;
        btnIniciarSesion.SetBounds(left, btnY, formWidth, btnH);

        lblForgot.Left = left + (formWidth - lblForgot.Width) / 2;
        lblForgot.Top = btnY + btnH + 24;

        panelFooter.SetBounds(0, panelLogin.Height - 78, pw, 78);
    }

    private Button CrearBotonTipo(string text, bool activo)
    {
        var boton = new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = activo ? Color.White : azulPrincipal,
            BackColor = activo ? azulPrincipal : Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        boton.FlatAppearance.BorderColor = bordeCampo;
        boton.FlatAppearance.BorderSize = 1;
        return boton;
    }

    private Label CrearLabelCampo(string text) => new()
    {
        Text = text,
        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
        ForeColor = azulOscuro,
        TextAlign = ContentAlignment.MiddleLeft
    };

    private TextBox CrearTextBox(string placeholder, bool password) => new()
    {
        PlaceholderText = placeholder,
        PasswordChar = password ? '●' : '\0',
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font("Segoe UI", 11F),
        ForeColor = azulOscuro,
        BackColor = Color.White
    };

    private void SeleccionarTipoUsuario(bool estudiante)
    {
        btnEstudiante.BackColor = estudiante ? azulPrincipal : Color.White;
        btnEstudiante.ForeColor = estudiante ? Color.White : azulPrincipal;
        btnAdministrador.BackColor = estudiante ? Color.White : azulPrincipal;
        btnAdministrador.ForeColor = estudiante ? azulPrincipal : Color.White;
        txtUsuario.PlaceholderText = estudiante ? "Ingrese su numero de control" : "Ingrese usuario administrador";
        EstablecerRol(estudiante ? Models.RolUsuario.Estudiante : Models.RolUsuario.Administrador);
        txtUsuario.Focus();
    }

    private void CargarLogo()
    {
        picLogo.Image = UiAssets.CargarLogoInstitucion();
        if (picLogo.Image == null)
            picLogo.Paint += LogoFallback_Paint;
    }

    private void LogoFallback_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var fondo = new SolidBrush(azulClaro);
        using var borde = new Pen(azulPrincipal, 3);
        e.Graphics.FillEllipse(fondo, 18, 8, 100, 94);
        e.Graphics.DrawEllipse(borde, 18, 8, 100, 94);
        using var fuente = new Font("Segoe UI", 18F, FontStyle.Bold);
        e.Graphics.DrawString("G", fuente, new SolidBrush(azulPrincipal), 56, 36);
    }

    private void PanelImagen_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.FromArgb(238, 241, 245));

        using var pared = new LinearGradientBrush(panelImagen.ClientRectangle, Color.White, Color.FromArgb(210, 215, 222), 90F);
        g.FillRectangle(pared, panelImagen.ClientRectangle);

        using var luz = new SolidBrush(Color.FromArgb(245, 248, 252));
        g.FillRectangle(luz, 120, 20, panelImagen.Width - 240, 24);
        g.FillRectangle(luz, panelImagen.Width - 260, 54, 170, 22);

        using var mesa = new SolidBrush(Color.FromArgb(0, 92, 185));
        using var sombra = new SolidBrush(Color.FromArgb(25, 31, 40));
        int baseY = (int)(panelImagen.Height * 0.62);
        g.FillRectangle(mesa, 40, baseY, panelImagen.Width - 80, 96);
        g.FillRectangle(sombra, 40, baseY + 96, panelImagen.Width - 80, 24);

        for (int i = 0; i < 5; i++)
        {
            int x = 70 + i * Math.Max(92, (panelImagen.Width - 160) / 5);
            DibujarMonitor(g, x, baseY - 105);
        }

        for (int i = 0; i < 4; i++)
        {
            int x = 120 + i * Math.Max(105, (panelImagen.Width - 190) / 4);
            DibujarAlumno(g, x, baseY - 70);
        }

        using var ventana = new Pen(Color.FromArgb(70, 78, 90), 5);
        int vx = panelImagen.Width - 130;
        g.DrawRectangle(ventana, vx, 100, 92, 150);
        g.DrawLine(ventana, vx + 46, 100, vx + 46, 250);
        g.DrawLine(ventana, vx, 175, vx + 92, 175);
    }

    private void DibujarMonitor(Graphics g, int x, int y)
    {
        using var pantalla = new SolidBrush(Color.FromArgb(24, 31, 42));
        using var baseMonitor = new SolidBrush(Color.FromArgb(36, 44, 55));
        g.FillRectangle(pantalla, x, y, 82, 58);
        g.FillRectangle(baseMonitor, x + 34, y + 58, 14, 28);
        g.FillRectangle(baseMonitor, x + 20, y + 82, 42, 8);
    }

    private void DibujarAlumno(Graphics g, int x, int y)
    {
        using var piel = new SolidBrush(Color.FromArgb(205, 150, 110));
        using var cabello = new SolidBrush(Color.FromArgb(35, 28, 24));
        using var ropa = new SolidBrush(Color.FromArgb(18, 42, 75));
        g.FillEllipse(piel, x, y, 34, 38);
        g.FillPie(cabello, x - 2, y - 5, 38, 28, 180, 180);
        g.FillRectangle(ropa, x - 10, y + 36, 54, 54);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var path = new GraphicsPath();
        int r = 18;
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        path.AddArc(rect.Left, rect.Top, r, r, 180, 90);
        path.AddArc(rect.Right - r, rect.Top, r, r, 270, 90);
        path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - r, r, r, 90, 90);
        path.CloseFigure();
        Region = new Region(path);
    }
}
