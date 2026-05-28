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

    // Datos editables del módulo Contacto.
    // Cuando conectes una base de datos, estos valores pueden venir de una tabla de configuración.
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
        BackColor = fondoGeneral;
        Font = new Font("Segoe UI", 10F);
        Controls.Clear();

        Controls.Add(CrearContenido());
        Controls.Add(CrearSidebar());
        Controls.Add(CrearHeader());
        Controls.Add(new Label
        {
            Text = "© 2026 GALAB - Instituto Tecnológico Superior de San Miguel el Grande",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = azulClaro,
            ForeColor = texto,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        });
    }

    private Panel CrearHeader()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 96,
            BackColor = azulClaro
        };

        var logoIcono = new Label
        {
            Text = "⚙",
            Font = new Font("Segoe UI Symbol", 36F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            Location = new Point(24, 18),
            Size = new Size(62, 58),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var galab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(96, 16),
            Size = new Size(180, 34)
        };

        var subtitulo = new Label
        {
            Text = "Sistema de Gestión\nde Incidencias",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = texto,
            Location = new Point(100, 52),
            Size = new Size(180, 40)
        };

        var instituto = new Label
        {
            Text = "Instituto Tecnológico Superior\nde San Miguel el Grande",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = texto,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(760, 30),
            Size = new Size(290, 42)
        };

        var campana = new Label
        {
            Text = "🔔",
            Font = new Font("Segoe UI Emoji", 20F),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(1065, 28),
            Size = new Size(44, 40),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var usuario = new Label
        {
            Text = $"👤  {NombreUsuarioActual}\n     {RolUsuarioActual}",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = texto,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(1075, 28),
            Size = new Size(190, 44)
        };

        header.Resize += (s, e) =>
        {
            instituto.Left = header.Width - 590;
            campana.Left = header.Width - 270;
            usuario.Left = header.Width - 220;
        };

        header.Controls.AddRange(new Control[] { logoIcono, galab, subtitulo, instituto, campana, usuario });
        return header;
    }

    private Panel CrearSidebar()
    {
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 250,
            BackColor = Color.White,
            Padding = new Padding(16, 28, 16, 16)
        };

        int y = 38;
        sidebar.Controls.Add(CrearBotonMenu("⌂  Inicio", y, IrAInicio));
        y += 68;
        sidebar.Controls.Add(CrearBotonMenu("☷  Gestión de Incidencias   ›", y, () => AbrirFormulario(new IncidenciaForm())));
        y += 68;
        sidebar.Controls.Add(CrearBotonMenu("👤  Perfil", y, () => AbrirFormulario(new PerfilForm())));
        y += 68;
        sidebar.Controls.Add(CrearBotonMenu("☎  Contacto", y, null, true));
        y += 68;
        sidebar.Controls.Add(CrearBotonMenu("?  Ayuda", y, () => AbrirFormulario(new AyudaForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesión",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
            Location = new Point(24, 610),
            Size = new Size(205, 44),
            TextAlign = ContentAlignment.MiddleCenter
        };
        cerrar.FlatAppearance.BorderColor = Color.FromArgb(172, 199, 232);
        cerrar.FlatAppearance.BorderSize = 1;
        cerrar.Click += (s, e) => CerrarSesion();
        sidebar.Resize += (s, e) => cerrar.Top = sidebar.Height - 70;
        sidebar.Controls.Add(cerrar);

        return sidebar;
    }

    private Button CrearBotonMenu(string textoBoton, int y, Action? accion, bool activo = false)
    {
        var boton = new Button
        {
            Text = textoBoton,
            Font = new Font("Segoe UI", 10F, activo ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = activo ? azulPrincipal : texto,
            BackColor = activo ? azulClaro : Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(16, y),
            Size = new Size(218, 50),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0)
        };
        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = azulClaro;
        if (accion != null)
            boton.Click += (s, e) => accion();

        return boton;
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
            Size = new Size(260, 44)
        };

        var descripcion = new Label
        {
            Text = "Estamos aquí para ayudarte. Ponte en contacto con nosotros.",
            Font = new Font("Segoe UI", 13F),
            ForeColor = texto,
            Location = new Point(36, 105),
            Size = new Size(780, 34)
        };

        var card = CrearCardContacto();
        card.Location = new Point(36, 170);

        contenido.Resize += (s, e) =>
        {
            card.Width = Math.Min(620, Math.Max(560, contenido.ClientSize.Width - 92));
            contenido.AutoScrollMinSize = new Size(0, card.Bottom + 50);
        };

        contenido.Controls.AddRange(new Control[] { icono, titulo, descripcion, card });
        return contenido;
    }

    private Panel CrearCardContacto()
    {
        var card = new Panel
        {
            Size = new Size(620, 520),
            BackColor = Color.White
        };
        card.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, card.ClientRectangle, 12, Color.White, borde);

        var titulo = new Label
        {
            Text = "📄  Información de contacto",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(28, 24),
            Size = new Size(380, 38)
        };
        var separador = new Panel
        {
            BackColor = borde,
            Location = new Point(28, 82),
            Size = new Size(560, 1),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        card.Controls.AddRange(new Control[]
        {
            titulo,
            separador,
            CrearDatoContacto("📍", "Dirección", DireccionContacto, 108, null),
            CrearDatoContacto("☎", "Teléfono", TelefonoContacto, 198, () => CopiarAlPortapapeles(TelefonoContacto, "Teléfono copiado.")),
            CrearDatoContacto("✉", "Correo electrónico", CorreoContacto, 276, () => AbrirCorreo()),
            CrearDatoContacto("◷", "Horario de atención", HorarioContacto, 354, null),
            CrearDatoContacto("🌐", "Sitio web", SitioWebContacto, 442, AbrirSitioWeb)
        });

        return card;
    }

    private Panel CrearDatoContacto(string icono, string titulo, string valor, int y, Action? accion)
    {
        var item = new Panel
        {
            BackColor = Color.White,
            Location = new Point(28, y),
            Size = new Size(560, 72),
            Cursor = accion == null ? Cursors.Default : Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var cajaIcono = new Label
        {
            Text = icono,
            Font = new Font("Segoe UI Emoji", 16F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = azulClaro,
            Location = new Point(0, 6),
            Size = new Size(48, 48),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            Location = new Point(72, 2),
            Size = new Size(430, 24)
        };

        var lblValor = new Label
        {
            Text = valor,
            Font = new Font("Segoe UI", 10F),
            ForeColor = texto,
            Location = new Point(72, 28),
            Size = new Size(465, 42)
        };

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

    private void IrAInicio()
    {
        Close();
    }

    private void AbrirFormulario(Form formulario)
    {
        formulario.StartPosition = FormStartPosition.CenterScreen;
        formulario.Show();
        Close();
    }

    private void CerrarSesion()
    {
        var login = new LoginForm();
        login.Show();

        foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
        {
            if (form != login)
                form.Close();
        }
    }

    private void CopiarAlPortapapeles(string valor, string mensaje)
    {
        Clipboard.SetText(valor);
        MessageBox.Show(mensaje, "Contacto", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
