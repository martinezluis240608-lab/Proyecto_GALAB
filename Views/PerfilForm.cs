using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views;

public partial class PerfilForm : Form
{
    private TextBox txtIdAlumno = null!;
    private TextBox txtNombre = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtControl = null!;
    private TextBox txtUsuario = null!;
    private TextBox txtEstatus = null!;
    private TextBox txtSemestre = null!;
    private TextBox txtGrupo = null!;
    private TextBox txtNumeroAsiento = null!;
    private TextBox txtEstado = null!;
    private TextBox txtFechaRegistro = null!;
    private Button btnEditar = null!;
    private Button btnGuardar = null!;
    private Button btnCancelar = null!;
    private bool modoEdicion;
    private PerfilUsuario? respaldoPerfil;

    public PerfilForm()
    {
        InitializeComponent();
        UiAssets.PrepararPantallaCompleta(this);
        Text = "GALAB - Perfil";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        CrearInterfaz();
        CargarPerfilEnVista();
    }

    private void CrearInterfaz()
    {
        var header = CrearHeader();
        var sidebar = CrearSidebar();
        var contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2025 GALAB - Instituto Tecnologico Superior de San Miguel el Grande",
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

        var titulo = new Label
        {
            Text = "Instituto Tecnologico Superior de San Miguel el Grande",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(30, 34),
            AutoSize = true
        };

        var logo = new PictureBox
        {
            Size = new Size(86, 76),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = UiAssets.CargarLogoInstitucion(),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Transparent
        };
        var instituto = new Label
        {
            Text = "Instituto Tecnologico\nSuperior de San Miguel\nel Grande",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Size = new Size(300, 76)
        };
        panel.Resize += (s, e) =>
        {
            logo.Left = panel.Width - 390;
            logo.Top = 24;
            instituto.Left = panel.Width - 292;
            instituto.Top = 28;
        };

        panel.Controls.AddRange(new Control[] { titulo, logo, instituto });
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
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestion de incidencias", y, false, () => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, true, null));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesion",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 48),
            Location = new Point(40, 575),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        cerrar.FlatAppearance.BorderColor = UiAssets.Borde;
        cerrar.FlatAppearance.BorderSize = 1;
        cerrar.Click += (s, e) => UiAssets.CerrarSesion(this);
        panel.Resize += (s, e) => cerrar.Top = panel.Height - 78;
        panel.Controls.Add(cerrar);
        UiAssets.RedondearControl(cerrar, 8);

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

        var cardGeneral = CrearSeccion("Informacion general", 24, 22, 520, 220);
        var cardEscolar = CrearSeccion("Informacion escolar", 560, 22, 520, 220);
        var cardCuenta = CrearSeccion("Informacion de cuenta", 24, 260, 520, 260);

        txtNombre = CrearFila(cardGeneral, "Nombre", 62);
        txtCorreo = CrearFila(cardGeneral, "Correo electronico", 100);
        txtTelefono = CrearFila(cardGeneral, "Telefono", 138);

        txtControl = CrearFila(cardEscolar, "No. de control", 56);
        txtEstatus = CrearFila(cardEscolar, "Estatus", 92);
        txtSemestre = CrearFila(cardEscolar, "Semestre", 128);
        txtGrupo = CrearFila(cardEscolar, "Grupo", 164);

        txtIdAlumno = CrearFila(cardCuenta, "ID alumno", 56);
        txtUsuario = CrearFila(cardCuenta, "Usuario", 92);
        txtNumeroAsiento = CrearFila(cardCuenta, "Numero de asiento", 128);
        txtEstado = CrearFila(cardCuenta, "Estado", 164);
        txtFechaRegistro = CrearFila(cardCuenta, "Fecha de registro", 200);

