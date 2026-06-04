using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

/// <summary>
/// Perfil del administrador, sin datos escolares ni campos personales no necesarios.
/// </summary>
public class AdminPerfilForm : Form
{
    private PictureBox picFoto = null!;
    private TextBox txtNombre = null!;
    private TextBox txtCurp = null!;
    private TextBox txtFechaNacimiento = null!;
    private TextBox txtGenero = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
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
    private PerfilAdministrador? respaldo;

    public AdminPerfilForm()
    {
        Text = "GALAB - Perfil administrador";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        UiAssets.PrepararPantallaCompleta(this);
        CrearInterfaz();
        CargarVista();
    }

    private void CrearInterfaz()
    {
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo
        };

        var header = AdminSidebar.CrearHeader();
        var sidebar = AdminSidebar.Crear(this, AdminModulo.Perfil);
        var contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2025 GALAB - Panel administrador",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        };

        // Nest header and contenido into panelDerecho
        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        // Add controls to main Form in correct docking Z-order
        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo, AutoScroll = true };

        var cardGeneral = CrearSeccion("Informacion general", 24, 22, 520, 560);
        var cardContacto = CrearSeccion("Informacion de contacto", 560, 22, 520, 560);

        picFoto = new PictureBox
        {
            Size = new Size(140, 140),
            Location = new Point(190, 62),
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
            Location = new Point(190, 210),
            Cursor = Cursors.Hand,
            Visible = false
        };
        btnCambiarFoto.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCambiarFoto.FlatAppearance.BorderSize = 1;
        btnCambiarFoto.Click += (_, _) => CambiarFoto();
        cardGeneral.Controls.Add(btnCambiarFoto);

        txtNombre = CrearFila(cardGeneral, "Nombre", 268);
        txtCurp = CrearFila(cardGeneral, "CURP", 316);
        txtFechaNacimiento = CrearFila(cardGeneral, "Fecha de nacimiento", 364);
        txtGenero = CrearFila(cardGeneral, "Genero", 412);
        txtTelefono = CrearFila(cardGeneral, "Telefono", 460);
        txtCorreo = CrearFila(cardGeneral, "Correo electronico", 508);

        txtCalle = CrearFila(cardContacto, "Calle y numero", 56);
        txtColonia = CrearFila(cardContacto, "Colonia", 126);
        txtCodigoPostal = CrearFila(cardContacto, "Codigo postal", 196);
        txtMunicipio = CrearFila(cardContacto, "Municipio", 266);
        txtEstado = CrearFila(cardContacto, "Estado", 336);

        txtNombre.KeyPress += SoloLetras_KeyPress;
        txtGenero.KeyPress += SoloLetras_KeyPress;
        txtMunicipio.KeyPress += SoloLetras_KeyPress;
        txtEstado.KeyPress += SoloLetras_KeyPress;

        txtTelefono.KeyPress += SoloNumeros_KeyPress;
        txtCodigoPostal.KeyPress += SoloNumeros_KeyPress;

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50),
            Location = new Point(870, 500)
        };
        btnEditar.FlatAppearance.BorderSize = 0;
        btnEditar.Click += (_, _) => ActivarEdicion();
        UiAssets.RedondearControl(btnEditar, 10);

        btnGuardar = BotonAccion("Guardar", Color.FromArgb(34, 166, 88), Color.White, 650, false);
        btnGuardar.Click += (_, _) => Guardar();
        btnCancelar = BotonAccion("Cancelar", Color.White, UiAssets.AzulPrincipal, 820, false);
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => Cancelar();

        panel.Controls.AddRange(new Control[] { cardGeneral, cardContacto, btnEditar, btnGuardar, btnCancelar });

        cardGeneral.Resize += (s, e) =>
        {
            picFoto.Left = (cardGeneral.Width - picFoto.Width) / 2;
            btnCambiarFoto.Left = (cardGeneral.Width - btnCambiarFoto.Width) / 2;
        };

        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int padding = 24;
            int gap = 16;
            int availableW = panel.ClientSize.Width - (padding * 2) - gap;
            if (availableW < 1000) availableW = 1000;

            int cardW = availableW / 2;

            int startX = padding;
            cardGeneral.Location = new Point(startX, 22);
            cardGeneral.Size = new Size(cardW, 560);

            cardContacto.Location = new Point(startX + cardW + gap, 22);
            cardContacto.Size = new Size(cardW, 560);

            btnEditar.Left = cardContacto.Right - btnEditar.Width;
            btnEditar.Top = cardContacto.Bottom + 16;

            btnGuardar.Left = btnEditar.Left - btnGuardar.Width - 12;
            btnGuardar.Top = cardContacto.Bottom + 16;

            btnCancelar.Left = cardContacto.Right - btnCancelar.Width;
            btnCancelar.Top = cardContacto.Bottom + 16;

            panel.AutoScrollMinSize = new Size(cardContacto.Right + padding, btnEditar.Bottom + padding);
        };
        return panel;
    }

    private Button BotonAccion(string texto, Color fondo, Color textoColor, int x, bool visible)
    {
        var b = new Button
        {
            Text = texto,
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = textoColor,
            BackColor = fondo,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(160, 50),
            Location = new Point(x, 500),
            Visible = visible
        };
        b.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(b, 10);
        return b;
    }

    private static Panel CrearSeccion(string titulo, int x, int y, int w, int h)
    {
        var card = new Panel { BackColor = Color.White, Location = new Point(x, y), Size = new Size(w, h) };
        card.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(219, 227, 241)), 0, 0, card.Width - 1, card.Height - 1);
        UiAssets.RedondearControl(card, 10);
        var enc = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(237, 244, 255) };
        enc.Controls.Add(new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 9),
            AutoSize = true
        });
        card.Controls.Add(enc);
        return card;
    }

    private TextBox CrearFila(Control parent, string etiqueta, int y)
    {
        parent.Controls.Add(new Label
        {
            Text = $"{etiqueta}:",
            Font = new Font("Segoe UI", 12F),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, y + 2),
            Size = new Size(200, 30)
        });
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 12F),
            Location = new Point(230, y + 2),
            Size = new Size(parent.Width - 260, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            TabStop = true
        };
        parent.Controls.Add(txt);
        return txt;
    }

    private IEnumerable<TextBox> Campos() => new[]
    {
        txtNombre, txtCurp, txtFechaNacimiento, txtGenero, txtTelefono, txtCorreo,
        txtCalle, txtColonia, txtCodigoPostal, txtMunicipio, txtEstado
    };

    private void SetEdicion(bool editar)
    {
        foreach (var t in Campos())
        {
            t.ReadOnly = !editar;
            t.BorderStyle = editar ? BorderStyle.FixedSingle : BorderStyle.None;
            t.BackColor = editar ? Color.FromArgb(250, 252, 255) : Color.White;
            t.TabStop = editar;
        }
    }

    private void ActivarEdicion()
    {
        if (modoEdicion) return;
        respaldo = PerfilAdministradorStore.Obtener();
        modoEdicion = true;
        SetEdicion(true);
        btnEditar.Visible = false;
        btnGuardar.Visible = true;
        btnCancelar.Visible = true;
        btnCambiarFoto.Visible = true;
    }

    private void Guardar()
    {
        string email = txtCorreo.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var p = PerfilAdministradorStore.Obtener();
        p.NombreCompleto = txtNombre.Text.Trim();
        p.Curp = txtCurp.Text.Trim();
        p.FechaNacimiento = txtFechaNacimiento.Text.Trim();
        p.Genero = txtGenero.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();
        p.Correo = txtCorreo.Text.Trim();
        p.Calle = txtCalle.Text.Trim();
        p.Colonia = txtColonia.Text.Trim();
        p.CodigoPostal = txtCodigoPostal.Text.Trim();
        p.Municipio = txtMunicipio.Text.Trim();
        p.Estado = txtEstado.Text.Trim();
        PerfilAdministradorStore.Guardar(p);
        FinalizarEdicion();
    }

    private void Cancelar()
    {
        if (respaldo != null)
            PerfilAdministradorStore.Guardar(respaldo);
        CargarVista();
        FinalizarEdicion();
    }

    private void FinalizarEdicion()
    {
        modoEdicion = false;
        SetEdicion(false);
        btnEditar.Visible = true;
        btnGuardar.Visible = false;
        btnCancelar.Visible = false;
        btnCambiarFoto.Visible = false;
    }

    private void CargarVista()
    {
        var p = PerfilAdministradorStore.Obtener();
        txtNombre.Text = p.NombreCompleto;
        txtCurp.Text = p.Curp;
        txtFechaNacimiento.Text = p.FechaNacimiento;
        txtGenero.Text = p.Genero;
        txtTelefono.Text = p.Telefono;
        txtCorreo.Text = p.Correo;
        txtCalle.Text = p.Calle;
        txtColonia.Text = p.Colonia;
        txtCodigoPostal.Text = p.CodigoPostal;
        txtMunicipio.Text = p.Municipio;
        txtEstado.Text = p.Estado;
        SetEdicion(false);
        CargarFoto(p.RutaFotoPerfil);
    }

    private void CambiarFoto()
    {
        using var dlg = new OpenFileDialog { Filter = "Imagenes|*.jpg;*.jpeg;*.png;*.bmp;*.webp" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        var p = PerfilAdministradorStore.Obtener();
        p.RutaFotoPerfil = dlg.FileName;
        PerfilAdministradorStore.Guardar(p);
        CargarFoto(p.RutaFotoPerfil);
    }

    private void CargarFoto(string ruta)
    {
        if (!string.IsNullOrWhiteSpace(ruta) && File.Exists(ruta))
        {
            using var img = Image.FromFile(ruta);
            picFoto.Image = new Bitmap(img);
        }
        else
        {
            picFoto.Image = null;
        }
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
