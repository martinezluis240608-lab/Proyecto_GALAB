using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

/// <summary>
/// Perfil del administrador: igual al estudiante pero sin bloque escolar (carrera, semestre, etc.).
/// </summary>
public class AdminPerfilForm : Form
{
    private PictureBox picFoto = null!;
    private TextBox txtNombre = null!;
    private TextBox txtCurp = null!;
    private TextBox txtFechaNacimiento = null!;
    private TextBox txtGenero = null!;
    private TextBox txtEstadoCivil = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtSeguro = null!;
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
        Controls.Add(CrearContenido());
        Controls.Add(AdminSidebar.Crear(this, AdminModulo.Perfil));
        Controls.Add(AdminSidebar.CrearHeader());
        Controls.Add(new Label
        {
            Text = "© 2025 GALAB - Panel administrador",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        });
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo, AutoScroll = true };

        var cardGeneral = CrearSeccion("👤  Información general", 24, 22, 520, 520);
        var cardContacto = CrearSeccion("📞  Información de contacto", 560, 22, 520, 520);

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
        txtCurp = CrearFila(cardGeneral, "CURP", 306);
        txtFechaNacimiento = CrearFila(cardGeneral, "Fecha de nacimiento", 344);
        txtGenero = CrearFila(cardGeneral, "Género", 382);
        txtEstadoCivil = CrearFila(cardGeneral, "Estado civil", 420);
        txtTelefono = CrearFila(cardGeneral, "Teléfono", 458);
        txtCorreo = CrearFila(cardGeneral, "Correo electrónico", 496);
        txtSeguro = CrearFila(cardGeneral, "N° de servicio médico", 534);

        txtCalle = CrearFila(cardContacto, "Calle y número", 56);
        txtColonia = CrearFila(cardContacto, "Colonia", 104);
        txtCodigoPostal = CrearFila(cardContacto, "Código postal", 152);
        txtMunicipio = CrearFila(cardContacto, "Municipio", 200);
        txtEstado = CrearFila(cardContacto, "Estado", 248);

        btnEditar = new Button
        {
            Text = "✎  Editar perfil",
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

        btnGuardar = BotonAccion("Guardar", Color.FromArgb(34, 166, 88), Color.White, 650, false);
        btnGuardar.Click += (_, _) => Guardar();
        btnCancelar = BotonAccion("Cancelar", Color.White, UiAssets.AzulPrincipal, 820, false);
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => Cancelar();

        panel.Controls.AddRange(new Control[] { cardGeneral, cardContacto, btnEditar, btnGuardar, btnCancelar });
        panel.Resize += (s, e) =>
        {
            int w = Math.Max(1060, panel.ClientSize.Width - 48);
            int startX = (panel.ClientSize.Width - w) / 2;
            cardGeneral.Left = startX;
            cardContacto.Left = cardGeneral.Right + 16;
            btnEditar.Left = cardContacto.Right - btnEditar.Width;
            btnGuardar.Left = btnEditar.Left - btnGuardar.Width - 12;
            btnCancelar.Left = cardContacto.Right - btnCancelar.Width;
            panel.AutoScrollMinSize = new Size(startX + w + 20, 640);
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
            Location = new Point(x, 560),
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
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, y + 2),
            Size = new Size(200, 28)
        });
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(230, y + 2),
            Size = new Size(parent.Width - 260, 28),
            TabStop = true
        };
        parent.Controls.Add(txt);
        return txt;
    }

    private IEnumerable<TextBox> Campos() => new[]
    {
        txtNombre, txtCurp, txtFechaNacimiento, txtGenero, txtEstadoCivil, txtTelefono, txtCorreo, txtSeguro,
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
        var p = PerfilAdministradorStore.Obtener();
        p.NombreCompleto = txtNombre.Text.Trim();
        p.Curp = txtCurp.Text.Trim();
        p.FechaNacimiento = txtFechaNacimiento.Text.Trim();
        p.Genero = txtGenero.Text.Trim();
        p.EstadoCivil = txtEstadoCivil.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();
        p.Correo = txtCorreo.Text.Trim();
        p.NumeroServicioMedico = txtSeguro.Text.Trim();
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
        txtEstadoCivil.Text = p.EstadoCivil;
        txtTelefono.Text = p.Telefono;
        txtCorreo.Text = p.Correo;
        txtSeguro.Text = p.NumeroServicioMedico;
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
        using var dlg = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.webp" };
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
            picFoto.Image = null;
    }
}
