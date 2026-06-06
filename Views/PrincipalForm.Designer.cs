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
            Width     = 150, Height = 75,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left,
            Image     = UiAssets.CargarLogoGalab()
        };

        picLogoInstituto = new PictureBox
        {
            Top       = 8,
            Width     = 70, Height = 70,
            SizeMode  = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Anchor    = AnchorStyles.Top | AnchorStyles.Right,
            Image     = UiAssets.CargarLogoInstitucion()
        };
        picLogoInstituto.Paint += (s, e) =>
        {
            if (picLogoInstituto.Image != null)
                return;

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
            AutoSize  = true,
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
            Font      = new Font("Segoe UI", 30F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 20, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        lblDescripcion = new Label
        {
            Text      = "GALAB es un sistema web para registrar y dar seguimiento a incidencias en\nlaboratorios de cómputo de manera rápida y organizada.",
            Font      = new Font("Segoe UI", 14F),
            ForeColor = Color.FromArgb(35, 45, 65),
            BackColor = Color.White,
            TextAlign = ContentAlignment.TopCenter,
            AutoSize  = false,
            UseMnemonic = false,
            Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // ── Botones de menú ──────────────────────────────────────
        btnPerfil  = MkMenuBtn("👤  PERFIL");
        btnGestion = MkMenuBtn("☰  GESTIÓN DE\n    INCIDENCIAS");
        btnAyuda   = MkMenuBtn("🌐  AYUDA");
        btnContacto= MkMenuBtn("📞  CONTACTO");

        UiAssets.RedondearControl(btnPerfil, 10);
        UiAssets.RedondearControl(btnGestion, 10);
        UiAssets.RedondearControl(btnAyuda, 10);
        UiAssets.RedondearControl(btnContacto, 10);

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
        if (WindowState == FormWindowState.Minimized) return;
        int cw = ClientSize.Width - 20;
        int ch = ClientSize.Height - 28;

        lblApartado.Width    = ClientSize.Width;
        panelContenido.Width = cw;
        panelContenido.Height= ch;

        // Header
        panelHeader.Width = cw - 2;
        picLogoInstituto.Left = cw - 90;
        lblInstituto.Left     = picLogoInstituto.Left - lblInstituto.Width - 10;

        // Escalado dinámico de botones de menú: tres arriba y uno abajo.
        int bw = Math.Min(370, Math.Max(250, cw / 3));
        int bh = Math.Min(160, Math.Max(120, ch / 6));
        int gap = Math.Min(38, Math.Max(22, cw / 50));

        // Ajustar fuentes proporcionalmente
        float fontSize = bw > 300 ? 15F : 13F;
        var btnFont = new Font("Segoe UI", fontSize, FontStyle.Bold);
        btnPerfil.Font = btnFont;
        btnGestion.Font = btnFont;
        btnAyuda.Font = btnFont;
        btnContacto.Font = btnFont;

        int totalW3 = bw * 3 + gap * 2;
        if (totalW3 > cw - 60)
        {
            bw = Math.Max(190, (cw - 60 - gap * 2) / 3);
            bh = Math.Max(90, bh);
        }

        int startX = (cw - (bw * 3 + gap * 2)) / 2;
        int gridH = bh * 2 + gap;
        int row1Y = Math.Max(270, 245 + (ch - 245 - gridH) / 2);
        int row2Y = row1Y + bh + gap;

        // Bienvenido y descripción: descripción centrada entre título y botones
        lblBienvenido.Left  = 0;
        lblBienvenido.Top   = 108;
        lblBienvenido.Width = cw - 2;
        lblBienvenido.Height = 80;
        lblBienvenido.Font = new Font("Segoe UI", 36F, FontStyle.Bold);

        lblDescripcion.Left = 20;
        lblDescripcion.Width = cw - 40;
        int descripcionHeight = Math.Max(66, TextRenderer.MeasureText(
            lblDescripcion.Text,
            lblDescripcion.Font,
            new Size(Math.Max(200, cw - 40), int.MaxValue),
            TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height + 4);
        lblDescripcion.Height = descripcionHeight;
        int espacioSuperior = lblBienvenido.Bottom + 12;
        int espacioInferior = row1Y - 12;
        lblDescripcion.Top = Math.Max(espacioSuperior, espacioSuperior + Math.Max(0, (espacioInferior - espacioSuperior - descripcionHeight) / 2));

        btnPerfil.SetBounds(startX, row1Y, bw, bh);
        btnGestion.SetBounds(startX + bw + gap, row1Y, bw, bh);
        btnAyuda.SetBounds(startX + (bw + gap) * 2, row1Y, bw, bh);
        btnContacto.SetBounds((cw - bw) / 2, row2Y, bw, bh);
    }

    private static Button MkMenuBtn(string text) => new Button
    {
        Text      = text,
        BackColor = Color.FromArgb(210, 235, 250),
        ForeColor = Color.FromArgb(30, 30, 30),
        FlatStyle = FlatStyle.Flat,
        Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
        Cursor    = Cursors.Hand,
        TextAlign = ContentAlignment.MiddleCenter
    };

}
