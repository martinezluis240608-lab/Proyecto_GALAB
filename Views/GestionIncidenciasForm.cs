using System.Drawing.Drawing2D;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views;

public class GestionIncidenciasForm : Form
{
    private Panel header = null!;
    private Panel sidebar = null!;
    private Panel contenido = null!;
    private Label lblInstituto = null!;
    private PictureBox picLogoInstituto = null!;
    private Button btnCerrarSesion = null!;

    public GestionIncidenciasForm()
    {
        Text = "GALAB - Gestion de Incidencias";
        Size = new Size(1280, 760);
        MinimumSize = new Size(1050, 680);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        header = CrearHeader();
        sidebar = CrearSidebar();
        contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2024 GALAB - Instituto Tecnológico Superior de San Miguel el Grande",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
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
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 126,
            BackColor = UiAssets.AzulClaro
        };

        var iconoGalab = new Panel
        {
            Location = new Point(24, 28),
            Size = new Size(72, 62),
            BackColor = Color.Transparent
        };
        iconoGalab.Paint += (s, e) => DibujarLogoGalab(e.Graphics, iconoGalab.ClientRectangle);

        var galab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(110, 22),
            Size = new Size(210, 42)
        };

        var subtitulo = new Label
        {
            Text = "Sistema de Gestión\nde Incidencias",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(114, 66),
            Size = new Size(230, 54)
        };

        picLogoInstituto = new PictureBox
        {
            Size = new Size(86, 76),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = UiAssets.CargarLogoInstitucion(),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Transparent
        };

        lblInstituto = new Label
        {
            Text = "Instituto Tecnológico\nSuperior de San Miguel\nel Grande",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Size = new Size(300, 76)
        };

        panel.Resize += (s, e) =>
        {
            picLogoInstituto.Left = panel.Width - 390;
            picLogoInstituto.Top = 24;
            lblInstituto.Left = panel.Width - 292;
            lblInstituto.Top = 28;
        };

        panel.Controls.AddRange(new Control[] { iconoGalab, galab, subtitulo, picLogoInstituto, lblInstituto });
        return panel;
    }

    private Panel CrearSidebar()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 290,
            BackColor = Color.White
        };

        int y = 56;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("⌂", "Inicio", y, false, () => UiAssets.AbrirCerrandoActual(this, new PrincipalForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias   ›", y, true, null));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, false, () => UiAssets.AbrirCerrandoActual(this, new PerfilForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        btnCerrarSesion = new Button
        {
            Text = "↪  Cerrar sesión",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 48),
            Location = new Point(40, 575),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        btnCerrarSesion.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCerrarSesion.FlatAppearance.BorderSize = 1;
        btnCerrarSesion.Click += (s, e) => UiAssets.CerrarSesion(this);
        panel.Resize += (s, e) => btnCerrarSesion.Top = panel.Height - 78;
        panel.Controls.Add(btnCerrarSesion);

        return panel;
    }

    private Panel CrearContenido()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            AutoScroll = true
        };

        var titulo = new Label
        {
            Text = "GESTION DE INCIDENCIAS",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(0, 28),
            Size = new Size(860, 48),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var lblGalab = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(48, 115),
            Size = new Size(260, 42)
        };

        var descripcion = new Label
        {
            Text = "Sistema de registro y seguimiento de\nincidencias en laboratorios de computo",
            Font = new Font("Segoe UI", 16F),
            ForeColor = UiAssets.AzulOscuro,
            BackColor = Color.White,
            Location = new Point(52, 172),
            Size = new Size(520, 86),
            AutoSize = false,
            TextAlign = ContentAlignment.TopLeft
        };

        var computadora = new Panel
        {
            Location = new Point(640, 106),
            Size = new Size(250, 185),
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        computadora.Paint += (s, e) => DibujarComputadoraGalab(e.Graphics, computadora.ClientRectangle);

        var resumen = new IncidenciaEstadisticasService().ObtenerResumen();
        var cardActivas = CrearCardEstado("Incidencias activas", resumen.Activas.ToString(), Color.FromArgb(210, 30, 55), "▯", 76, 340);
        var cardProceso = CrearCardEstado("En proceso", resumen.EnProceso.ToString(), Color.FromArgb(235, 145, 12), "◷", 382, 340);
        var cardResueltas = CrearCardEstado("Resueltas", resumen.Resueltas.ToString(), Color.FromArgb(10, 170, 55), "✓", 686, 340);

        var registrar = new Button
        {
            Text = "⊕   Registrar nueva incidencia",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(238, 0, 100),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(68, 510),
            Size = new Size(410, 74),
            TextAlign = ContentAlignment.MiddleCenter
        };
        registrar.FlatAppearance.BorderSize = 0;
        registrar.Click += (s, e) => UiAssets.AbrirCerrandoActual(this, new IncidenciaForm());

        var historial = new Button
        {
            Text = "Consultar historial",
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.Black,
            BackColor = Color.FromArgb(223, 242, 255),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(530, 510),
            Size = new Size(390, 74),
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(0)
        };
        historial.FlatAppearance.BorderColor = Color.FromArgb(58, 174, 220);
        historial.FlatAppearance.BorderSize = 1;
        historial.Click += (s, e) => UiAssets.AbrirCerrandoActual(this, new HistorialIncidenciasForm());

        UiAssets.RedondearControl(registrar, 12);
        UiAssets.RedondearControl(historial, 12);

        panel.Resize += (s, e) =>
        {
            int clientW = panel.ClientSize.Width;
            int clientH = panel.ClientSize.Height;

            int ancho = Math.Max(940, clientW);
            int alto = Math.Max(600, clientH);
            titulo.Width = ancho;

            int layoutWidth = Math.Min(1120, Math.Max(900, ancho - 110));
            int startX = (ancho - layoutWidth) / 2;

            // Alinea cabecera con el layout
            lblGalab.Left = startX + 4;
            descripcion.Left = startX + 8;
            computadora.Left = startX + layoutWidth - computadora.Width;

            // Centrado de tarjetas
            int cardW = Math.Min(310, Math.Max(250, (layoutWidth - 80) / 3));
            int cardH = Math.Min(130, Math.Max(105, alto / 7));
            cardActivas.Size = cardProceso.Size = cardResueltas.Size = new Size(cardW, cardH);

            int totalCardsWidth = cardW * 3 + 40 * 2;
            int startCardsX = startX + (layoutWidth - totalCardsWidth) / 2;
            cardActivas.Left = startCardsX;
            cardProceso.Left = cardActivas.Right + 40;
            cardResueltas.Left = cardProceso.Right + 40;

            // Centrado de botones e igualación de tamaños
            int registrarW = Math.Min(470, Math.Max(410, layoutWidth / 2 - 32));
            int historialW = registrarW;

            int contentGroupHeight;
            int startGroupY;

            if (ancho < 920)
            {
                // Apilado vertical si es angosto para que no se corten
                int activeBtnW = Math.Min(400, ancho - 100);
                registrar.Width = activeBtnW;
                historial.Width = activeBtnW;
                registrar.Left = (ancho - activeBtnW) / 2;
                historial.Left = registrar.Left;

                contentGroupHeight = cardH + 30 + 74 + 16 + 74;
                int remainingHeight = alto - 280;
                startGroupY = Math.Max(320, 270 + (remainingHeight - contentGroupHeight) / 2);

                cardActivas.Top = cardProceso.Top = cardResueltas.Top = startGroupY;
                registrar.Top = startGroupY + cardH + 30;
                historial.Top = registrar.Top + 74 + 16;
            }
            else
            {
                // Lado a lado
                registrar.Width = registrarW;
                historial.Width = historialW;

                int totalBtnsWidth = registrarW + 48 + historialW;
                int startBtnsX = (ancho - totalBtnsWidth) / 2;

                registrar.Left = startBtnsX;
                historial.Left = registrar.Right + 48;

                contentGroupHeight = cardH + 78 + 74;
                int remainingHeight = alto - 280;
                startGroupY = Math.Max(320, 270 + (remainingHeight - contentGroupHeight) / 2);

                cardActivas.Top = cardProceso.Top = cardResueltas.Top = startGroupY;
                registrar.Top = historial.Top = startGroupY + cardH + 78;
            }

            panel.AutoScrollMinSize = new Size(980, 660);
        };

        panel.Controls.AddRange(new Control[]
        {
            titulo, lblGalab, computadora,
            cardActivas, cardProceso, cardResueltas,
            registrar, historial
        });
        panel.Controls.Add(descripcion);
        descripcion.BringToFront();

        return panel;
    }

    private Panel CrearCardEstado(string titulo, string total, Color color, string icono, int x, int y)
    {
        var panel = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(285, 120),
            BackColor = Color.White
        };
        panel.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 10);
            using var pen = new Pen(color, 2);
            e.Graphics.DrawPath(pen, path);
        };

        panel.Controls.Add(new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.Black,
            Location = new Point(22, 8),
            Size = new Size(220, 34),
            Tag = "titulo"
        });
        panel.Controls.Add(new Label
        {
            Text = total,
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),
            ForeColor = color,
            Location = new Point(56, 52),
            Size = new Size(90, 48),
            TextAlign = ContentAlignment.MiddleCenter,
            Tag = "total"
        });
        panel.Controls.Add(new Label
        {
            Text = icono,
            Font = new Font("Segoe UI Symbol", 38F, FontStyle.Bold),
            ForeColor = color == Color.FromArgb(235, 145, 12) ? Color.Black : color,
            Location = new Point(190, 44),
            Size = new Size(78, 58),
            TextAlign = ContentAlignment.MiddleCenter,
            Tag = "icono"
        });

        panel.Resize += (s, e) =>
        {
            foreach (Control control in panel.Controls)
            {
                if (control.Tag?.ToString() == "titulo")
                {
                    control.Left = 22;
                    control.Top = 10;
                    control.Width = panel.Width - 44;
                    control.Height = 36;
                }
                else if (control.Tag?.ToString() == "total")
                {
                    control.Left = 36;
                    control.Top = panel.Height - 68;
                    control.Width = 100;
                    control.Height = 52;
                }
                else if (control.Tag?.ToString() == "icono")
                {
                    control.Left = panel.Width - 92;
                    control.Top = panel.Height - 74;
                    control.Width = 76;
                    control.Height = 62;
                }
            }
        };

        return panel;
    }

    private void DibujarLogoGalab(Graphics g, Rectangle rect)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var azul = new SolidBrush(UiAssets.AzulPrincipal);
        using var claro = new SolidBrush(Color.White);
        g.FillEllipse(azul, 7, 6, 52, 52);
        g.FillEllipse(claro, 18, 17, 30, 30);
        g.DrawEllipse(new Pen(UiAssets.AzulOscuro, 4), 23, 22, 20, 20);
    }

    private void DibujarComputadoraGalab(Graphics g, Rectangle rect)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var azul = new SolidBrush(Color.FromArgb(40, 155, 225));
        using var azulOscuro = new Pen(UiAssets.AzulOscuro, 4);
        g.FillRectangle(Brushes.White, 34, 16, 170, 96);
        g.DrawRectangle(azulOscuro, 34, 16, 170, 96);
        g.DrawString("GALAB", new Font("Segoe UI", 20F, FontStyle.Bold), new SolidBrush(UiAssets.AzulOscuro), 82, 48);
        g.FillEllipse(azul, 58, 48, 38, 38);
        g.FillRectangle(azul, 106, 115, 28, 20);
        g.FillRectangle(azul, 70, 136, 110, 18);
        g.FillEllipse(azul, 202, 138, 32, 30);
    }
}
