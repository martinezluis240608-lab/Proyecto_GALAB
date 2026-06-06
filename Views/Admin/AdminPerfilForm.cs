using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

/// <summary>
/// Perfil del administrador, estructurado exactamente con el esquema de la base de datos.
/// </summary>
public class AdminPerfilForm : Form
{
    private TextBox txtIdAdministrador = null!;
    private TextBox txtUsuario = null!;
    private TextBox txtNombre = null!;
    private TextBox txtPrimerApellido = null!;
    private TextBox txtSegundoApellido = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtRol = null!;
    private TextBox txtActivo = null!;
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

        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo, AutoScroll = true };

        var cardPersonales = CrearSeccion("Información personal", 24, 22, 520, 360);
        var cardCuenta = CrearSeccion("Detalles de la cuenta", 560, 22, 520, 360);

        txtNombre = CrearFila(cardPersonales, "Nombre", 56);
        txtPrimerApellido = CrearFila(cardPersonales, "Primer apellido", 112);
        txtSegundoApellido = CrearFila(cardPersonales, "Segundo apellido", 168);
        txtTelefono = CrearFila(cardPersonales, "Teléfono", 224);
        txtTelefono.MaxLength = 10;
        txtCorreo = CrearFila(cardPersonales, "Correo electrónico", 280);

        txtIdAdministrador = CrearFila(cardCuenta, "ID Administrador", 56);
        txtUsuario = CrearFila(cardCuenta, "Usuario", 112);
        txtRol = CrearFila(cardCuenta, "Rol de sistema", 168);
        txtActivo = CrearFila(cardCuenta, "Estado de cuenta", 224);
        txtFechaRegistro = CrearFila(cardCuenta, "Fecha de registro de ingreso", 280);

        txtNombre.KeyPress += SoloLetras_KeyPress;
        txtPrimerApellido.KeyPress += SoloLetras_KeyPress;
        txtSegundoApellido.KeyPress += SoloLetras_KeyPress;
        txtTelefono.KeyPress += SoloNumeros_KeyPress;

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50)
        };
        btnEditar.FlatAppearance.BorderSize = 0;
        btnEditar.Click += (_, _) => ActivarEdicion();
        UiAssets.RedondearControl(btnEditar, 10);

        btnGuardar = BotonAccion("Guardar", Color.FromArgb(34, 166, 88), Color.White, false);
        btnGuardar.Click += (_, _) => Guardar();

        btnCancelar = BotonAccion("Cancelar", Color.White, UiAssets.AzulPrincipal, false);
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => Cancelar();

        panel.Controls.AddRange(new Control[] { cardPersonales, cardCuenta, btnEditar, btnGuardar, btnCancelar });

        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int padding = 24;
            int gap = 16;
            int availableW = panel.ClientSize.Width - (padding * 2) - gap;
            if (availableW < 1000) availableW = 1000;

            int cardW = availableW / 2;

            int startX = padding;
            cardPersonales.Location = new Point(startX, 22);
            cardPersonales.Size = new Size(cardW, 360);

            cardCuenta.Location = new Point(startX + cardW + gap, 22);
            cardCuenta.Size = new Size(cardW, 360);

            // Position buttons dynamically below the cards
            btnEditar.Left = cardCuenta.Right - btnEditar.Width;
            btnEditar.Top = cardCuenta.Bottom + 20;

            btnGuardar.Left = btnEditar.Left - btnGuardar.Width - 12;
            btnGuardar.Top = cardCuenta.Bottom + 20;

            btnCancelar.Left = cardCuenta.Right - btnCancelar.Width;
            btnCancelar.Top = cardCuenta.Bottom + 20;

            panel.AutoScrollMinSize = new Size(cardCuenta.Right + padding, btnEditar.Bottom + padding);
        };

        return panel;
    }

    private Button BotonAccion(string texto, Color fondo, Color textoColor, bool visible)
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
            Size = new Size(240, 30)
        });
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 12F),
            Location = new Point(270, y + 2),
            Size = new Size(parent.Width - 300, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            TabStop = true
        };
        parent.Controls.Add(txt);
        return txt;
    }

    private IEnumerable<TextBox> CamposEdicion() => new[]
    {
        txtNombre, txtPrimerApellido, txtSegundoApellido, txtTelefono, txtCorreo, txtUsuario
    };

    private IEnumerable<TextBox> TodosLosCampos() => new[]
    {
        txtNombre, txtPrimerApellido, txtSegundoApellido, txtTelefono, txtCorreo,
        txtIdAdministrador, txtUsuario, txtRol, txtActivo, txtFechaRegistro
    };

    private void SetEdicion(bool editar)
    {
        foreach (var t in CamposEdicion())
        {
            t.ReadOnly = !editar;
            t.BorderStyle = editar ? BorderStyle.FixedSingle : BorderStyle.None;
            t.BackColor = editar ? Color.FromArgb(250, 252, 255) : Color.White;
            t.TabStop = editar;
        }

        foreach (var t in TodosLosCampos().Except(CamposEdicion()))
        {
            t.ReadOnly = true;
            t.BorderStyle = BorderStyle.None;
            t.BackColor = Color.White;
            t.TabStop = false;
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
        string email = txtCorreo.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string tel = txtTelefono.Text.Trim();
        if (tel.Length != 10)
        {
            MessageBox.Show("El número de teléfono debe tener exactamente 10 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtUsuario.Text.Trim()))
        {
            MessageBox.Show("El nombre de usuario es obligatorio.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtNombre.Text.Trim()))
        {
            MessageBox.Show("El nombre es obligatorio.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text.Trim()))
        {
            MessageBox.Show("El primer apellido es obligatorio.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var p = PerfilAdministradorStore.Obtener();
        p.Usuario = txtUsuario.Text.Trim();
        p.Nombre = txtNombre.Text.Trim();
        p.PrimerApellido = txtPrimerApellido.Text.Trim();
        p.SegundoApellido = txtSegundoApellido.Text.Trim();
        p.Correo = txtCorreo.Text.Trim();
        p.Telefono = txtTelefono.Text.Trim();

        PerfilAdministradorStore.Guardar(p);
        FinalizarEdicion();
        CargarVista();
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
        txtIdAdministrador.Text = p.IdAdministrador;
        txtUsuario.Text = p.Usuario;
        txtNombre.Text = p.Nombre;
        txtPrimerApellido.Text = p.PrimerApellido;
        txtSegundoApellido.Text = p.SegundoApellido;
        txtCorreo.Text = p.Correo;
        txtTelefono.Text = p.Telefono;
        txtRol.Text = p.Rol;
        txtActivo.Text = p.Activo ? "Activo" : "Inactivo";
        txtFechaRegistro.Text = p.FechaRegistro.ToString("g");
        SetEdicion(false);
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
