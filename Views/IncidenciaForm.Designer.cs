namespace Proyecto_GALAB.Views;

partial class IncidenciaForm
{
    private System.ComponentModel.IContainer components = null!;

    private Panel panelHeader;
    private Label lblHeaderTitle;
    private Label lblTitulo;
    private Label lblQuienReporta;
    private TextBox txtQuienReporta;
    private Label lblTipo;
    private ComboBox cmbTipo;
    private Label lblNombreEquipo;
    private TextBox txtNombreEquipo;
    private Label lblFechaHora;
    private DateTimePicker dtpFecha;
    private NumericUpDown nudHora;
    private Label lblDos;
    private NumericUpDown nudMinuto;
    private Label lblDescripcion;
    private TextBox txtDescripcion;
    private Label lblEvidencias;
    private Button btnAdjuntar;
    private Label lblEvidencia;
    private Button btnEnviar;

    // Panel contenedor principal con scroll
    private Panel mainContainer;
    private TableLayoutPanel mainLayout;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // ── Form ────────────────────────────────────────────────
        this.Text = "REGISTRO DE NUEVA INCIDENCIA";
        this.Size = new Size(950, 700);
        this.MinimumSize = new Size(600, 550);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.MaximizeBox = true;
        this.MinimizeBox = true;
        this.BackColor = Color.FromArgb(173, 216, 230);
        this.Font = new Font("Segoe UI", 10F);
        this.AutoScaleMode = AutoScaleMode.Font;

