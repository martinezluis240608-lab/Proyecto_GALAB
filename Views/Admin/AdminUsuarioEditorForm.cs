using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminUsuarioEditorForm : Form
{
    private readonly UsuarioSistema? _existente;
    
    private TextBox txtId = null!;
    private TextBox txtNombre = null!;
    private TextBox txtPrimerApellido = null!;
    private TextBox txtSegundoApellido = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtTelefono = null!;
    
    private ComboBox cmbRol = null!;
    private ComboBox cmbEstado = null!;
    private TextBox txtUsuario = null!;
    
    private Panel pnlAlumnos = null!;
    private TextBox txtNumeroControl = null!;
    private TextBox txtSemestre = null!;
    private TextBox txtGrupo = null!;
    private TextBox txtNumeroAsiento = null!;

    public AdminUsuarioEditorForm(UsuarioSistema? existente)
    {
        _existente = existente;
        Text = existente == null ? "Nuevo usuario" : "Editar usuario";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(720, 560);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        // Column 1 (Left - Personal Info)
        var lblId = Titulo("ID (Asignado por el sistema)", 24, 20);
        var pnlId = CrearCampoTexto(existente?.Id ?? "Autogenerado", out txtId, 24, 44, 320);
        txtId.ReadOnly = true;
        pnlId.BackColor = Color.FromArgb(240, 243, 248);
        txtId.BackColor = Color.FromArgb(240, 243, 248);

        var lblNombre = Titulo("Nombre(s) *", 24, 94);
        var pnlNombre = CrearCampoTexto(existente?.Nombre ?? "", out txtNombre, 24, 118, 320);

        var lblPrimerApellido = Titulo("Primer apellido *", 24, 168);
        var pnlPrimerApellido = CrearCampoTexto(existente?.PrimerApellido ?? "", out txtPrimerApellido, 24, 192, 320);

        var lblSegundoApellido = Titulo("Segundo apellido", 24, 242);
        var pnlSegundoApellido = CrearCampoTexto(existente?.SegundoApellido ?? "", out txtSegundoApellido, 24, 266, 320);

        var lblCorreo = Titulo("Correo electrónico *", 24, 316);
        var pnlCorreo = CrearCampoTexto(existente?.Correo ?? "", out txtCorreo, 24, 340, 320);

        var lblTelefono = Titulo("Teléfono *", 24, 390);
        var pnlTelefono = CrearCampoTexto(existente?.Telefono ?? "", out txtTelefono, 24, 414, 320);
        txtTelefono.MaxLength = 10;

        // Column 2 (Right - Account Info)
        var lblRol = Titulo("Rol *", 376, 20);
        var pnlRol = CrearCampoCombo(new[] { "Administrador", "Soporte", "Técnico", "Usuario" }, existente?.Rol ?? "Usuario", out cmbRol, 376, 44, 320);

        var lblEstado = Titulo("Estado *", 376, 94);
        var pnlEstado = CrearCampoCombo(new[] { "Activo", "Inactivo" }, existente?.Estado ?? "Activo", out cmbEstado, 376, 118, 320);

        var lblUsuarioField = Titulo("Nombre de Usuario (Autogenerado)", 376, 168);
        var pnlUsuarioField = CrearCampoTexto(existente?.Usuario ?? "", out txtUsuario, 376, 192, 320);
        txtUsuario.ReadOnly = true;
        pnlUsuarioField.BackColor = Color.FromArgb(240, 243, 248);
        txtUsuario.BackColor = Color.FromArgb(240, 243, 248);

        // Panel de Alumnos (Dinámico)
        pnlAlumnos = new Panel
        {
            Location = new Point(376, 242),
            Size = new Size(320, 220),
            BackColor = Color.Transparent
        };

        var lblNumeroControl = Titulo("Número de Control *", 0, 0);
        string valNumControl = (existente == null || existente.NumeroControl == 0) ? "" : existente.NumeroControl.ToString();
        var pnlNumeroControl = CrearCampoTexto(valNumControl, out txtNumeroControl, 0, 24, 320);

        var lblSemestre = Titulo("Semestre *", 0, 74);
        var pnlSemestre = CrearCampoTexto(existente?.Semestre ?? "", out txtSemestre, 0, 98, 150);

        var lblGrupo = Titulo("Grupo *", 170, 74);
        var pnlGrupo = CrearCampoTexto(existente?.Grupo ?? "", out txtGrupo, 170, 98, 150);

        var lblAsiento = Titulo("Número de Asiento", 0, 148);
        var pnlAsiento = CrearCampoTexto(existente?.NumeroAsiento?.ToString() ?? "", out txtNumeroAsiento, 0, 172, 320);

        pnlAlumnos.Controls.AddRange(new Control[] { 
            lblNumeroControl, pnlNumeroControl,
            lblSemestre, pnlSemestre, 
            lblGrupo, pnlGrupo, 
            lblAsiento, pnlAsiento 
        });

        // Evento de cambio de rol para mostrar/ocultar campos académicos
        cmbRol.SelectedIndexChanged += (s, e) =>
        {
            bool esEstudiante = cmbRol.SelectedItem?.ToString() == "Usuario";
            pnlAlumnos.Visible = esEstudiante;
            if (esEstudiante)
            {
                ActualizarUsuarioAutocompletado();
            }
            else
            {
                txtUsuario.Text = txtCorreo.Text.Trim();
            }
        };
        pnlAlumnos.Visible = (cmbRol.SelectedItem?.ToString() == "Usuario");

        txtNombre.TextChanged += (s, e) => { if (cmbRol.SelectedItem?.ToString() == "Usuario") ActualizarUsuarioAutocompletado(); };
        txtPrimerApellido.TextChanged += (s, e) => { if (cmbRol.SelectedItem?.ToString() == "Usuario") ActualizarUsuarioAutocompletado(); };
        txtSegundoApellido.TextChanged += (s, e) => { if (cmbRol.SelectedItem?.ToString() == "Usuario") ActualizarUsuarioAutocompletado(); };

        txtCorreo.TextChanged += (s, e) =>
        {
            if (cmbRol.SelectedItem?.ToString() != "Usuario")
            {
                txtUsuario.Text = txtCorreo.Text.Trim();
            }
        };

        // KeyPress Validations
        txtNombre.KeyPress += SoloLetras_KeyPress;
        txtPrimerApellido.KeyPress += SoloLetras_KeyPress;
        txtSegundoApellido.KeyPress += SoloLetras_KeyPress;
        txtTelefono.KeyPress += SoloNumeros_KeyPress;
        txtNumeroControl.KeyPress += SoloNumeros_KeyPress;
        txtSemestre.KeyPress += SoloNumeros_KeyPress;
        txtNumeroAsiento.KeyPress += SoloNumeros_KeyPress;

        // Add main controls
        Controls.AddRange(new Control[]
        {
            lblId, pnlId,
            lblNombre, pnlNombre,
            lblPrimerApellido, pnlPrimerApellido,
            lblSegundoApellido, pnlSegundoApellido,
            lblCorreo, pnlCorreo,
            lblTelefono, pnlTelefono,
            lblRol, pnlRol,
            lblEstado, pnlEstado,
            lblUsuarioField, pnlUsuarioField,
            pnlAlumnos
        });

        // Bottom panel for action buttons
        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 64,
            BackColor = Color.White
        };
        bottomPanel.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(UiAssets.Borde), 0, 0, bottomPanel.Width, 0);
        };

        var btnCancelar = new Button 
        { 
            Text = "Cancelar", 
            DialogResult = DialogResult.Cancel, 
            Location = new Point(24, 12), 
            Size = new Size(130, 40),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = Color.White,
            ForeColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        UiAssets.RedondearControl(btnCancelar, 6);

        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(566, 12),
            Size = new Size(130, 40),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btnGuardar, 6);

        // Hover Animations
        btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = UiAssets.AzulOscuro;
        btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = UiAssets.AzulPrincipal;
        btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = UiAssets.AzulClaro;
        btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.White;

        btnGuardar.Click += (_, _) =>
        {
            // Validations
            if (string.IsNullOrWhiteSpace(txtId.Text) || 
                string.IsNullOrWhiteSpace(txtNombre.Text) || 
                string.IsNullOrWhiteSpace(txtPrimerApellido.Text) || 
                string.IsNullOrWhiteSpace(txtCorreo.Text) || 
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Los campos con asterisco (*) son obligatorios.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            bool esEstudiante = cmbRol.SelectedItem?.ToString() == "Usuario";
            if (esEstudiante)
            {
                if (string.IsNullOrWhiteSpace(txtNumeroControl.Text))
                {
                    MessageBox.Show("El Número de Control es obligatorio para los estudiantes.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!long.TryParse(txtNumeroControl.Text.Trim(), out _))
                {
                    MessageBox.Show("El Número de Control debe ser un número válido.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSemestre.Text) || string.IsNullOrWhiteSpace(txtGrupo.Text))
                {
                    MessageBox.Show("Semestre y Grupo son requeridos para los estudiantes.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int? asiento = null;
            if (!string.IsNullOrWhiteSpace(txtNumeroAsiento.Text))
            {
                if (int.TryParse(txtNumeroAsiento.Text.Trim(), out int val))
                    asiento = val;
            }

            var u = _existente ?? new UsuarioSistema();
            u.Id = txtId.Text.Trim();
            u.Nombre = txtNombre.Text.Trim();
            u.PrimerApellido = txtPrimerApellido.Text.Trim();
            u.SegundoApellido = txtSegundoApellido.Text.Trim();
            u.NombreCompleto = $"{u.Nombre} {u.PrimerApellido} {u.SegundoApellido}".Trim();
            u.Correo = email;
            u.Rol = cmbRol.SelectedItem?.ToString() ?? "Usuario";
            u.Estado = cmbEstado.SelectedItem?.ToString() ?? "Activo";
            u.Telefono = txtTelefono.Text.Trim();
            
            string username = txtUsuario.Text.Trim();
            u.Usuario = username;
            
            if (_existente == null)
            {
                // Assign a default password for new users
                u.Contrasena = esEstudiante ? txtNumeroControl.Text.Trim() : "12345";
            }
            
            if (esEstudiante)
            {
                if (long.TryParse(txtNumeroControl.Text.Trim(), out long numCtrl))
                    u.NumeroControl = numCtrl;
                u.Semestre = txtSemestre.Text.Trim();
                u.Grupo = txtGrupo.Text.Trim();
                u.NumeroAsiento = asiento;
            }
            else
            {
                u.NumeroControl = 0;
            }

            try
            {
                UsuarioSistemaStore.Guardar(u, _existente == null);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                NotificacionForm.MostrarExcepcion(this, ex);
            }
        };

        bottomPanel.Controls.AddRange(new Control[] { btnCancelar, btnGuardar });
        Controls.Add(bottomPanel);
        CancelButton = btnCancelar;
    }

    private void ActualizarUsuarioAutocompletado()
    {
        string n = txtNombre.Text.Trim().ToLower();
        string p1 = txtPrimerApellido.Text.Trim().ToLower();
        string p2 = txtSegundoApellido.Text.Trim().ToLower();

        var partes = new System.Collections.Generic.List<string>();
        if (!string.IsNullOrEmpty(n)) partes.Add(n);
        if (!string.IsNullOrEmpty(p1)) partes.Add(p1);
        if (!string.IsNullOrEmpty(p2)) partes.Add(p2);

        txtUsuario.Text = string.Join(" ", partes);
    }

    private static Label Titulo(string t, int x, int y) => new()
    {
        Text = t, 
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), 
        ForeColor = UiAssets.AzulOscuro,
        Location = new Point(x, y), 
        AutoSize = true
    };

    private Panel CrearCampoTexto(string valor, out TextBox txtOut, int x, int y, int width)
    {
        var container = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(width, 40),
            BackColor = Color.White
        };

        var txt = new TextBox
        {
            Text = valor,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(10, 10),
            Width = width - 20
        };
        txtOut = txt;

        container.Controls.Add(txt);

        bool isHovered = false;

        container.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var colorBorde = txt.Focused ? UiAssets.AzulPrincipal 
                           : isHovered ? Color.FromArgb(160, 185, 220) 
                           : UiAssets.Borde;
            int grosor = txt.Focused ? 2 : 1;
            using var pen = new Pen(colorBorde, grosor);
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, container.Width - 1, container.Height - 1), 6);
            e.Graphics.DrawPath(pen, path);
        };

        txt.GotFocus += (s, e) => container.Invalidate();
        txt.LostFocus += (s, e) => container.Invalidate();

        container.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        container.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };
        txt.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        txt.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };

        UiAssets.RedondearControl(container, 6);

        return container;
    }

    private Panel CrearCampoCombo(object[] items, object seleccionado, out ComboBox cmbOut, int x, int y, int width)
    {
        var container = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(width, 40),
            BackColor = Color.White
        };

        var cmb = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(8, 8),
            Width = width - 16
        };
        cmb.Items.AddRange(items);
        cmb.SelectedItem = seleccionado;
        cmbOut = cmb;

        container.Controls.Add(cmb);

        bool isHovered = false;

        container.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var colorBorde = cmb.Focused ? UiAssets.AzulPrincipal 
                           : isHovered ? Color.FromArgb(160, 185, 220) 
                           : UiAssets.Borde;
            int grosor = cmb.Focused ? 2 : 1;
            using var pen = new Pen(colorBorde, grosor);
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, container.Width - 1, container.Height - 1), 6);
            e.Graphics.DrawPath(pen, path);
        };

        cmb.GotFocus += (s, e) => container.Invalidate();
        cmb.LostFocus += (s, e) => container.Invalidate();

        container.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        container.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };
        cmb.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        cmb.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };

        UiAssets.RedondearControl(container, 6);

        return container;
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
