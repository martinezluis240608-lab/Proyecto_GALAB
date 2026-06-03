using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

public partial class AyudaForm : Form
{
    private readonly Color azulPrincipal = Color.FromArgb(32, 91, 187);
    private readonly Color azulOscuro = Color.FromArgb(9, 26, 75);
    private readonly Color azulClaro = Color.FromArgb(233, 243, 255);
    private readonly Color fondoGeneral = Color.FromArgb(247, 250, 254);
    private readonly Color borde = Color.FromArgb(220, 228, 238);
    private readonly Color texto = Color.FromArgb(8, 18, 48);

    // Datos temporales del usuario.
    // Cuando conectes la base de datos, reemplaza estos valores con los datos
    // obtenidos del usuario que inició sesión.
    private const string NombreUsuarioActual = "Nombre del usuario";
    private const string RolUsuarioActual = "Rol del usuario";

    public AyudaForm()
    {
        Text = "GALAB - Ayuda";
        Size = new Size(1280, 760);
        MinimumSize = new Size(1050, 680);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        BackColor = fondoGeneral;
        Font = new Font("Segoe UI", 10F);

        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        var header = CrearHeader();
        var sidebar = CrearSidebar();
        var contenido = CrearContenido();
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

        Controls.Add(contenido);
        Controls.Add(sidebar);
        Controls.Add(header);
        Controls.Add(footer);
    }

