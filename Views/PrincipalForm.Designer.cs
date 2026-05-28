namespace Proyecto_GALAB.Views;

partial class PrincipalForm
{
    private System.ComponentModel.IContainer components = null!;

    private Label      lblApartado;
    private Panel      panelContenido;
    private Panel      panelHeader;
    private PictureBox picLogoGalab;
    private PictureBox picLogoInstituto;
    private Label      lblInstituto;
    private Label      lblBienvenido;
    private Label      lblDescripcion;
    private Button     btnPerfil;
    private Button     btnGestion;
    private Button     btnAyuda;
    private Button     btnContacto;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        Text            = "GALAB - Apartado Principal";
        Size            = new Size(860, 600);
        MinimumSize     = new Size(700, 500);
        StartPosition   = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox     = true;
        BackColor       = Color.White;
        Font            = new Font("Segoe UI", 10F);

        // ── Etiqueta superior ────────────────────────────────────
        lblApartado = new Label
        {
            Text      = "APARTADO PRINCIPAL",
            Left      = 0, Top = 0, Height = 28,
            Font      = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(100, 100, 100),
            BackColor = Color.White,
            Padding   = new Padding(8, 6, 0, 0),
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── Panel contenido principal ────────────────────────────
        panelContenido = new Panel
        {
            Left        = 10, Top = 28,
            BackColor   = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor      = AnchorStyles.Top | AnchorStyles.Bottom
                        | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── Header azul ──────────────────────────────────────────
        panelHeader = new Panel
        {
            Left      = 0, Top = 0,
            Height    = 90,
            BackColor = Color.FromArgb(173, 216, 230),
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        picLogoGalab = new PictureBox
        {
            Left      = 10, Top = 8,
            Width     = 100, Height = 75,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left
        };
        picLogoGalab.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // Fondo azul claro del logo
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 230, 245)), 0, 0, picLogoGalab.Width, picLogoGalab.Height);
            using var f = new Font("Segoe UI", 9F, FontStyle.Bold);
            e.Graphics.DrawString("⚙ GALAB", f, new SolidBrush(Color.FromArgb(30,30,80)), 10, 28);
        };

        picLogoInstituto = new PictureBox
        {
            Top       = 8,
            Width     = 70, Height = 70,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor    = AnchorStyles.Top | AnchorStyles.Right,
            Image = Image.FromFile(@"C:\Users\LENOVO\Pictures\descargar.jpg")
        };
        picLogoInstituto.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(200,220,180)), 0, 0, 68, 68);
            e.Graphics.DrawEllipse(new Pen(Color.Transparent), 0, 0, 68, 68);
            using var f = new Font("Segoe UI", 6F);
            e.Graphics.DrawString("TecNM", f, Brushes.DarkGreen, 22, 28);
        };

        lblInstituto = new Label
        {
            Text      = "Instituto Tecnológico Superior de San Miguel el Grande",
            Top       = 20,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            AutoSize  = false, Width = 200, Height = 50,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor    = AnchorStyles.Top | AnchorStyles.Right
        };

        panelHeader.Controls.AddRange(new Control[]
        {
            picLogoGalab, picLogoInstituto, lblInstituto
        });

        // ── Bienvenido ───────────────────────────────────────────
        lblBienvenido = new Label
        {
            Text      = "BIENVENIDO",
            Font      = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 20, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        lblDescripcion = new Label
        {
            Text      = "GALAB es un sistema web para registrar y dar seguimiento a incidencias en\nlaboratorios de cómputo de manera rápida y organizada.",
            Font      = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(80, 80, 80),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── Botones de menú ──────────────────────────────────────
        btnPerfil  = MkMenuBtn("👤  PERFIL");
        btnGestion = MkMenuBtn("☰  GESTIÓN DE\n    INCIDENCIAS");
        btnAyuda   = MkMenuBtn("🌐  AYUDA");
        btnContacto= MkMenuBtn("📞  CONTACTO");

        btnPerfil.Click   += btnPerfil_Click;
        btnGestion.Click  += btnGestion_Click;
        btnAyuda.Click    += btnAyuda_Click;
        btnContacto.Click += btnContacto_Click;

        panelContenido.Controls.AddRange(new Control[]
        {
            panelHeader, lblBienvenido, lblDescripcion,
            btnPerfil, btnGestion, btnAyuda, btnContacto
        });

        Controls.AddRange(new Control[] { lblApartado, panelContenido });

        this.Resize += (s, e) => AjustarLayout();
        this.Load   += (s, e) => AjustarLayout();
    }

    private void AjustarLayout()
    {
        int cw = ClientSize.Width - 20;
        int ch = ClientSize.Height - 28;

        lblApartado.Width    = ClientSize.Width;
        panelContenido.Width = cw;
        panelContenido.Height= ch;

        // Header
        panelHeader.Width = cw - 2;
        picLogoInstituto.Left = cw - 90;
        lblInstituto.Left     = cw - 300;

        // Bienvenido
        lblBienvenido.Left  = 0;    lblBienvenido.Top  = 108;
        lblBienvenido.Width = cw - 2; lblBienvenido.Height = 50;

        lblDescripcion.Left  = 20;  lblDescripcion.Top  = 168;
        lblDescripcion.Width = cw - 40; lblDescripcion.Height = 50;

        // Fila 1 de botones (3 botones centrados)
        int bw = 180, bh = 80, gap = 20;
        int totalW3 = bw * 3 + gap * 2;
        int startX  = (cw - totalW3) / 2;
        int row1Y   = 250;

        btnPerfil.Left  = startX;           btnPerfil.Top  = row1Y;
        btnGestion.Left = startX + bw + gap; btnGestion.Top = row1Y;
        btnAyuda.Left   = startX + (bw+gap)*2; btnAyuda.Top = row1Y;

        btnPerfil.Width = btnGestion.Width = btnAyuda.Width = bw;
        btnPerfil.Height= btnGestion.Height= btnAyuda.Height= bh;

        // Fila 2 (1 botón centrado)
        int row2Y = row1Y + bh + gap;
        btnContacto.Left   = (cw - bw) / 2;
        btnContacto.Top    = row2Y;
        btnContacto.Width  = bw;
        btnContacto.Height = bh;
    }

    private static Button MkMenuBtn(string text) => new Button
    {
        Text      = text,
        BackColor = Color.FromArgb(210, 235, 250),
        ForeColor = Color.FromArgb(30, 30, 30),
        FlatStyle = FlatStyle.Flat,
        Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
        Cursor    = Cursors.Hand,
        TextAlign = ContentAlignment.MiddleCenter
    };
}
