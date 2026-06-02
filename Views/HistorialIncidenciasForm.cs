using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

public class HistorialIncidenciasForm : Form
{
    private DataGridView grid = null!;

    public HistorialIncidenciasForm()
    {
        Text = "GALAB - Historial de incidencias";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        UiAssets.PrepararPantallaCompleta(this);
        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        var header = CrearHeader();
        var sidebar = CrearSidebar();
        var contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2025 GALAB - Instituto Tecnológico Superior de San Miguel el Grande",
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
        iconoGalab.Paint += (s, e) => DibujarLogoGalab(e.Graphics);

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

        var picLogoInstituto = new PictureBox
        {
            Size = new Size(86, 76),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = UiAssets.CargarLogoInstitucion(),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Transparent
        };

        var lblInstituto = new Label
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
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias   ›", y, true, () => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, false, () => UiAssets.AbrirCerrandoActual(this, new PerfilForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        var btnCerrar = new Button
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
        btnCerrar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCerrar.FlatAppearance.BorderSize = 1;
        btnCerrar.Click += (s, e) => UiAssets.CerrarSesion(this);
        panel.Resize += (s, e) => btnCerrar.Top = panel.Height - 78;
        panel.Controls.Add(btnCerrar);
        UiAssets.RedondearControl(btnCerrar, 8);

        return panel;
    }

    private Panel CrearContenido()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo
        };

        var titulo = new Label
        {
            Text = "Historial de incidencias",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(40, 28),
            Size = new Size(520, 42)
        };

        var card = new Panel
        {
            BackColor = Color.White,
            Location = new Point(40, 92),
            Size = new Size(1140, 600)
        };
        card.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawPath(pen, path);
        };

        var encabezado = new Panel
        {
            BackColor = Color.White,
            Dock = DockStyle.Top,
            Height = 44
        };
        var lblRecientes = new Label
        {
            Text = "Incidencias recientes",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(33, 41, 58),
            AutoSize = true,
            Location = new Point(16, 11)
        };
        var lblVerTodas = new Label
        {
            Text = "Ver todas >",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            AutoSize = true,
            Cursor = Cursors.Hand
        };
        encabezado.Resize += (s, e) =>
        {
            lblVerTodas.Left = encabezado.Width - lblVerTodas.Width - 18;
            lblVerTodas.Top = 12;
        };
        encabezado.Controls.Add(lblRecientes);
        encabezado.Controls.Add(lblVerTodas);

        grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeight = 50,
            RowTemplate = { Height = 46 },
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        // Azul más suave para cabecera y selección.
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 248, 255);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 38, 54);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        grid.EnableHeadersVisualStyles = false;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(45, 52, 67);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(246, 250, 255);
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(35, 45, 65);
        grid.GridColor = Color.FromArgb(229, 234, 242);

        grid.Columns.Add("Folio", "Folio");
        grid.Columns.Add("Equipo", "Pc/portátil afectado");
        grid.Columns.Add("Descripcion", "Descripción");
        grid.Columns.Add("Estado", "Estado");
        grid.Columns.Add("Fecha", "Fecha");
        grid.Columns.Add("Acciones", "Acciones");
        grid.Columns["Folio"]!.FillWeight = 120;
        grid.Columns["Equipo"]!.FillWeight = 140;
        grid.Columns["Descripcion"]!.FillWeight = 210;
        grid.Columns["Estado"]!.FillWeight = 95;
        grid.Columns["Fecha"]!.FillWeight = 95;
        grid.Columns["Acciones"]!.FillWeight = 90;

        grid.Rows.Add("Sin incidencias", "-", "No hay incidencias registradas", "-", "-", "—");

        card.Controls.Add(grid);
        card.Controls.Add(encabezado);
        panel.Controls.Add(titulo);
        panel.Controls.Add(card);

        panel.Resize += (s, e) =>
        {
            int maxWidth = Math.Max(900, panel.ClientSize.Width - 60);
            int contentWidth = Math.Min(1220, maxWidth);
            int startX = (panel.ClientSize.Width - contentWidth) / 2;

            titulo.Left = startX;
            card.Left = startX;
            card.Width = contentWidth;
            card.Height = Math.Max(500, panel.ClientSize.Height - 120);
        };

        return panel;
    }

    private static void DibujarLogoGalab(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var azul = new SolidBrush(UiAssets.AzulPrincipal);
        using var claro = new SolidBrush(Color.White);
        g.FillEllipse(azul, 7, 6, 52, 52);
        g.FillEllipse(claro, 18, 17, 30, 30);
        g.DrawEllipse(new Pen(UiAssets.AzulOscuro, 4), 23, 22, 20, 20);
    }
}
