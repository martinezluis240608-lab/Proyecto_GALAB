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
            BackColor = Color.White
        };
        CargarLogo();

        lblGalab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 30F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
        };

        lblSubtitulo = new Label
        {
            Text = "Sistema de Gestión de Incidencias",
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = azulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true
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

    private Image ObtenImagenPortada()
    {
        try
        {
            return UiAssets.CargarImagen("login-portada.png") ?? Properties.Resources.ima_L;
        }
        catch
        {
            return Properties.Resources.ima_L;
        }
    }

    private void AjustarLayout()
    {
        int margen = 14;
        int alto = ClientSize.Height - margen * 2;
        int anchoImagen = Math.Max(420, ClientSize.Width / 2);
        Color backColor = Color.FromArgb(235, 238, 242);
        try
        {
            var img = ObtenImagenPortada();
            if (img != null)
            {
                anchoImagen = (int)(alto * ((float)img.Width / img.Height));
                int maxAncho = (int)(ClientSize.Width * 0.55f);
                int minAncho = 360;
                anchoImagen = Math.Max(minAncho, Math.Min(maxAncho, anchoImagen));

                if (img is Bitmap bmp)
                {
                    try
                    {
                        var cornerPixel = bmp.GetPixel(0, 0);
                        if (cornerPixel.A == 255)
                        {
                            backColor = cornerPixel;
                        }
                    }
                    catch {}
                }
            }
        }
        catch {}

        panelImagen.BackColor = backColor;
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

        int txtH = formWidth > 420 ? 48 : 44;
        int btnH = formWidth > 420 ? 52 : 48;
        txtUsuario.Font = new Font("Segoe UI", formWidth > 420 ? 12F : 11F);
        txtContrasena.Font = new Font("Segoe UI", formWidth > 420 ? 12F : 11F);
        btnIniciarSesion.Font = new Font("Segoe UI", formWidth > 420 ? 13F : 12F, FontStyle.Bold);

        int picH = 110;
        int tabH = 48;
        int lblH = 24;

        // Centrado vertical dinámico basado en un estimado
        int contentHeight = 630;
        int startY = Math.Max(40, (panelLogin.Height - 78 - contentHeight) / 2);

        picLogo.SetBounds((pw - 140) / 2, startY, 140, picH);
        
        lblGalab.Left = (pw - lblGalab.Width) / 2;
        lblGalab.Top = startY + picH + 10;
        
        lblSubtitulo.Left = (pw - lblSubtitulo.Width) / 2;
        lblSubtitulo.Top = lblGalab.Bottom + 8;

        int tabY = lblSubtitulo.Bottom + 18;
        int tabW = formWidth / 2;
        btnEstudiante.SetBounds(left, tabY, tabW, tabH);
        btnAdministrador.SetBounds(left + tabW, tabY, tabW, tabH);

        int lblUsrY = tabY + tabH + 30;
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
        try { picLogo.Image = Properties.Resources.logo_instituto; }
        catch { picLogo.Paint += LogoFallback_Paint; }
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

    private static bool TienePixelesTransparentes(Image image)
    {
        if (image is not Bitmap bitmap) return false;

        int pasoX = Math.Max(1, bitmap.Width / 32);
        int pasoY = Math.Max(1, bitmap.Height / 32);
        for (int x = 0; x < bitmap.Width; x += pasoX)
        {
            for (int y = 0; y < bitmap.Height; y += pasoY)
            {
                if (bitmap.GetPixel(x, y).A < 255) return true;
            }
        }

        return false;
    }
    private void PanelImagen_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

        try
        {
            var img = ObtenImagenPortada();
            if (img != null)
            {
                float escala = Math.Min(
                    (float)panelImagen.Width / img.Width,
                    (float)panelImagen.Height / img.Height);
                int w = (int)(img.Width * escala);
                int h = (int)(img.Height * escala);
                int x = (panelImagen.Width - w) / 2;
                int y = (panelImagen.Height - h) / 2;

                g.Clear(panelImagen.BackColor);
                g.DrawImage(img, new Rectangle(x, y, w, h));
            }
            else
            {
                g.Clear(Color.FromArgb(235, 238, 242));
            }
        }
        catch
        {
            g.Clear(Color.FromArgb(235, 238, 242));
        }
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

