using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views;

public partial class PerfilForm : Form
{
    private PictureBox picFoto = null!;
    private TextBox txtNombre = null!;
    private TextBox txtCurp = null!;
    private TextBox txtFechaNacimiento = null!;
    private TextBox txtGenero = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtControl = null!;
    private TextBox txtEstatus = null!;
    private TextBox txtSemestre = null!;
    private TextBox txtCarrera = null!;
    private TextBox txtGrupo = null!;
    private TextBox txtCalle = null!;
    private TextBox txtColonia = null!;
    private TextBox txtCodigoPostal = null!;
    private TextBox txtMunicipio = null!;
    private TextBox txtEstado = null!;
    private Button btnEditar = null!;
    private Button btnGuardar = null!;
    private Button btnCancelar = null!;
    private Button btnCambiarFoto = null!;
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
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo
        };

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

        var sidebar = CrearSidebar();
        var header = CrearHeader();
        var contenido = CrearContenido();

        // Nest header and contenido into panelDerecho
        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        // Add controls to main Form in correct docking Z-order
        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
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
            AutoSize = true
        };
        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            logo.Left = panel.Width - 390;
            logo.Top = 24;
            instituto.Left = logo.Right + 12;
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

        var cardGeneral = CrearSeccion("Informacion general", 24, 22, 380, 600);
        var cardEscolar = CrearSeccion("Informacion escolar", 426, 22, 716, 320);
        var cardContacto = CrearSeccion("Informacion de contacto", 426, 280, 716, 320);

        picFoto = new PictureBox
        {
            Size = new Size(140, 140),
            Location = new Point(120, 62),
            BackColor = Color.FromArgb(227, 238, 255),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom
        };
        UiAssets.RedondearControl(picFoto, 70);
        cardGeneral.Controls.Add(picFoto);

        btnCambiarFoto = new Button
        {
            Text = "Cambiar foto",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(140, 34),
            Location = new Point(120, 210),
            Cursor = Cursors.Hand,
            Visible = false
        };
        btnCambiarFoto.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCambiarFoto.FlatAppearance.BorderSize = 1;
        btnCambiarFoto.Click += (_, _) => CambiarFoto();
        UiAssets.RedondearControl(btnCambiarFoto, 8);
        cardGeneral.Controls.Add(btnCambiarFoto);

        txtNombre = CrearFila(cardGeneral, "Nombre", 268);
        txtCurp = CrearFila(cardGeneral, "CURP", 316);
        txtFechaNacimiento = CrearFila(cardGeneral, "Fecha de nacimiento", 364);
        txtGenero = CrearFila(cardGeneral, "Genero", 412);
        txtTelefono = CrearFila(cardGeneral, "Telefono", 460);
        txtCorreo = CrearFila(cardGeneral, "Correo electronico", 508);

        txtControl = CrearFila(cardEscolar, "No. de control", 56);
        txtEstatus = CrearFila(cardEscolar, "Estatus", 104);
        txtSemestre = CrearFila(cardEscolar, "Semestre", 152);
        txtCarrera = CrearFila(cardEscolar, "Carrera", 200);
        txtGrupo = CrearFila(cardEscolar, "Grupo", 248);

        txtCalle = CrearFila(cardContacto, "Calle y numero", 56);
        txtColonia = CrearFila(cardContacto, "Colonia", 104);
        txtCodigoPostal = CrearFila(cardContacto, "Codigo postal", 152);
        txtMunicipio = CrearFila(cardContacto, "Municipio", 200);
        txtEstado = CrearFila(cardContacto, "Estado", 248);

        txtNombre.KeyPress += SoloLetras_KeyPress;
        txtGenero.KeyPress += SoloLetras_KeyPress;
        txtCarrera.KeyPress += SoloLetras_KeyPress;
        txtMunicipio.KeyPress += SoloLetras_KeyPress;
        txtEstado.KeyPress += SoloLetras_KeyPress;

        txtTelefono.KeyPress += SoloNumeros_KeyPress;
        txtControl.KeyPress += SoloNumeros_KeyPress;
        txtSemestre.KeyPress += SoloNumeros_KeyPress;
        txtCodigoPostal.KeyPress += SoloNumeros_KeyPress;

        panel.Controls.AddRange(new Control[] { cardGeneral, cardEscolar, cardContacto });

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50),
            Location = new Point(930, 656)
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
            Location = new Point(824, 656),
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
            Location = new Point(992, 656),
            Visible = false
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => CancelarEdicion();
        UiAssets.RedondearControl(btnCancelar, 10);

        panel.Controls.AddRange(new Control[] { btnEditar, btnGuardar, btnCancelar });

        cardGeneral.Resize += (s, e) =>
        {
            picFoto.Left = (cardGeneral.Width - picFoto.Width) / 2;
            btnCambiarFoto.Left = (cardGeneral.Width - btnCambiarFoto.Width) / 2;
        };

        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int clientW = panel.ClientSize.Width;
            int clientH = panel.ClientSize.Height;

            int padding = 24;
            int gap = 20;

            int availableW = clientW - (padding * 2) - gap;
            if (availableW < 900) availableW = 900;

            int generalW = (int)(availableW * 0.35);
            int escolarW = availableW - generalW;

            generalW = Math.Max(350, Math.Min(450, generalW));
            escolarW = availableW - generalW;

            int startX = padding;
            cardGeneral.Location = new Point(startX, padding);
            cardGeneral.Size = new Size(generalW, 600);

            int col2X = startX + generalW + gap;
            cardEscolar.Location = new Point(col2X, padding);
            cardEscolar.Size = new Size(escolarW, 320);

            cardContacto.Location = new Point(col2X, cardEscolar.Bottom + gap);
            cardContacto.Size = new Size(escolarW, 320);

            btnEditar.Location = new Point(col2X + escolarW - btnEditar.Width, cardContacto.Bottom + gap);
            btnGuardar.Location = new Point(col2X + escolarW - (btnGuardar.Width * 2 + 10), cardContacto.Bottom + gap);
            btnCancelar.Location = new Point(col2X + escolarW - btnCancelar.Width, cardContacto.Bottom + gap);

            panel.AutoScrollMinSize = new Size(col2X + escolarW + padding, btnEditar.Bottom + padding);
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
            Font = new Font("Segoe UI", 12F),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, y + 2),
            Size = new Size(220, 30)
        };
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 12F),
            ForeColor = Color.FromArgb(35, 45, 65),
            Location = new Point(250, y + 2),
            Size = new Size(parent.Width - 270, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
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
        btnCambiarFoto.Visible = true;
    }

    private void GuardarEdicion()
    {
        string email = txtCorreo.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var perfil = PerfilUsuarioStore.Obtener();
        perfil.NombreCompleto = txtNombre.Text.Trim();
        perfil.Curp = txtCurp.Text.Trim();
        perfil.FechaNacimiento = txtFechaNacimiento.Text.Trim();
        perfil.Genero = txtGenero.Text.Trim();
        perfil.Telefono = txtTelefono.Text.Trim();
        perfil.Correo = txtCorreo.Text.Trim();
        perfil.ControlNumber = txtControl.Text.Trim();
        perfil.Rol = txtEstatus.Text.Trim();
        perfil.Semestre = txtSemestre.Text.Trim();
        perfil.Carrera = txtCarrera.Text.Trim();
        perfil.Grupo = txtGrupo.Text.Trim();
        perfil.Calle = txtCalle.Text.Trim();
        perfil.Colonia = txtColonia.Text.Trim();
        perfil.CodigoPostal = txtCodigoPostal.Text.Trim();
        perfil.Municipio = txtMunicipio.Text.Trim();
        perfil.Estado = txtEstado.Text.Trim();
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
        btnCambiarFoto.Visible = false;
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
            txtNombre, txtCurp, txtFechaNacimiento, txtGenero, txtTelefono, txtCorreo,
            txtControl, txtEstatus, txtSemestre, txtCarrera, txtGrupo,
            txtCalle, txtColonia, txtCodigoPostal, txtMunicipio, txtEstado
        };
    }

    private void CargarPerfilEnVista()
    {
        var perfil = PerfilUsuarioStore.Obtener();
        txtNombre.Text = perfil.NombreCompleto;
        txtCurp.Text = perfil.Curp;
        txtFechaNacimiento.Text = perfil.FechaNacimiento;
        txtGenero.Text = perfil.Genero;
        txtTelefono.Text = perfil.Telefono;
        txtCorreo.Text = perfil.Correo;
        txtControl.Text = perfil.ControlNumber;
        txtEstatus.Text = perfil.Rol;
        txtSemestre.Text = perfil.Semestre;
        txtCarrera.Text = perfil.Carrera;
        txtGrupo.Text = perfil.Grupo;
        txtCalle.Text = perfil.Calle;
        txtColonia.Text = perfil.Colonia;
        txtCodigoPostal.Text = perfil.CodigoPostal;
        txtMunicipio.Text = perfil.Municipio;
        txtEstado.Text = perfil.Estado;
        SetReadOnlyCampos(true);
        CargarFoto(picFoto, perfil.RutaFotoPerfil);
    }

    private void CambiarFoto()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Seleccionar foto de perfil",
            Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.webp"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        var perfil = PerfilUsuarioStore.Obtener();
        perfil.RutaFotoPerfil = dialog.FileName;
        PerfilUsuarioStore.Guardar(perfil);
        CargarFoto(picFoto, perfil.RutaFotoPerfil);
    }

    private static void CargarFoto(PictureBox pictureBox, string ruta)
    {
        if (!string.IsNullOrWhiteSpace(ruta) && File.Exists(ruta))
        {
            using var img = Image.FromFile(ruta);
            pictureBox.Image = new Bitmap(img);
            return;
        }

        pictureBox.Image = null;
    }

    private void SoloNumeros_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void SoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
        {
            e.Handled = true;
        }
    }
}