        // ── Header (Dock Top para que siempre se vea) ───────────
        panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = Color.FromArgb(240, 235, 245)
        };

        panelHeader.Paint += (s, e) =>
        {
            int r = 20;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, r, r, 180, 90);
            path.AddArc(panelHeader.Width - r, 0, r, r, 270, 90);
            path.AddArc(panelHeader.Width - r, panelHeader.Height - r, r, r, 0, 90);
            path.AddArc(0, panelHeader.Height - r, r, r, 90, 90);
            path.CloseAllFigures();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(240, 235, 245)), path);
        };

        // Logo más grande
        lblHeaderTitle = new Label
        {
            Text = "GALAB",
            Font = new Font("Segoe UI", 32F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Image = Image.FromFile(@"C:\Users\LENOVO\Documents\dibujos\logo GALAB.png"),
            ImageAlign = ContentAlignment.MiddleLeft
        };
        panelHeader.Controls.Add(lblHeaderTitle);

        // ── Contenedor principal con scroll automático ──────────
        mainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.Transparent
        };

        // ── Layout principal con TableLayoutPanel ───────────────
        mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(25, 15, 25, 25),
            ColumnCount = 2,
            RowCount = 12,
            BackColor = Color.Transparent,
            MinimumSize = new Size(500, 0)
        };

        // Configurar columnas (una fija para etiquetas, una flexible para campos)
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Configurar filas con altura automática
        for (int i = 0; i < 12; i++)
        {
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        // ── Fila 0: Título ─────────────────────────────────────
        lblTitulo = new Label
        {
            Text = "REGISTRO DE NUEVA INCIDENCIA",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill,
            Height = 45
        };
        mainLayout.Controls.Add(lblTitulo, 0, 0);
        mainLayout.SetColumnSpan(lblTitulo, 2);

        // ── Fila 1: Espaciador ─────────────────────────────────
        Panel spacer1 = new Panel { Height = 10 };
        mainLayout.Controls.Add(spacer1, 0, 1);
        mainLayout.SetColumnSpan(spacer1, 2);

        // ── Fila 2: Quien reporta ───────────────────────────────
        lblQuienReporta = MkLabel("QUIEN REPORTA");
        lblQuienReporta.Dock = DockStyle.Fill;
        lblQuienReporta.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblQuienReporta, 0, 2);

        txtQuienReporta = MkTextBox("Ingrese nombre completo");
        txtQuienReporta.Dock = DockStyle.Fill;
        txtQuienReporta.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        mainLayout.Controls.Add(txtQuienReporta, 1, 2);

        // ── Fila 3: Tipo de incidencia ──────────────────────────
        lblTipo = MkLabel("TIPO DE INCIDENCIA");
        lblTipo.Dock = DockStyle.Fill;
        lblTipo.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblTipo, 0, 3);

        cmbTipo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F),
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            Height = 40
        };
        cmbTipo.Items.AddRange(new object[]
        {
            "INFRAESTRUCTURA", "SOFTWARE", "HARDWARE", "RED", "OTRO"
        });
        cmbTipo.SelectedIndex = 0;
        mainLayout.Controls.Add(cmbTipo, 1, 3);

        // ── Fila 4: Nombre del equipo ───────────────────────────
        lblNombreEquipo = MkLabel("NOMBRE DEL EQUIPO");
        lblNombreEquipo.Dock = DockStyle.Fill;
        lblNombreEquipo.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblNombreEquipo, 0, 4);

        txtNombreEquipo = MkTextBox("Ingrese el nombre del equipo");
        txtNombreEquipo.Dock = DockStyle.Fill;
        txtNombreEquipo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        mainLayout.Controls.Add(txtNombreEquipo, 1, 4);

        // ── Fila 5: Fecha y hora ────────────────────────────────
        lblFechaHora = MkLabel("FECHA Y HORA");
        lblFechaHora.Dock = DockStyle.Fill;
        lblFechaHora.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblFechaHora, 0, 5);

        // Panel para organizar fecha y hora horizontalmente
        Panel fechaHoraPanel = new Panel
        {
            Height = 40,
            Dock = DockStyle.Fill
        };

        dtpFecha = new DateTimePicker
        {
            Left = 0,
            Top = 2,
            Width = 180,
            Height = 36,
            Format = DateTimePickerFormat.Short,
            Font = new Font("Segoe UI", 10F),
            Anchor = AnchorStyles.Left
        };

        nudHora = new NumericUpDown
        {
            Left = 190,
            Top = 2,
            Width = 65,
            Height = 36,
            Minimum = 0,
            Maximum = 23,
            Value = DateTime.Now.Hour,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center,
            Anchor = AnchorStyles.Left
        };

        Label lblDosLocal = new Label
        {
            Text = ":",
            Left = 260,
            Top = 2,
            Width = 15,
            Height = 36,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(30, 30, 30),
            Anchor = AnchorStyles.Left
        };

        nudMinuto = new NumericUpDown
        {
            Left = 280,
            Top = 2,
            Width = 65,
            Height = 36,
            Minimum = 0,
            Maximum = 59,
            Value = DateTime.Now.Minute,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            TextAlign = HorizontalAlignment.Center,
            Anchor = AnchorStyles.Left
        };

        fechaHoraPanel.Controls.AddRange(new Control[] { dtpFecha, nudHora, lblDosLocal, nudMinuto });
        mainLayout.Controls.Add(fechaHoraPanel, 1, 5);

        // ── Fila 6: Espaciador ──────────────────────────────────
        Panel spacer2 = new Panel { Height = 10 };
        mainLayout.Controls.Add(spacer2, 0, 6);
        mainLayout.SetColumnSpan(spacer2, 2);

        // ── Fila 7: Descripción ─────────────────────────────────
        lblDescripcion = MkLabel("DESCRIPCION");
        lblDescripcion.Dock = DockStyle.Fill;
        lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblDescripcion, 0, 7);

        txtDescripcion = new TextBox
        {
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            PlaceholderText = "Ingrese la descripción detallada de la incidencia...",
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 10F),
            BackColor = Color.White,
            Height = 100,
            Dock = DockStyle.Fill,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
        };
        mainLayout.Controls.Add(txtDescripcion, 1, 7);

        // ── Fila 8: Espaciador ──────────────────────────────────
        Panel spacer3 = new Panel { Height = 10 };
        mainLayout.Controls.Add(spacer3, 0, 8);
        mainLayout.SetColumnSpan(spacer3, 2);

        // ── Fila 9: Evidencias ──────────────────────────────────
        lblEvidencias = MkLabel("EVIDENCIAS");
        lblEvidencias.Dock = DockStyle.Fill;
        lblEvidencias.TextAlign = ContentAlignment.MiddleLeft;
        mainLayout.Controls.Add(lblEvidencias, 0, 9);

        Panel evidenciaPanel = new Panel
        {
            Height = 50,
            Dock = DockStyle.Fill
        };

        btnAdjuntar = new Button
        {
            Text = "📎 ADJUNTAR ARCHIVO",
            Left = 0,
            Top = 5,
            Width = 160,
            Height = 40,
            BackColor = Color.FromArgb(200, 40, 40),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Left
        };
        btnAdjuntar.FlatAppearance.BorderSize = 0;
        btnAdjuntar.Click += btnAdjuntar_Click;

        lblEvidencia = new Label
        {
            Left = 170,
            Top = 15,
            Width = 350,
            Height = 24,
            ForeColor = Color.FromArgb(60, 60, 60),
            Font = new Font("Segoe UI", 9F),
            Text = "Ningún archivo seleccionado",
            Anchor = AnchorStyles.Left | AnchorStyles.Right
        };

        evidenciaPanel.Controls.Add(btnAdjuntar);
        evidenciaPanel.Controls.Add(lblEvidencia);
        mainLayout.Controls.Add(evidenciaPanel, 1, 9);

        // ── Fila 10: Espaciador ─────────────────────────────────
        Panel spacer4 = new Panel { Height = 15 };
        mainLayout.Controls.Add(spacer4, 0, 10);
        mainLayout.SetColumnSpan(spacer4, 2);

        // ── Fila 11: Botón Enviar ───────────────────────────────
        Panel buttonPanel = new Panel
        {
            Height = 60,
            Dock = DockStyle.Fill
        };

        btnEnviar = new Button
        {
            Text = "✓ ENVIAR REPORTE",
            Width = 240,
            Height = 45,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.None
        };
        btnEnviar.FlatAppearance.BorderSize = 0;
        btnEnviar.Click += btnEnviar_Click;

        // Centrar botón horizontalmente
        btnEnviar.Location = new Point((buttonPanel.Width - btnEnviar.Width) / 2, 8);
        buttonPanel.Controls.Add(btnEnviar);
        buttonPanel.Resize += (s, e) =>
        {
            btnEnviar.Location = new Point((buttonPanel.Width - btnEnviar.Width) / 2, 8);
        };

        mainLayout.Controls.Add(buttonPanel, 0, 11);
        mainLayout.SetColumnSpan(buttonPanel, 2);

        // Agregar layout al contenedor principal
        mainContainer.Controls.Add(mainLayout);

        // Agregar todo al formulario
        this.Controls.Add(mainContainer);
        this.Controls.Add(panelHeader);

        // Asegurar que el layout se ajuste al ancho del contenedor
        this.Resize += (s, e) =>
        {
            if (mainContainer.Width > 0)
            {
                mainLayout.Width = mainContainer.Width - 20;
            }
        };
    }

    // ── Helpers actualizados ────────────────────────────────────
    private static Label MkLabel(string text) => new Label
    {
        Text = text,
        AutoSize = true,
        MinimumSize = new Size(150, 35),
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = Color.FromArgb(50, 50, 50),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private static TextBox MkTextBox(string placeholder) => new TextBox
    {
        PlaceholderText = placeholder,
        Height = 40,
        BorderStyle = BorderStyle.FixedSingle,
        Font = new Font("Segoe UI", 10F),
        BackColor = Color.White,
        Margin = new Padding(0, 3, 0, 3)
    };
}