        panel.Controls.AddRange(new Control[] { cardGeneral, cardEscolar, cardCuenta });

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50),
            Location = new Point(870, 560)
        };
        btnEditar.FlatAppearance.BorderSize = 0;
        btnEditar.Click += (_, _) => ActivarEdicion();
        UiAssets.RedondearControl(btnEditar, 10);

        btnGuardar = new Button
        {
            Text = "Guardar",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(34, 166, 88),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(160, 50),
            Location = new Point(650, 560),
            Visible = false
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += (_, _) => GuardarEdicion();
        UiAssets.RedondearControl(btnGuardar, 10);

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(160, 50),
            Location = new Point(820, 560),
            Visible = false
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => CancelarEdicion();
        UiAssets.RedondearControl(btnCancelar, 10);

        panel.Controls.AddRange(new Control[] { btnEditar, btnGuardar, btnCancelar });

        panel.Resize += (s, e) =>
        {
            int contentW = Math.Max(1160, panel.ClientSize.Width - 40);
            int startX = Math.Max(18, (panel.ClientSize.Width - contentW) / 2);

            cardGeneral.Left = startX;
            cardEscolar.Left = cardGeneral.Right + 22;
            cardCuenta.Left = startX;
            btnEditar.Left = cardEscolar.Right - btnEditar.Width;
            btnGuardar.Left = btnEditar.Left - btnGuardar.Width - 12;
            btnCancelar.Left = cardEscolar.Right - btnCancelar.Width;
            panel.AutoScrollMinSize = new Size(startX + 1148 + 30, 700);
        };

        return panel;
    }

    private static Panel CrearSeccion(string titulo, int x, int y, int w, int h)
    {
        var card = new Panel
        {
            BackColor = Color.White,
            Location = new Point(x, y),
            Size = new Size(w, h)
        };
        card.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(219, 227, 241)), 0, 0, card.Width - 1, card.Height - 1);
        };
        UiAssets.RedondearControl(card, 10);
        var encabezado = new Panel
        {
            Dock = DockStyle.Top,
            Height = 46,
            BackColor = Color.FromArgb(237, 244, 255)
        };
        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 9),
            AutoSize = true
        };
        encabezado.Controls.Add(lblTitulo);
        card.Controls.Add(encabezado);
        return card;
    }

    private TextBox CrearFila(Control parent, string etiqueta, int y)
    {
        var label = new Label
        {
            Text = $"{etiqueta}:",
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, y + 2),
            Size = new Size(220, 28)
        };
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(35, 45, 65),
            Location = new Point(250, y + 2),
            Size = new Size(parent.Width - 270, 28),
            TabStop = true
        };
        parent.Controls.Add(label);
        parent.Controls.Add(txt);
        return txt;
    }

    private void ActivarEdicion()
    {
        if (modoEdicion)
            return;

        respaldoPerfil = PerfilUsuarioStore.Obtener();
        modoEdicion = true;
        SetReadOnlyCampos(false);
        btnEditar.Visible = false;
        btnGuardar.Visible = true;
        btnCancelar.Visible = true;
    }

    private void GuardarEdicion()
    {
        var perfil = PerfilUsuarioStore.Obtener();
        perfil.NombreCompleto = txtNombre.Text.Trim();
        perfil.Telefono = txtTelefono.Text.Trim();
        perfil.Correo = txtCorreo.Text.Trim();
        perfil.ControlNumber = txtControl.Text.Trim();
        perfil.Rol = txtEstatus.Text.Trim();
        perfil.Semestre = txtSemestre.Text.Trim();
        perfil.Grupo = txtGrupo.Text.Trim();
        PerfilUsuarioStore.Guardar(perfil);
        FinalizarEdicion();
    }

    private void CancelarEdicion()
    {
        if (respaldoPerfil != null)
            PerfilUsuarioStore.Guardar(respaldoPerfil);
        CargarPerfilEnVista();
        FinalizarEdicion();
    }

    private void FinalizarEdicion()
    {
        modoEdicion = false;
        SetReadOnlyCampos(true);
        btnEditar.Visible = true;
        btnGuardar.Visible = false;
        btnCancelar.Visible = false;
    }

    private void SetReadOnlyCampos(bool readOnly)
    {
        foreach (var txt in ObtenerCampos())
        {
            txt.ReadOnly = readOnly;
            txt.BorderStyle = readOnly ? BorderStyle.None : BorderStyle.FixedSingle;
            txt.BackColor = readOnly ? Color.White : Color.FromArgb(250, 252, 255);
            txt.TabStop = !readOnly;
        }
    }

    private IEnumerable<TextBox> ObtenerCampos()
    {
        return new[]
        {
            txtNombre, txtTelefono, txtCorreo,
            txtControl, txtEstatus, txtSemestre, txtGrupo
        };
    }

    private void CargarPerfilEnVista()
    {
        var perfil = PerfilUsuarioStore.Obtener();
        txtNombre.Text = perfil.NombreCompleto;
        txtTelefono.Text = perfil.Telefono;
        txtCorreo.Text = perfil.Correo;
        txtControl.Text = perfil.ControlNumber;
        txtIdAlumno.Text = perfil.IdAlumno;
        txtUsuario.Text = string.IsNullOrWhiteSpace(perfil.Usuario) ? SesionActual.NombreUsuario : perfil.Usuario;
        txtEstatus.Text = perfil.Rol;
        txtSemestre.Text = perfil.Semestre;
        txtGrupo.Text = perfil.Grupo;
        txtNumeroAsiento.Text = perfil.NumeroAsiento;
        txtEstado.Text = perfil.Estado;
        txtFechaRegistro.Text = perfil.FechaRegistro;
        SetReadOnlyCampos(true);
    }
}
