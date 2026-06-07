using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

public partial class ContactoForm : Form
{
    private readonly Color azulPrincipal = Color.FromArgb(32, 91, 187);
    private readonly Color azulOscuro = Color.FromArgb(9, 26, 75);
    private readonly Color azulClaro = Color.FromArgb(233, 243, 255);
    private readonly Color fondoGeneral = Color.FromArgb(247, 250, 254);
    private readonly Color borde = Color.FromArgb(220, 228, 238);
    private readonly Color texto = Color.FromArgb(8, 18, 48);

    private const string NombreUsuarioActual = "Nombre del usuario";
    private const string RolUsuarioActual = "Rol del usuario";
    private const string DireccionContacto = "Carretera a Valsequillo Km. 1.5, San Miguel el Grande,\nTlaxiaco, Oaxaca, C.P. 69800";
    private const string TelefonoContacto = "953 552 20 59";
    private const string CorreoContacto = "soporte@itsmg.edu.mx";
    private const string HorarioContacto = "Lunes a Viernes\n8:00 a.m. - 4:00 p.m.";
    private const string SitioWebContacto = "https://www.itsesm.edu.mx";

    public ContactoForm()
    {
        InitializeComponent();
        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        Text = "GALAB - Contacto";
        Size = new Size(1280, 760);
        MinimumSize = new Size(1050, 680);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        BackColor = fondoGeneral;
        Font = new Font("Segoe UI", 10F);
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = fondoGeneral
        };

        var footer = new Label
        {
            Text = "© 2026 GALAB - Instituto Tecnológico Superior de San Miguel el Grande",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = azulClaro,
            ForeColor = texto,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        };

        var sidebar = CrearSidebar();
        var header = CrearHeader();
        var contenido = CrearContenido();