    private Panel CrearHeader()
    {
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 96,
            BackColor = azulClaro,
            Padding = new Padding(28, 12, 28, 12)
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

        var logoInstituto = new PictureBox
        {
            Image = UiAssets.CargarLogoInstitucion(),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Size = new Size(64, 58)
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
            Location = new Point(1115, 28),
            Size = new Size(190, 44)
        };

        header.Resize += (s, e) =>
        {
            logoInstituto.Left = header.Width - 665;
            logoInstituto.Top = 20;
            instituto.Left = header.Width - 590;
            campana.Left = header.Width - 270;
            usuario.Left = header.Width - 220;
        };

        header.Controls.AddRange(new Control[] { logoIcono, galab, subtitulo, logoInstituto, instituto, campana, usuario });
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
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 68;
        sidebar.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, true, null));

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
        var contenedor = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = fondoGeneral,
            AutoScroll = true,
            Padding = new Padding(28, 24, 28, 24)
        };

        var icono = CrearIconoGrande("🌐");
        icono.Location = new Point(28, 24);

        var titulo = new Label
        {
            Text = "Ayuda",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            Location = new Point(116, 30),
            Size = new Size(240, 44)
        };

        var descripcion = new Label
        {
            Text = "Encuentra aquí manuales, guías y materiales que te ayudarán a utilizar el sistema GALAB de manera fácil y efectiva.",
            Font = new Font("Segoe UI", 11F),
            ForeColor = texto,
            Location = new Point(32, 92),
            Size = new Size(650, 50)
        };

        var manual = CrearCardManual();
        var material = CrearCardMaterial();
        manual.Location = new Point(32, 170);
        material.Location = new Point(500, 170);

        contenedor.Resize += (s, e) =>
        {
            int anchoDisponible;
            int leftMargin;
            int maxAncho = 1140;

            if (contenedor.ClientSize.Width > maxAncho + 64)
            {
                anchoDisponible = maxAncho;
                leftMargin = (contenedor.ClientSize.Width - maxAncho) / 2;
            }
            else
            {
                leftMargin = 32;
                anchoDisponible = Math.Max(500, contenedor.ClientSize.Width - 64);
            }

            // Alinear cabecera
            icono.Left = leftMargin - 4;
            titulo.Left = leftMargin + 84;
            descripcion.Left = leftMargin;

            if (contenedor.ClientSize.Width < 980)
            {
                manual.Left = leftMargin;
                manual.Width = anchoDisponible;
                material.Left = leftMargin;
                material.Width = anchoDisponible;
                material.Top = manual.Bottom + 22;
                contenedor.AutoScrollMinSize = new Size(0, material.Bottom + 40);
            }
            else
            {
                int anchoCard = (anchoDisponible - 24) / 2;
                manual.Left = leftMargin;
                manual.Width = anchoCard;
                material.Left = manual.Right + 24;
                material.Width = anchoCard;
                material.Top = manual.Top;
                contenedor.AutoScrollMinSize = new Size(0, Math.Max(manual.Bottom, material.Bottom) + 40);
            }
        };

        contenedor.Controls.AddRange(new Control[] { icono, titulo, descripcion, manual, material });
        return contenedor;
    }

    private Panel CrearCardManual()
    {
        var card = CrearCard(570);
        var titulo = CrearTituloCard("📋", "Manual de usuario", 24, 24);
        var textoIntro = CrearTexto("Consulta el manual de usuario para conocer paso a paso cómo utilizar todas las funciones del sistema.", 24, 82, 410, 48);
        var separador = CrearSeparador(24, 140);
        var temas = new Label
        {
            Text = "Temas incluidos:",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(24, 158),
            Size = new Size(220, 24)
        };

        string[] lista =
        {
            "Inicio de sesión",
            "Gestión de incidencias",
            "Registro de incidencias",
            "Consulta de historial",
            "Perfil de usuario",
            "Opciones y configuración",
            "Cerrar sesión"
        };

        int y = 190;
        foreach (var item in lista)
        {
            card.Controls.Add(new Label
            {
                Text = "●  " + item,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = texto,
                Location = new Point(32, y),
                Size = new Size(330, 24),
                Tag = "tema"
            });
            y += 26;
        }

        var descargar = new Button
        {
            Text = "⇩  Descargar manual (PDF)",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(24, 392),
            Size = new Size(230, 38)
        };
        descargar.FlatAppearance.BorderColor = azulPrincipal;
        descargar.FlatAppearance.BorderSize = 1;
        descargar.Click += (s, e) => DescargarManual();

        var importante = new Label
        {
            Text = "ℹ  Importante\n    Te recomendamos leer el manual para aprovechar al máximo todas las funciones del sistema.",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = texto,
            BackColor = azulClaro,
            Location = new Point(280, 392),
            Size = new Size(300, 70),
            Padding = new Padding(10)
        };

        card.Resize += (s, e) =>
        {
            textoIntro.Width = card.Width - 48;
            separador.Width = card.Width - 48;
            foreach (Control control in card.Controls)
            {
                if (control.Tag?.ToString() == "tema")
                    control.Width = Math.Max(330, card.Width - 72);
            }

            if (card.Width > 620)
            {
                importante.Left = card.Width - 324;
                importante.Top = 392;
                importante.Width = 300;
                importante.Visible = true;
            }
            else
            {
                importante.Left = 24;
                importante.Top = 444;
                importante.Width = card.Width - 48;
                importante.Visible = true;
            }
        };

        card.Controls.AddRange(new Control[] { titulo, textoIntro, separador, temas, descargar, importante });
        return card;
    }

    private Panel CrearCardMaterial()
    {
        var card = CrearCard(540);
        var titulo = CrearTituloCard("📄", "Material complementario", 24, 24);
        var intro = CrearTexto("Consulta guías rápidas, preguntas frecuentes y otros recursos que te pueden ser de utilidad.", 24, 82, 410, 48);
        var separador = CrearSeparador(24, 140);
        var disponibles = new Label
        {
            Text = "Recursos disponibles:",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(24, 158),
            Size = new Size(220, 24)
        };

        var guia = CrearRecurso("📖", "Guía rápida", "Pasos básicos para registrar una incidencia.", 190, () => MostrarRecurso("Guía rápida", "1. Inicia sesión.\n2. Entra a Gestión de Incidencias.\n3. Llena los datos de la incidencia.\n4. Adjunta evidencia si es necesario.\n5. Presiona Enviar reporte."));
        var faq = CrearRecurso("?", "Preguntas frecuentes (FAQ)", "Resuelve las dudas más comunes sobre el sistema.", 252, () => MostrarRecurso("Preguntas frecuentes", "¿Cómo registro una incidencia?\nDesde Gestión de Incidencias.\n\n¿Qué hago si olvidé mi contraseña?\nContacta al administrador.\n\n¿Puedo adjuntar evidencia?\nSí, desde el botón Adjuntar."));
        var plantillas = CrearRecurso("⇩", "Formatos y plantillas", "Descarga formatos útiles para tus reportes.", 314, AbrirPlantillas);

        var contacto = new Panel
        {
            BackColor = azulClaro,
            Location = new Point(24, 392),
            Size = new Size(520, 58)
        };
        contacto.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, contacto.ClientRectangle, 10, azulClaro, azulClaro);
        var contactoTexto = new Label
        {
            Text = "🎧  ¿No encontraste lo que buscabas?\n     Contáctanos y con gusto te ayudaremos.",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = texto,
            Location = new Point(12, 8),
            Size = new Size(320, 42)
        };
        var irContacto = new Button
        {
            Text = "Ir a contacto  ›",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(365, 12),
            Size = new Size(130, 34)
        };
        irContacto.FlatAppearance.BorderColor = azulPrincipal;
        irContacto.FlatAppearance.BorderSize = 1;
        irContacto.Click += (s, e) => UiAssets.AbrirCerrandoActual(this, new ContactoForm());
        contacto.Controls.AddRange(new Control[] { contactoTexto, irContacto });

        card.Resize += (s, e) =>
        {
            intro.Width = card.Width - 48;
            separador.Width = card.Width - 48;
            foreach (Control control in card.Controls)
            {
                if (control.Tag?.ToString() == "recurso")
                    control.Width = card.Width - 48;
            }
            contacto.Width = card.Width - 48;
            irContacto.Left = Math.Max(330, contacto.Width - 150);
        };

        card.Controls.AddRange(new Control[] { titulo, intro, separador, disponibles, guia, faq, plantillas, contacto });
        return card;
    }

    private Panel CrearCard(int alto)
    {
        var card = new Panel
        {
            BackColor = Color.White,
            Size = new Size(430, alto),
            Padding = new Padding(20)
        };
        card.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, card.ClientRectangle, 12, Color.White, borde);
        return card;
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

    private Label CrearTituloCard(string icono, string titulo, int x, int y) => new()
    {
        Text = $"{icono}  {titulo}",
        Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
        ForeColor = azulOscuro,
        Location = new Point(x, y),
        Size = new Size(380, 34)
    };

    private Label CrearTexto(string contenido, int x, int y, int ancho, int alto) => new()
    {
        Text = contenido,
        Font = new Font("Segoe UI", 9.5F),
        ForeColor = texto,
        Location = new Point(x, y),
        Size = new Size(ancho, alto)
    };

    private Panel CrearSeparador(int x, int y) => new()
    {
        BackColor = borde,
        Location = new Point(x, y),
        Size = new Size(380, 1)
    };

    private Panel CrearRecurso(string icono, string titulo, string descripcion, int y, Action accion)
    {
        var recurso = new Panel
        {
            BackColor = Color.White,
            Location = new Point(24, y),
            Size = new Size(520, 50),
            Cursor = Cursors.Hand,
            Tag = "recurso"
        };
        recurso.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, recurso.ClientRectangle, 8, Color.White, borde);

        var lblIcono = new Label
        {
            Text = icono,
            Font = new Font("Segoe UI Emoji", 16F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = azulClaro,
            Location = new Point(12, 9),
            Size = new Size(34, 32),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(64, 6),
            Size = new Size(310, 22)
        };

        var lblDescripcion = new Label
        {
            Text = descripcion,
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = texto,
            Location = new Point(64, 27),
            Size = new Size(360, 18)
        };

        var flecha = new Label
        {
            Text = "›",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(485, 8),
            Size = new Size(25, 34)
        };

        recurso.Resize += (s, e) => flecha.Left = recurso.Width - 35;
        recurso.Click += (s, e) => accion();
        foreach (Control control in new Control[] { lblIcono, lblTitulo, lblDescripcion, flecha })
            control.Click += (s, e) => accion();

        recurso.Controls.AddRange(new Control[] { lblIcono, lblTitulo, lblDescripcion, flecha });
        return recurso;
    }

    private void DescargarManual()
    {
        string ruta = Path.Combine(Application.StartupPath, "Recursos", "manual_usuario.pdf");
        if (File.Exists(ruta))
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            return;
        }

        MessageBox.Show(
            "El archivo manual_usuario.pdf todavía no está agregado.\n\nCuando lo tengas, colócalo en la carpeta Recursos del ejecutable.",
            "Manual de usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void MostrarRecurso(string titulo, string mensaje)
    {
        MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void AbrirPlantillas()
    {
        string carpeta = Path.Combine(Application.StartupPath, "Recursos", "Plantillas");
        if (Directory.Exists(carpeta))
        {
            Process.Start(new ProcessStartInfo(carpeta) { UseShellExecute = true });
            return;
        }

        MessageBox.Show(
            "La carpeta de plantillas todavía no existe.\n\nCrea una carpeta llamada Recursos\\Plantillas junto al ejecutable para guardar formatos.",
            "Formatos y plantillas",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
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
