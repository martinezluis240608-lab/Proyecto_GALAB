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

        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

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
            BackColor = azulClaro,
            Padding = new Padding(28, 12, 28, 12)
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
            Location = new Point(760, 30),
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
        UiAssets.RedondearControl(cerrar, 8);

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
            AutoSize = true
        };

        var descripcion = new Label
        {
            Text = "Encuentra aquí manuales, guías y materiales que te ayudarán a utilizar el sistema GALAB de manera fácil y efectiva.",
            Font = new Font("Segoe UI", 11F),
            ForeColor = texto,
            Location = new Point(32, 92),
            AutoSize = true
        };

        var manual = CrearCardManual();
        var material = CrearCardMaterial();
        manual.Location = new Point(32, 170);
        material.Location = new Point(500, 170);

        contenedor.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int anchoDisponible;
            int leftMargin;
            int maxAncho = 1600;

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
            descripcion.MaximumSize = new Size(anchoDisponible, 0);
            descripcion.Width = anchoDisponible;

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
        var card = CrearCard(720);
        var titulo = CrearTituloCard("📋", "Manual de usuario", 24, 24);
        var textoIntro = CrearTexto("Consulta el manual de usuario para conocer paso a paso cómo utilizar todas las funciones del sistema.", 24, 82, 410, 48);
        var separador = CrearSeparador(24, 140);
        var temas = new Label
        {
            Text = "Temas incluidos:",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
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
                Text = "•  " + item,
                Font = new Font("Segoe UI", 11F),
                ForeColor = texto,
                Location = new Point(32, y),
                Size = new Size(330, 28),
                Tag = "tema"
            });
            y += 36; // Much better vertical line spacing
        }

        var descargar = new Button
        {
            Text = "⇩  Descargar manual (PDF)",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(24, 470),
            Size = new Size(240, 44)
        };
        descargar.FlatAppearance.BorderColor = azulPrincipal;
        descargar.FlatAppearance.BorderSize = 1;
        descargar.Click += (s, e) => DescargarManual();
        UiAssets.RedondearControl(descargar, 8);

        // Panel 'Importante' con fondo redondeado pintado (sin RedondearControl que clipa)
        var panelImportante = new Panel
        {
            BackColor = Color.Transparent,
            Location  = new Point(280, 470),
            Size      = new Size(320, 90)
        };
        panelImportante.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var br   = new SolidBrush(azulClaro);
            using var path = DibujarRectRedondo(new Rectangle(0, 0, panelImportante.Width - 1, panelImportante.Height - 1), 10);
            e.Graphics.FillPath(br, path);
        };
        var lblImportante = new Label
        {
            Text        = "ℹ  Importante",
            Font        = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor   = azulOscuro,
            BackColor   = Color.Transparent,
            Location    = new Point(12, 10),
            AutoSize    = true,
            MaximumSize = new Size(296, 0)
        };
        var lblImportanteDesc = new Label
        {
            Text        = "Te recomendamos leer el manual para aprovechar al máximo todas las funciones del sistema.",
            Font        = new Font("Segoe UI", 9.5F),
            ForeColor   = texto,
            BackColor   = Color.Transparent,
            Location    = new Point(12, 32),
            AutoSize    = true,
            MaximumSize = new Size(296, 0)
        };
        panelImportante.Controls.AddRange(new Control[] { lblImportante, lblImportanteDesc });
        panelImportante.Resize += (s, e) =>
        {
            int maxW = panelImportante.Width - 24;
            lblImportante.MaximumSize     = new Size(maxW, 0);
            lblImportanteDesc.MaximumSize = new Size(maxW, 0);
            panelImportante.Height = Math.Max(80, lblImportanteDesc.Bottom + 14);
            panelImportante.Invalidate();
        };

        card.Resize += (s, e) =>
        {
            textoIntro.MaximumSize = new Size(card.Width - 48, 0);
            textoIntro.Width = card.Width - 48;
            separador.Width  = card.Width - 48;
            foreach (Control control in card.Controls)
            {
                if (control.Tag?.ToString() == "tema")
                    control.Width = Math.Max(330, card.Width - 72);
            }

            if (card.Width > 620)
            {
                panelImportante.Left  = card.Width - 344;
                panelImportante.Top   = 470;
                panelImportante.Width = Math.Min(320, card.Width - 48);
            }
            else
            {
                panelImportante.Left  = 24;
                panelImportante.Top   = 540;
                panelImportante.Width = card.Width - 48;
            }
            panelImportante.Visible = true;
            panelImportante.PerformLayout();
            card.Invalidate();
        };

        card.Controls.AddRange(new Control[] { titulo, textoIntro, separador, temas, descargar, panelImportante });
        return card;
    }

    private Panel CrearCardMaterial()
    {
        var card = CrearCard(720);
        var titulo = CrearTituloCard("📄", "Material complementario", 24, 24);
        var intro = CrearTexto("Consulta guías rápidas, preguntas frecuentes y otros recursos que te pueden ser de utilidad.", 24, 82, 410, 48);
        var separador = CrearSeparador(24, 140);
        var disponibles = new Label
        {
            Text = "Recursos disponibles:",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location = new Point(24, 158),
            Size = new Size(220, 24)
        };

        var guia = CrearRecurso("📖", "Guía rápida", "Pasos básicos para registrar una incidencia.", 190, () => MostrarRecurso("Guía rápida", "1. Inicia sesión.\n2. Entra a Gestión de Incidencias.\n3. Llena los datos de la incidencia.\n4. Adjunta evidencia si es necesario.\n5. Presiona Enviar reporte."));
        var faq = CrearRecurso("💬", "Preguntas frecuentes (FAQ)", "Resuelve las dudas más comunes sobre el sistema.", 282, () => MostrarRecurso("Preguntas frecuentes", "¿Cómo registro una incidencia?\nDesde Gestión de Incidencias.\n\n¿Qué hago si olvidé mi contraseña?\nContacta al administrador.\n\n¿Puedo adjuntar evidencia?\nSí, desde el botón Adjuntar."));

        var contacto = new Panel
        {
            BackColor = azulClaro,
            Location = new Point(24, 374),
            Size = new Size(520, 80)
        };
        contacto.Paint += (s, e) => DibujarBordeRedondo(e.Graphics, contacto.ClientRectangle, 10, azulClaro, azulClaro);
        var contactoTexto = new Label
        {
            Text        = "🎧  ¿No encontraste lo que buscabas?\n     Contáctanos y con gusto te ayudaremos.",
            Font        = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor   = texto,
            BackColor   = Color.Transparent,
            Location    = new Point(16, 10),
            AutoSize    = true,
            MaximumSize = new Size(320, 0)
        };
        var irContacto = new Button
        {
            Text = "Ir a contacto  ›",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = azulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(365, 21),
            Size = new Size(130, 38)
        };
        irContacto.FlatAppearance.BorderColor = azulPrincipal;
        irContacto.FlatAppearance.BorderSize = 1;
        irContacto.Click += (s, e) => UiAssets.AbrirCerrandoActual(this, new ContactoForm());
        UiAssets.RedondearControl(irContacto, 8);

        irContacto.MouseEnter += (s, e) =>
        {
            irContacto.BackColor = azulPrincipal;
            irContacto.ForeColor = Color.White;
        };
        irContacto.MouseLeave += (s, e) =>
        {
            irContacto.BackColor = Color.White;
            irContacto.ForeColor = azulPrincipal;
        };

        contacto.Controls.AddRange(new Control[] { contactoTexto, irContacto });

        card.Resize += (s, e) =>
        {
            intro.MaximumSize = new Size(card.Width - 48, 0);
            intro.Width = card.Width - 48;
            separador.Width = card.Width - 48;
            foreach (Control control in card.Controls)
            {
                if (control.Tag?.ToString() == "recurso")
                    control.Width = card.Width - 48;
            }
            contacto.Width            = card.Width - 48;
            contactoTexto.MaximumSize  = new Size(Math.Max(180, contacto.Width - 170), 0);
            irContacto.Left            = Math.Max(contactoTexto.Right + 12, contacto.Width - 148);
            contacto.Height            = Math.Max(68, contactoTexto.Bottom + 14);
            contacto.Invalidate();
            card.Invalidate();
        };

        card.Controls.AddRange(new Control[] { titulo, intro, separador, disponibles, guia, faq, contacto });
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
        card.Resize += (s, e) => card.Invalidate();
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
        Font = new Font("Segoe UI", 10.5F),
        ForeColor = texto,
        Location = new Point(x, y),
        MaximumSize = new Size(ancho, 0),
        AutoSize = true
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
            Location  = new Point(24, y),
            Size      = new Size(520, 76),
            Cursor    = Cursors.Hand,
            Tag       = "recurso"
        };

        // ─── Ícono: Panel con fondo redondeado pintado (evita Region-clip invisible) ───
        var panelIcono = new Panel
        {
            BackColor = Color.Transparent,
            Location  = new Point(16, 16),
            Size      = new Size(44, 44)
        };

        panelIcono.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var br   = new SolidBrush(panelIcono.Tag is Color c ? c : azulClaro);
            using var path = DibujarRectRedondo(new Rectangle(0, 0, panelIcono.Width - 1, panelIcono.Height - 1), 10);
            e.Graphics.FillPath(br, path);
        };

        var lblEmoji = new Label
        {
            Text      = icono,
            Font      = new Font("Segoe UI Emoji", 18F),
            ForeColor = azulPrincipal,
            BackColor = Color.Transparent,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelIcono.Tag = azulClaro; // color de fondo inicial
        panelIcono.Controls.Add(lblEmoji);

        // ─── Título y descripción ─────────────────────────────────────────
        var lblTitulo = new Label
        {
            Text      = titulo,
            Font      = new Font("Segoe UI", 11.5F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Location  = new Point(72, 10),
            AutoSize  = true
        };

        var lblDescripcion = new Label
        {
            Text      = descripcion,
            Font      = new Font("Segoe UI", 10F),
            ForeColor = texto,
            Location  = new Point(72, 38),
            AutoSize  = true
        };

        // ─── Flecha ───────────────────────────────────────────────────────
        var flecha = new Label
        {
            Text      = "›",
            Font      = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = azulOscuro,
            Anchor    = AnchorStyles.Top | AnchorStyles.Right,
            Location  = new Point(recurso.Width - 36, 18),
            Size      = new Size(28, 36)
        };

        bool isHovered = false;

        // ─── Pintar borde y barra lateral del recurso ─────────────────────
        recurso.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            DibujarBordeRedondo(e.Graphics, recurso.ClientRectangle, 8,
                recurso.BackColor, isHovered ? azulPrincipal : borde);
            using var barBrush = new SolidBrush(isHovered ? azulPrincipal : Color.FromArgb(170, 190, 220));
            e.Graphics.FillRectangle(barBrush, 4, 12, 4, recurso.Height - 24);
        };

        // ─── Hover: entrar ────────────────────────────────────────────────
        Action enterAction = () =>
        {
            if (isHovered) return;
            isHovered = true;
            recurso.BackColor     = Color.FromArgb(242, 248, 255);
            panelIcono.Tag        = azulPrincipal;   // nuevo color de fondo
            lblEmoji.ForeColor    = Color.White;
            lblTitulo.ForeColor   = azulPrincipal;
            flecha.ForeColor      = azulPrincipal;
            flecha.Left           = recurso.Width - 32;
            panelIcono.Invalidate();
            recurso.Invalidate();
        };

        // ─── Hover: salir ─────────────────────────────────────────────────
        Action leaveAction = () =>
        {
            Point pos = recurso.PointToClient(Cursor.Position);
            if (recurso.ClientRectangle.Contains(pos)) return;
            isHovered = false;
            recurso.BackColor   = Color.White;
            panelIcono.Tag      = azulClaro;
            lblEmoji.ForeColor  = azulPrincipal;
            lblTitulo.ForeColor = azulOscuro;
            flecha.ForeColor    = azulOscuro;
            flecha.Left         = recurso.Width - 36;
            panelIcono.Invalidate();
            recurso.Invalidate();
        };

        recurso.MouseEnter   += (s, e) => enterAction();
        recurso.MouseLeave   += (s, e) => leaveAction();

        recurso.Resize += (s, e) =>
        {
            flecha.Left = recurso.Width - (isHovered ? 32 : 36);
            lblTitulo.MaximumSize     = new Size(recurso.Width - 120, 0);
            lblDescripcion.MaximumSize = new Size(recurso.Width - 120, 0);
            recurso.Invalidate();
        };

        foreach (Control ctrl in new Control[] { panelIcono, lblEmoji, lblTitulo, lblDescripcion, flecha })
        {
            ctrl.MouseEnter += (s, e) => enterAction();
            ctrl.MouseLeave += (s, e) => leaveAction();
            ctrl.Click      += (s, e) => accion();
        }
        recurso.Click += (s, e) => accion();

        recurso.Controls.AddRange(new Control[] { panelIcono, lblTitulo, lblDescripcion, flecha });
        return recurso;
    }

    // Helper local: GraphicsPath sin dependencia del método estático privado
    private static GraphicsPath DibujarRectRedondo(Rectangle rect, int radio)
    {
        int d = radio * 2;
        var path = new GraphicsPath();
        path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }


    private void DescargarManual()
    {
        string ruta = Path.Combine(Application.StartupPath, "Recursos", "manual_usuario.pdf");
        if (File.Exists(ruta))
        {
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            return;
        }

        MostrarModalEstilizado(
            "⇩  Descargar Manual",
            "El manual de usuario aún no ha sido agregado al sistema.",
            "Cuando lo tengas disponible, coloca el archivo manual_usuario.pdf dentro de la carpeta Recursos junto al ejecutable del sistema.",
            "Entendido");
    }

    private void MostrarRecurso(string titulo, string mensaje)
    {
        // Determinar ícono y color de acento según el título
        string icono = titulo.Contains("Guía") ? "📖" : titulo.Contains("Preguntas") ? "❓" : "📥";
        Color acento = titulo.Contains("Guía") ? Color.FromArgb(32, 91, 187)
                     : titulo.Contains("Preguntas") ? Color.FromArgb(130, 60, 200)
                     : Color.FromArgb(0, 148, 120);

        // Convertir líneas del mensaje en ítems de lista
        var lineas = mensaje.Split('\n');
        MostrarModalEstilizado(icono + "  " + titulo, null, mensaje, "Cerrar", acento);
    }

    private void MostrarModalEstilizado(string titulo, string? subtitulo, string contenido, string textoBoton, Color? colorAcento = null)
    {
        var acento = colorAcento ?? azulPrincipal;

        // Separar emoji y texto del título
        string[] partesTitulo = titulo.Split(new[] { "  " }, 2, StringSplitOptions.None);
        string emojiHeader = partesTitulo.Length > 1 ? partesTitulo[0].Trim() : "📋";
        string textoTitulo  = partesTitulo.Length > 1 ? partesTitulo[1].Trim() : titulo;

        // Pre-calcular alto necesario según líneas de contenido
        var lineas = contenido.Split('\n');
        int lineCount = lineas.Count(l => !string.IsNullOrWhiteSpace(l));
        int estimatedBodyHeight = 24 + (lineCount * 34) + 100;
        int dialogHeight = 130 + Math.Max(220, Math.Min(500, estimatedBodyHeight));

        var dialog = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition   = FormStartPosition.CenterParent,
            Size            = new Size(640, dialogHeight),
            BackColor       = Color.White,
            ShowInTaskbar   = false,
            MinimumSize     = new Size(640, 320)
        };
        UiAssets.RedondearControl(dialog, 16);
        dialog.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(195, 215, 235));
            e.Graphics.DrawRectangle(pen, 0, 0, dialog.Width - 1, dialog.Height - 1);
        };

        // ── Header ──────────────────────────────────────────────────────────
        var header = new Panel
        {
            BackColor = acento,
            Dock      = DockStyle.Top,
            Height    = 130
        };
        header.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path  = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, header.Width, header.Height + 20), 16);
            using var brush = new SolidBrush(acento);
            e.Graphics.FillPath(brush, path);
            // Círculos decorativos translúcidos
            using var deco = new SolidBrush(Color.FromArgb(25, 255, 255, 255));
            e.Graphics.FillEllipse(deco, header.Width - 130, -50, 160, 160);
            e.Graphics.FillEllipse(deco, header.Width - 60,  40, 100, 100);
            e.Graphics.FillEllipse(deco, -30, -20, 100, 100);
        };

        // Caja redondeada para el emoji (fondo semitransparente blanco)
        var cajaEmoji = new Panel
        {
            BackColor = Color.FromArgb(45, 255, 255, 255),
            Location  = new Point(24, 20),
            Size      = new Size(84, 84)
        };
        UiAssets.RedondearControl(cajaEmoji, 18);
        cajaEmoji.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var br   = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, cajaEmoji.Width - 1, cajaEmoji.Height - 1), 18);
            e.Graphics.FillPath(br, path);
        };
        var lblEmoji = new Label
        {
            Text      = emojiHeader,
            Font      = new Font("Segoe UI Emoji", 36F),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Dock      = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        cajaEmoji.Controls.Add(lblEmoji);

        // Bloque de texto (título + subtítulo) a la derecha del emoji
        int textoX = 124;
        int textoMaxW = 640 - textoX - 56; // dejar espacio para ✕

        var lblTitulo = new Label
        {
            Text        = textoTitulo,
            Font        = new Font("Segoe UI", 17F, FontStyle.Bold),
            ForeColor   = Color.White,
            BackColor   = Color.Transparent,
            Location    = new Point(textoX, 18),
            AutoSize    = true,
            MaximumSize = new Size(textoMaxW, 0)
        };

        var lblSub = new Label
        {
            Text        = subtitulo ?? "GALAB · Sistema de Gestión de Incidencias",
            Font        = new Font("Segoe UI", 9.5F),
            ForeColor   = Color.FromArgb(215, 238, 255),
            BackColor   = Color.Transparent,
            AutoSize    = true,
            MaximumSize = new Size(textoMaxW, 0)
        };
        // Posicionar subtítulo justo debajo del título (calculado después de layout)
        lblTitulo.SizeChanged += (s, e) =>
            lblSub.Location = new Point(textoX, lblTitulo.Bottom + 4);
        lblSub.Location = new Point(textoX, lblTitulo.Bottom + 4);

        // Botón ✕
        var btnX = new Button
        {
            Text      = "✕",
            Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            FlatStyle = FlatStyle.Flat,
            Cursor    = Cursors.Hand,
            Size      = new Size(38, 38),
            Location  = new Point(640 - 50, 10)
        };
        btnX.FlatAppearance.BorderSize = 0;
        btnX.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
        btnX.Click += (s, e) => dialog.Close();

        header.Controls.AddRange(new Control[] { cajaEmoji, lblTitulo, lblSub, btnX });

        // ── Cuerpo ───────────────────────────────────────────────────────────
        var cuerpo = new Panel
        {
            BackColor  = Color.White,
            Dock       = DockStyle.Fill,
            AutoScroll = true,
            Padding    = new Padding(32, 22, 32, 24)
        };

        int yLinea = 22;
        int maxAncho = 550;

        foreach (var linea in lineas)
        {
            string t = linea.Trim();
            if (string.IsNullOrWhiteSpace(t)) { yLinea += 10; continue; }

            bool esNumero   = t.Length > 2 && t[1] == '.' && char.IsDigit(t[0]);
            bool esPregunta = t.StartsWith("¿");

            if (esNumero)
            {
                // Burbuja numerada
                var burbuja = new Label
                {
                    Text      = t[0].ToString(),
                    Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = acento,
                    Location  = new Point(32, yLinea + 3),
                    Size      = new Size(26, 26),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                UiAssets.RedondearControl(burbuja, 13);

                var lbl = new Label
                {
                    Text        = t.Substring(3).Trim(),
                    Font        = new Font("Segoe UI", 11F),
                    ForeColor   = texto,
                    Location    = new Point(68, yLinea),
                    AutoSize    = true,
                    MaximumSize = new Size(maxAncho - 40, 0)
                };
                cuerpo.Controls.AddRange(new Control[] { burbuja, lbl });
                yLinea += 36;
            }
            else if (esPregunta)
            {
                // Separador visual antes de la pregunta (no en la primera)
                if (yLinea > 30)
                {
                    var sep = new Panel
                    {
                        BackColor = Color.FromArgb(228, 236, 248),
                        Location  = new Point(32, yLinea),
                        Size      = new Size(maxAncho, 1)
                    };
                    cuerpo.Controls.Add(sep);
                    yLinea += 12;
                }

                var lbl = new Label
                {
                    Text        = t,
                    Font        = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                    ForeColor   = acento,
                    Location    = new Point(32, yLinea),
                    AutoSize    = true,
                    MaximumSize = new Size(maxAncho, 0)
                };
                cuerpo.Controls.Add(lbl);
                yLinea += 32;
            }
            else
            {
                var lbl = new Label
                {
                    Text        = t,
                    Font        = new Font("Segoe UI", 11F),
                    ForeColor   = Color.FromArgb(55, 75, 105),
                    Location    = new Point(40, yLinea),
                    AutoSize    = true,
                    MaximumSize = new Size(maxAncho - 10, 0)
                };
                cuerpo.Controls.Add(lbl);
                yLinea += 30;
            }
        }

        // Recalcular alto del diálogo según contenido real
        int totalH = 130 + yLinea + 100;
        dialog.Height = Math.Max(320, Math.Min(680, totalH));

        // Línea separadora
        var lineaSep = new Panel
        {
            BackColor = Color.FromArgb(228, 236, 248),
            Location  = new Point(0, dialog.Height - 130 - 80),
            Size      = new Size(dialog.Width, 1)
        };

        // ── Botón cerrar ─────────────────────────────────────────────────────
        var btnCerrar = new Button
        {
            Text      = textoBoton,
            Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = acento,
            FlatStyle = FlatStyle.Flat,
            Cursor    = Cursors.Hand,
            Size      = new Size(180, 48),
            Location  = new Point((640 - 180) / 2, dialog.Height - 130 - 68)
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click      += (s, e) => dialog.Close();
        btnCerrar.MouseEnter += (s, e) => btnCerrar.BackColor = Color.FromArgb(
            Math.Max(0, acento.R - 28), Math.Max(0, acento.G - 22), Math.Max(0, acento.B - 22));
        btnCerrar.MouseLeave += (s, e) => btnCerrar.BackColor = acento;
        UiAssets.RedondearControl(btnCerrar, 12);

        cuerpo.Controls.AddRange(new Control[] { lineaSep, btnCerrar });
        dialog.Controls.AddRange(new Control[] { cuerpo, header });
        dialog.ShowDialog(this);
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