        // Nest header and contenido into panelDerecho
        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        // Add controls to main Form in correct docking Z-order
        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearHeader()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 96,
            BackColor = azulClaro
        };

        var picLogoGalab = new PictureBox
        {
            Image = UiAssets.CargarLogoGalab(),
            SizeMode = PictureBoxSizeMode.Zoom,
            Location = new Point(24, 10),
            Size = new Size(180, 76),
            BackColor = Color.Transparent
        };

        var subtitulo = new Label
        {
            Text = "Sistema de Gestión\nde Incidencias",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = texto,
            Location = new Point(210, 32),
            AutoSize = true
        };

        var instituto = new Label
        {
            Text = "Instituto Tecnológico Superior\nde San Miguel el Grande",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = texto,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(400, 30),
            AutoSize = true
        };

        var logoInstituto = new PictureBox
        {
            Image = UiAssets.CargarLogoInstitucion(),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Size = new Size(64, 58)
        };

        header.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            logoInstituto.Left = header.Width - 665;
            logoInstituto.Top = 20;
            instituto.Left = header.Width - 590;
        };

        header.Controls.AddRange(new Control[] { picLogoGalab, subtitulo, logoInstituto, instituto });
        return header;
    }

    private Panel CrearSidebar()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 290,
            BackColor = Color.White,
            Padding = new Padding(16, 28, 16, 16)
        };

        int y = 38;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("⌂", "Inicio", y, false, () => UiAssets.AbrirCerrandoActual(this, new PrincipalForm())));
        y += 68;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias   ›", y, false, () => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm())));
        y += 68;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, false, () => UiAssets.AbrirCerrandoActual(this, new PerfilForm())));
        y += 68;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, true, null));
        y += 68;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesión",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
            Location = new Point(40, 610),
            Size = new Size(210, 48),
            TextAlign = ContentAlignment.MiddleCenter
        };
        cerrar.FlatAppearance.BorderColor = Color.FromArgb(172, 199, 232);
        cerrar.FlatAppearance.BorderSize = 1;
        cerrar.Click += (s, e) => UiAssets.CerrarSesion(this);
        sidebar.Resize += (s, e) => cerrar.Top = sidebar.Height - 70;
        sidebar.Controls.Add(cerrar);
        UiAssets.RedondearControl(cerrar, 8);

        return sidebar;
    }

    private Panel CrearContenido()
    {
        var contenido = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = fondoGeneral,
            Padding = new Padding(28, 24, 28, 24)
        };

        var icono = CrearIconoGrande("☎");
        icono.Location = new Point(34, 28);

        var titulo = new Label
        {
            Text = "Contacto",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            Location = new Point(118, 35),
            AutoSize = true
        };

        var descripcion = new Label
        {
            Text = "Estamos aquí para ayudarte. Ponte en contacto con nosotros.",
            Font = new Font("Segoe UI", 13F),
            ForeColor = texto,
            Location = new Point(36, 105),
            AutoSize = true
        };

        var card = CrearCardContacto();
        card.Location = new Point(36, 170);

        contenido.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int w = Math.Min(980, Math.Max(560, contenido.ClientSize.Width - 92));
            int left = Math.Max(36, (contenido.ClientSize.Width - w) / 2);
            
            card.Width = w;
            card.Left = left;
            
            icono.Left = left - 2;
            titulo.Left = left + 82;
            descripcion.Left = left;
            
            contenido.AutoScrollMinSize = new Size(0, card.Bottom + 50);
        };

        contenido.Controls.AddRange(new Control[] { icono, titulo, descripcion, card });
        return contenido;
    }

    private Panel CrearCardContacto()
    {
        var card = new Panel
        {
            Size = new Size(650, 640),
            BackColor = Color.White
        };
        card.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, card.ClientRectangle, 12, Color.White, borde);

        var lblTituloCard = new Label
        {
            Text = "📄  Información de contacto",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(28, 20),
            AutoSize = true
        };
        var separador = new Panel
        {
            BackColor = borde,
            Location = new Point(28, 68),
            Size = new Size(590, 1),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        card.Controls.AddRange(new Control[]
        {
            lblTituloCard,
            separador,
            CrearDatoContacto("📍", "Dirección", DireccionContacto, 85, null),
            CrearDatoContacto("☎", "Teléfono", TelefonoContacto, 195, () => CopiarAlPortapapeles(TelefonoContacto, "Teléfono copiado.")),
            CrearDatoContacto("✉", "Correo electrónico", CorreoContacto, 305, () => AbrirCorreo()),
            CrearDatoContacto("◷", "Horario de atención", HorarioContacto, 415, null),
            CrearDatoContacto("🌐", "Sitio web", SitioWebContacto, 525, AbrirSitioWeb)
        });

        return card;
    }

    private Panel CrearDatoContacto(string icono, string titulo, string valor, int y, Action? accion)
    {
        var item = new Panel
        {
            BackColor = Color.White,
            Location = new Point(28, y),
            Size = new Size(590, 96),
            Cursor = accion == null ? Cursors.Default : Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var cajaIcono = new Label
        {
            Text = icono,
            Font = new Font("Segoe UI Emoji", 18F),
            ForeColor = azulPrincipal,
            BackColor = azulClaro,
            Location = new Point(0, 12),
            Size = new Size(48, 48),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            Location = new Point(72, 4),
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var lblValor = new Label
        {
            Text = valor,
            Font = new Font("Segoe UI", 10F),
            ForeColor = texto,
            Location = new Point(72, 28),
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        item.Resize += (s, e) =>
        {
            lblValor.MaximumSize = new Size(item.Width - 80, 0);
        };
        lblValor.MaximumSize = new Size(item.Width - 80, 0);

        if (accion != null)
        {
            item.Click += (s, e) => accion();
            cajaIcono.Click += (s, e) => accion();
            lblTitulo.Click += (s, e) => accion();
            lblValor.Click += (s, e) => accion();
        }

        item.Controls.AddRange(new Control[] { cajaIcono, lblTitulo, lblValor });
        return item;
    }

    private Panel CrearIconoGrande(string icono)
    {
        var panel = new Panel
        {
            BackColor = azulPrincipal,
            Size = new Size(58, 58)
        };
        panel.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, panel.ClientRectangle, 10, azulPrincipal, azulPrincipal);
        panel.Controls.Add(new Label
        {
            Text = icono,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Emoji", 24F),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        });
        return panel;
    }

    private void CopiarAlPortapapeles(string valor, string mensaje)
    {
        Clipboard.SetText(valor);
        Proyecto_GALAB.Views.CustomMessageBox.Show(mensaje, "Contacto", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void AbrirCorreo()
    {
        Process.Start(new ProcessStartInfo($"mailto:{CorreoContacto}") { UseShellExecute = true });
    }

    private void AbrirSitioWeb()
    {
        Process.Start(new ProcessStartInfo(SitioWebContacto) { UseShellExecute = true });
    }

    private static void DibujarBordeRedondo(Graphics g, Rectangle rect, int radio, Color fondo, Color colorBorde)
    {
        using var path = CrearRectanguloRedondo(new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), radio);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var brush = new SolidBrush(fondo);
        using var pen = new Pen(colorBorde);
        g.FillPath(brush, path);
        g.DrawPath(pen, path);
    }

    private static GraphicsPath CrearRectanguloRedondo(Rectangle rect, int radio)
    {
        int diametro = radio * 2;
        var path = new GraphicsPath();
        path.AddArc(rect.Left, rect.Top, diametro, diametro, 180, 90);
        path.AddArc(rect.Right - diametro, rect.Top, diametro, diametro, 270, 90);
        path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - diametro, diametro, diametro, 90, 90);
        path.CloseFigure();
        return path;
    }
}
