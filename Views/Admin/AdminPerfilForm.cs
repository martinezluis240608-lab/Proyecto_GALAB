using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

/// <summary>
/// Perfil del administrador, sin datos escolares ni campos personales no necesarios.
/// </summary>
public class AdminPerfilForm : Form
{
    private TextBox txtIdAdministrador = null!;
    private TextBox txtNombre = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtUsuario = null!;
    private TextBox txtRol = null!;
    private TextBox txtEstado = null!;
    private TextBox txtFechaRegistro = null!;
    private Button btnEditar = null!;
    private Button btnGuardar = null!;
    private Button btnCancelar = null!;
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

        var cardGeneral = CrearSeccion("Informacion general", 24, 22, 520, 220);
        var cardCuenta = CrearSeccion("Informacion de cuenta", 560, 22, 520, 260);

        txtNombre = CrearFila(cardGeneral, "Nombre", 62);
        txtCorreo = CrearFila(cardGeneral, "Correo electronico", 100);
        txtTelefono = CrearFila(cardGeneral, "Telefono", 138);

        txtIdAdministrador = CrearFila(cardCuenta, "ID administrador", 56);
        txtUsuario = CrearFila(cardCuenta, "Usuario", 92);
        txtRol = CrearFila(cardCuenta, "Rol", 128);
        txtEstado = CrearFila(cardCuenta, "Estado", 164);
        txtFechaRegistro = CrearFila(cardCuenta, "Fecha de registro", 200);

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50),
            Location = new Point(870, 360)
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

        panel.Controls.AddRange(new Control[] { cardGeneral, cardCuenta, btnEditar, btnGuardar, btnCancelar });
        panel.Resize += (s, e) =>
        {
            int w = Math.Max(1060, panel.ClientSize.Width - 48);
            int startX = (panel.ClientSize.Width - w) / 2;
            cardGeneral.Left = startX;
            cardCuenta.Left = cardGeneral.Right + 16;
            btnEditar.Left = cardCuenta.Right - btnEditar.Width;
            btnGuardar.Left = btnEditar.Left - btnGuardar.Width - 12;
            btnCancelar.Left = cardCuenta.Right - btnCancelar.Width;
            panel.AutoScrollMinSize = new Size(startX + w + 20, 520);
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
            Location = new Point(x, 360),
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
        txtNombre, txtTelefono, txtCorreo
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
    }

    private void Guardar()
    {
        var p = PerfilAdministradorStore.Obtener();
        p.NombreCompleto = txtNombre.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();
        p.Correo = txtCorreo.Text.Trim();
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
    }

    private void CargarVista()
    {
        var p = PerfilAdministradorStore.Obtener();
        txtNombre.Text = p.NombreCompleto;
        txtTelefono.Text = p.Telefono;
        txtCorreo.Text = p.Correo;
        txtIdAdministrador.Text = p.IdAdministrador;
        txtUsuario.Text = string.IsNullOrWhiteSpace(p.Usuario) ? SesionActual.NombreUsuario : p.Usuario;
        txtRol.Text = p.Rol;
        txtEstado.Text = p.Estado;
        txtFechaRegistro.Text = p.FechaRegistro;
        SetEdicion(false);
    }
}
