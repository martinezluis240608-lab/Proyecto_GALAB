using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_GALAB.Views
{
    public partial class PerfilForm : Form
    {
        // ===== COLORES =====
        private readonly Color azulPrincipal = Color.FromArgb(32, 91, 187);
        private readonly Color azulClaro = Color.FromArgb(235, 242, 250);
        private readonly Color fondoGeneral = Color.FromArgb(245, 248, 252);

        // Datos temporales del usuario.
        // Cuando conectes la base de datos, reemplaza estos valores con los datos
        // obtenidos del usuario que inició sesión.
        private const string NombreUsuarioActual = "Nombre del usuario";
        private const string CorreoUsuarioActual = "correo@institucion.edu.mx";
        private const string RolUsuarioActual = "Rol del usuario";
        private const string CarreraUsuarioActual = "Carrera del usuario";

        public PerfilForm()
        {
            InitializeComponent();

            ConfigurarFormulario();
            CrearInterfaz();
        }

        // ===== CONFIGURACIÓN =====
        private void ConfigurarFormulario()
        {
            this.Text = "Perfil - GALAB";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = fondoGeneral;

            // IMPORTANTE PARA SCROLL HORIZONTAL Y VERTICAL
            this.AutoScroll = true;
            this.HorizontalScroll.Enabled = true;
            this.HorizontalScroll.Visible = true;
            this.VerticalScroll.Enabled = true;
            this.VerticalScroll.Visible = true;
        }

        // ===== INTERFAZ =====
        private void CrearInterfaz()
        {
            // ===== HEADER =====
            Panel header = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = azulClaro
            };

            this.Controls.Add(header);

            Label tituloSistema = new Label()
            {
                Text = "GALAB - Perfil de Usuario",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = azulPrincipal,
                AutoSize = true,
                Location = new Point(40, 25)
            };

            header.Controls.Add(tituloSistema);

            PictureBox logoInstituto = new PictureBox()
            {
                Image = UiAssets.CargarLogoInstitucion(),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = new Size(70, 64),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Label lblInstituto = new Label()
            {
                Text = "Instituto Tecnológico Superior\nde San Miguel el Grande",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Size = new Size(260, 46),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            header.Resize += (s, e) =>
            {
                logoInstituto.Left = header.Width - 350;
                logoInstituto.Top = 13;
                lblInstituto.Left = header.Width - 270;
                lblInstituto.Top = 22;
            };
            header.Controls.AddRange(new Control[] { logoInstituto, lblInstituto });

            // ===== SIDEBAR =====
            Panel sidebar = new Panel()
            {
                Width = 290,
                Dock = DockStyle.Left,
                BackColor = Color.White
            };

            this.Controls.Add(sidebar);

            int y = 56;

            sidebar.Controls.Add(UiAssets.CrearBotonSidebar("⌂", "Inicio", y, false, () => UiAssets.AbrirCerrandoActual(this, new PrincipalForm())));
            y += 70;

            sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias", y, false, () => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm())));
            y += 70;

            // BOTÓN ACTIVO
            sidebar.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, true, null));

            y += 70;

            sidebar.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
            y += 70;

            sidebar.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

            // ===== BOTÓN CERRAR SESIÓN =====
            Button btnCerrar = new Button()
            {
                Text = "↩ Cerrar sesión",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = azulPrincipal,
                Size = new Size(210, 48),
                Location = new Point(40, 760),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            sidebar.Resize += (s, e) => btnCerrar.Top = sidebar.Height - 78;

            btnCerrar.FlatAppearance.BorderColor = azulPrincipal;
            btnCerrar.FlatAppearance.BorderSize = 1;

            btnCerrar.Click += (s, e) =>
            {
                UiAssets.CerrarSesion(this);
            };

            sidebar.Controls.Add(btnCerrar);
            UiAssets.RedondearControl(btnCerrar, 8);

            // ===== CONTENIDO =====
            Panel contenido = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = fondoGeneral,
                AutoScroll = true
            };

            // IMPORTANTE PARA VISUALIZAR TODO
            contenido.AutoScrollMinSize = new Size(960, 660);

            this.Controls.Add(contenido);

            // ===== TARJETA PERFIL =====
            Panel card = new Panel()
            {
                BackColor = Color.White,
                Size = new Size(920, 620),
                Location = new Point(40, 40)
            };

            contenido.Controls.Add(card);
            UiAssets.RedondearControl(card, 12);

            contenido.Resize += (s, e) =>
            {
                card.Left = Math.Max(40, (contenido.ClientSize.Width - card.Width) / 2);
                card.Top = Math.Max(40, (contenido.ClientSize.Height - card.Height) / 2);
            };

            Label tituloPerfil = new Label()
            {
                Text = "Información del usuario",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = azulPrincipal,
                AutoSize = true,
                Location = new Point(40, 40)
            };

            card.Controls.Add(tituloPerfil);

            // FOTO
            PictureBox foto = new PictureBox()
            {
                Size = new Size(170, 170),
                Location = new Point(60, 120),
                BackColor = azulClaro,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            card.Controls.Add(foto);

            // DATOS
            AgregarCampo(card, "Nombre completo:", NombreUsuarioActual, 120);
            AgregarCampo(card, "Correo electrónico:", CorreoUsuarioActual, 200);
            AgregarCampo(card, "Rol:", RolUsuarioActual, 280);
            AgregarCampo(card, "Carrera:", CarreraUsuarioActual, 360);

            // BOTÓN EDITAR
            Button btnEditar = new Button()
            {
                Text = "✏ Editar perfil",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = azulPrincipal,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 48),
                Location = new Point(300, 460),
                Cursor = Cursors.Hand
            };

            btnEditar.FlatAppearance.BorderSize = 0;

            card.Controls.Add(btnEditar);
            UiAssets.RedondearControl(btnEditar, 8);
        }

        // ===== BOTONES MENU =====
        private void AgregarBoton(Panel parent, string texto, int y)
        {
            Button btn = new Button()
            {
                Text = texto,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Black,
                Size = new Size(258, 54),
                Location = new Point(16, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) =>
            {
                if (texto.Contains("Inicio"))
                {
                    UiAssets.AbrirCerrandoActual(this, new PrincipalForm());
                }
                else if (texto.Contains("Gestión"))
                {
                    UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm());
                }
                else if (texto.Contains("Contacto"))
                {
                    UiAssets.AbrirCerrandoActual(this, new ContactoForm());
                }
                else if (texto.Contains("Ayuda"))
                {
                    UiAssets.AbrirCerrandoActual(this, new AyudaForm());
                }
            };

            parent.Controls.Add(btn);
        }

        // ===== CAMPOS =====
        private void AgregarCampo(
            Panel parent,
            string titulo,
            string valor,
            int y)
        {
            Label lblTitulo = new Label()
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(300, y)
            };

            parent.Controls.Add(lblTitulo);

            TextBox txt = new TextBox()
            {
                Text = valor,
                Font = new Font("Segoe UI", 10.5F),
                Size = new Size(560, 32),
                Location = new Point(300, y + 26),
                ReadOnly = true
            };

            parent.Controls.Add(txt);
        }

        private void PerfilForm_Load(object sender, EventArgs e)
        {

        }
    }
}
