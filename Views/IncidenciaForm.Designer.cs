using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

partial class IncidenciaForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel header;
    private Panel contenido;
    private Label lblInstituto;
    private PictureBox picLogoInstituto;
    private Label txtTitulo;
    private TextBox txtQuienReporta;
    private ComboBox cmbTipo;
    private TextBox txtNombreEquipo;
    private DateTimePicker dtpFecha;
    private NumericUpDown nudHora;
    private NumericUpDown nudMinuto;
    private TextBox txtDescripcion;
    private TextBox txtEtiquetas;
    private Button btnAdjuntar;
    private Label lblEvidencia;
    private Button btnEnviar;
    private Button btnCancelar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Text = "GALAB - Registrar incidencia";
        Size = new Size(1280, 760);
        MinimumSize = new Size(1050, 680);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        AutoScaleMode = AutoScaleMode.Font;

        header = CrearHeader();
        contenido = CrearContenido();

        Controls.Add(contenido);
        Controls.Add(header);
    }

    private Panel CrearHeader()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 126,
            BackColor = UiAssets.AzulClaro
        };

        var picLogoGalab = new PictureBox
        {
            Image = UiAssets.CargarLogoGalab(),
            SizeMode = PictureBoxSizeMode.Zoom,
            Location = new Point(24, 20),
            Size = new Size(180, 76),
            BackColor = Color.Transparent
        };

        picLogoInstituto = new PictureBox
        {
            Size = new Size(84, 76),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = UiAssets.CargarLogoInstitucion(),
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        lblInstituto = new Label
        {
            Text = "Instituto Tecnológico\nSuperior de San Miguel\nel Grande",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            AutoSize = true
        };

        panel.Resize += (s, e) =>
        {
            picLogoInstituto.Left = panel.Width - 602;
            picLogoInstituto.Top = 22;
            lblInstituto.Left = picLogoInstituto.Right + 18;
            lblInstituto.Top = 28;
        };

        panel.Controls.AddRange(new Control[]
        {
            picLogoGalab, picLogoInstituto, lblInstituto
        });
        return panel;
    }

    private Panel CrearContenido()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo,
            AutoScroll = true
        };

        txtTitulo = new Label
        {
            Text = "REGISTRAR INCIDENCIA",
            Font = new Font("Segoe UI", 19F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(340, 24),
            Size = new Size(430, 42),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var subrayado = new Panel
        {
            BackColor = UiAssets.AzulPrincipal,
            Location = new Point(342, 68),
            Size = new Size(62, 5)
        };
        UiAssets.RedondearControl(subrayado, 3);

        var cardGeneral = CrearTarjeta(340, 96, 690, 480);
        CrearInfoGeneral(cardGeneral);

        var cardAdicional = CrearTarjeta(340, 594, 690, 282);
        CrearInfoAdicional(cardAdicional);

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(70, 78, 92),
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(340, 770),
            Size = new Size(122, 48)
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.AzulPrincipal;
        btnCancelar.FlatAppearance.BorderSize = 2;
        btnCancelar.Click += (s, e) => UiAssets.NavegarAGestionIncidencias(this);
        UiAssets.RedondearControl(btnCancelar, 8);

        btnEnviar = new Button
        {
            Text = "✈  Registrar Incidencia",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(572, 770),
            Size = new Size(238, 48)
        };
        btnEnviar.FlatAppearance.BorderSize = 0;
        btnEnviar.Click += btnEnviar_Click;
        UiAssets.RedondearControl(btnEnviar, 8);

        panel.Resize += (s, e) =>
        {
            int available = Math.Max(760, panel.ClientSize.Width - 330);
            int cardW = Math.Min(760, available - 90);
            int startX = 330 + Math.Max(36, (available - cardW) / 2);

            txtTitulo.Left = startX;
            subrayado.Left = startX + 2;
            cardGeneral.Left = startX;
            cardAdicional.Left = startX;
            cardGeneral.Width = cardW;
            cardAdicional.Width = cardW;

            AjustarTarjetaGeneral(cardGeneral);
            AjustarTarjetaAdicional(cardAdicional);

            btnCancelar.Left = startX;
            btnEnviar.Left = startX + 232;
            btnCancelar.Top = cardAdicional.Bottom + 22;
            btnEnviar.Top = btnCancelar.Top;
            panel.AutoScrollMinSize = new Size(1020, btnEnviar.Bottom + 32);
        };

        panel.Controls.AddRange(new Control[]
        {
            txtTitulo, subrayado, cardGeneral, cardAdicional, btnCancelar, btnEnviar
        });
        return panel;
    }

    private Panel CrearTarjeta(int x, int y, int width, int height)
    {
        var panel = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            BackColor = Color.White
        };
        panel.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 10);
            using var shadow = new SolidBrush(Color.FromArgb(18, 0, 0, 0));
            using var pen = new Pen(Color.FromArgb(226, 232, 241), 1);
            e.Graphics.FillPath(Brushes.White, path);
            e.Graphics.DrawPath(pen, path);
        };
        return panel;
    }

    private void CrearInfoGeneral(Panel card)
    {
        card.Controls.Add(CrearTituloSeccion("⌘", "Información general", 24, 24));

        card.Controls.Add(CrearEtiqueta("Título de la incidencia *", 24, 76));
        txtQuienReporta = CrearCaja("Ingresa un título breve y descriptivo", 24, 104, 642, 42);
        card.Controls.Add(txtQuienReporta);

        card.Controls.Add(CrearEtiqueta("Nombre del equipo / Área afectada *", 24, 168));
        txtNombreEquipo = CrearCaja("Ej: PC-LAB1-05 o Mesa 3", 24, 196, 642, 42);
        card.Controls.Add(txtNombreEquipo);

        card.Controls.Add(CrearEtiqueta("Tipo de incidencia *", 24, 252));
        cmbTipo = CrearCombo(24, 280, 642, new[] { "Selecciona un tipo de incidencia", "Hardware y software", "Infraestructura" });
        card.Controls.Add(cmbTipo);

        card.Controls.Add(CrearEtiqueta("Descripción *", 24, 336));
        txtDescripcion = new TextBox
        {
            PlaceholderText = "Describe la incidencia con el mayor detalle posible...",
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10.5F),
            Location = new Point(24, 364),
            Size = new Size(642, 78)
        };
        card.Controls.Add(txtDescripcion);

        card.Controls.Add(new Label
        {
            Text = "Mínimo 20 caracteres",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(82, 91, 112),
            Location = new Point(24, 444),
            Size = new Size(180, 22)
        });

        dtpFecha = new DateTimePicker { Value = DateTime.Now, Visible = false };
        nudHora = new NumericUpDown { Value = DateTime.Now.Hour, Maximum = 23, Visible = false };
        nudMinuto = new NumericUpDown { Value = DateTime.Now.Minute, Maximum = 59, Visible = false };
        card.Controls.AddRange(new Control[] { dtpFecha, nudHora, nudMinuto });
    }

    private void CrearInfoAdicional(Panel card)
    {
        card.Controls.Add(CrearTituloSeccion("⌕", "Información adicional", 24, 24));

        card.Controls.Add(CrearEtiqueta("Adjuntar archivos (opcional)", 24, 76));
        btnAdjuntar = new Button
        {
            Text = "☁   Arrastra y suelta archivos aquí\r\no selecciona archivos",
            Font = new Font("Segoe UI", 10F),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Location = new Point(24, 106),
            Size = new Size(642, 76),
            TextAlign = ContentAlignment.MiddleCenter
        };
        btnAdjuntar.FlatAppearance.BorderColor = Color.FromArgb(190, 202, 218);
        btnAdjuntar.FlatAppearance.BorderSize = 1;
        btnAdjuntar.Click += btnAdjuntar_Click;
        btnAdjuntar.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(185, 198, 216), 1) { DashStyle = DashStyle.Dash };
            e.Graphics.DrawRectangle(pen, 1, 1, btnAdjuntar.Width - 3, btnAdjuntar.Height - 3);
        };
        card.Controls.Add(btnAdjuntar);

        lblEvidencia = new Label
        {
            Text = "Ningún archivo seleccionado",
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = Color.FromArgb(90, 98, 116),
            Location = new Point(24, 184),
            Size = new Size(642, 22),
            TextAlign = ContentAlignment.MiddleCenter
        };
        card.Controls.Add(lblEvidencia);

        card.Controls.Add(CrearEtiqueta("Número de serie", 24, 210));
        txtEtiquetas = CrearCaja("Ej: SN-123456789", 24, 238, 642, 42);
        card.Controls.Add(txtEtiquetas);

        card.Controls.Add(new Label
        {
            Text = "Ingresa el número de serie del equipo afectado si lo conoces",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(82, 91, 112),
            Location = new Point(24, 284),
            Size = new Size(350, 22)
        });
    }

    private void AjustarTarjetaGeneral(Panel card)
    {
        int innerW = card.Width - 48;
        txtQuienReporta.Width = innerW;
        txtNombreEquipo.Width = innerW;
        cmbTipo.Width = innerW;
        txtDescripcion.Width = innerW;
    }

    private void AjustarTarjetaAdicional(Panel card)
    {
        int innerW = card.Width - 48;
        btnAdjuntar.Width = innerW;
        lblEvidencia.Width = innerW;
        txtEtiquetas.Width = innerW;
    }

    private Label CrearTituloSeccion(string icono, string texto, int x, int y) => new()
    {
        Text = $"{icono}   {texto}",
        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
        ForeColor = UiAssets.AzulPrincipal,
        Location = new Point(x, y),
        Size = new Size(360, 32),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private Label CrearEtiqueta(string texto, int x, int y) => new()
    {
        Text = texto,
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
        ForeColor = UiAssets.AzulOscuro,
        Location = new Point(x, y),
        Size = new Size(260, 24)
    };

    private TextBox CrearCaja(string placeholder, int x, int y, int width, int height) => new()
    {
        PlaceholderText = placeholder,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font("Segoe UI", 10.5F),
        Location = new Point(x, y),
        Size = new Size(width, height)
    };

    private ComboBox CrearCombo(int x, int y, int width, string[] opciones)
    {
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10.5F),
            Location = new Point(x, y),
            Size = new Size(width, 42)
        };
        combo.Items.AddRange(opciones);
        combo.SelectedIndex = 0;
        return combo;
    }
}
