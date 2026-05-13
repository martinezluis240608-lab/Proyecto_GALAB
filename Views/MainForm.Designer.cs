namespace Proyecto_GALAB.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null!;

        // Controles
        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Button btnSaludar;
        private Label lblResultado;
        private Panel panelHeader;
        private Panel panelBody;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Form ──────────────────────────────────────────────
            this.Text = "Mi Proyecto MVP";
            this.Size = new Size(480, 340);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 10F);

            // ── Panel Header ──────────────────────────────────────
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(37, 99, 235)   // azul
            };

            lblTitulo = new Label
            {
                Text = "🎓 Mi Proyecto MVP",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelHeader.Controls.Add(lblTitulo);

            // ── Panel Body ────────────────────────────────────────
            panelBody = new Panel
            {
                Left = 40,
                Top = 90,
                Width = 390,
                Height = 200,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            panelBody.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, panelBody.Width - 1, panelBody.Height - 1);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 220, 230)), rect);
            };

            lblNombre = new Label
            {
                Text = "Tu nombre:",
                Left = 20,
                Top = 25,
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 80)
            };

            txtNombre = new TextBox
            {
                Left = 20,
                Top = 50,
                Width = 350,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Escribe tu nombre aquí...",
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSaludar = new Button
            {
                Text = "Saludar",
                Left = 20,
                Top = 100,
                Width = 350,
                Height = 38,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSaludar.FlatAppearance.BorderSize = 0;
            btnSaludar.Click += btnSaludar_Click;

            lblResultado = new Label
            {
                Left = 20,
                Top = 152,
                Width = 350,
                AutoSize = false,
                Height = 36,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(22, 163, 74),   // verde
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = ""
            };

            panelBody.Controls.AddRange(new Control[]
            {
                lblNombre, txtNombre, btnSaludar, lblResultado
            });

            // ── Agregar al Form ───────────────────────────────────
            this.Controls.AddRange(new Control[] { panelHeader, panelBody });
        }
    }
}
