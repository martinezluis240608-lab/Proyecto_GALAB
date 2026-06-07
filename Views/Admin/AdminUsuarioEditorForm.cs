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
    private TextBox txtContrasena = null!;
    private TextBox txtConfirmarContrasena = null!;
    
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
        ClientSize = new Size(720, 810);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        
        var lblHeaderPersonal = new Label
        {
            Text = "👤  Información Personal",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 16),
            AutoSize = true
        };

        var lblHeaderAcceso = new Label
        {
            Text = "🔒  Información de Acceso",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(376, 16),
            AutoSize = true
        };

        // Column 1 (Left - Personal Info)
        var lblId = Titulo("ID (Asignado por el sistema)", 24, 56);
        string idVal = existente?.Id ?? "Autogenerado";
        var pnlId = CrearCampoTexto(idVal, out txtId, 24, 80, 320);
        txtId.ReadOnly = true;
        pnlId.BackColor = Color.FromArgb(240, 243, 248);
        txtId.BackColor = Color.FromArgb(240, 243, 248);

        var lblNombre = Titulo("Nombre(s) *", 24, 130);
        var pnlNombre = CrearCampoTexto(existente?.Nombre ?? "", out txtNombre, 24, 154, 320);

        var lblPrimerApellido = Titulo("Primer apellido *", 24, 204);
        var pnlPrimerApellido = CrearCampoTexto(existente?.PrimerApellido ?? "", out txtPrimerApellido, 24, 228, 320);

        var lblSegundoApellido = Titulo("Segundo apellido", 24, 278);
        var pnlSegundoApellido = CrearCampoTexto(existente?.SegundoApellido ?? "", out txtSegundoApellido, 24, 302, 320);

        var lblCorreo = Titulo("Correo electrónico *", 24, 352);
        var pnlCorreo = CrearCampoTexto(existente?.Correo ?? "", out txtCorreo, 24, 376, 320);

        var lblTelefono = Titulo("Teléfono *", 24, 426);
        var pnlTelefono = CrearCampoTexto(existente?.Telefono ?? "", out txtTelefono, 24, 450, 320);
        txtTelefono.MaxLength = 10;

        // Column 2 (Right - Account Info)
        var lblRol = Titulo("Rol *", 376, 56);
        var pnlRol = CrearCampoCombo(new[] { "Administrador", "Usuario" }, existente?.Rol ?? "Usuario", out cmbRol, 376, 80, 320);

        var lblEstado = Titulo("Estado *", 376, 130);
        var pnlEstado = CrearCampoCombo(new[] { "Activo", "Inactivo" }, existente?.Estado ?? "Activo", out cmbEstado, 376, 154, 320);

        var lblUsuarioField = Titulo("Usuario *", 376, 204);
        var pnlUsuarioField = CrearCampoTexto(existente?.Usuario ?? "", out txtUsuario, 376, 228, 320);
        txtUsuario.ReadOnly = false;

        var lblContrasenaField = Titulo("Contraseña" + (existente == null ? " *" : ""), 376, 278);
        var pnlContrasenaField = CrearCampoTexto(existente == null ? "" : existente.Contrasena, out txtContrasena, 376, 302, 320);
        txtContrasena.UseSystemPasswordChar = true;
        txtContrasena.Width = 320 - 50;
        var btnVerPassword = new Button
        {
            Text = "🙈",
            Font = new Font("Segoe UI Emoji", 12F, FontStyle.Regular),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(30, 30),
            Location = new Point(320 - 36, 4)
        };
        btnVerPassword.FlatAppearance.BorderSize = 0;
        bool passwordVisible = false;
        btnVerPassword.Click += (s, e) =>
        {
            passwordVisible = !passwordVisible;
            txtContrasena.UseSystemPasswordChar = !passwordVisible;
            btnVerPassword.Text = passwordVisible ? "👁" : "🙈";
        };
        pnlContrasenaField.Controls.Add(btnVerPassword);

        var lblConfirmarContrasenaField = Titulo("Confirmar contraseña" + (existente == null ? " *" : ""), 376, 352);
        var pnlConfirmarContrasenaField = CrearCampoTexto(existente == null ? "" : existente.Contrasena, out txtConfirmarContrasena, 376, 376, 320);
        txtConfirmarContrasena.UseSystemPasswordChar = true;
        txtConfirmarContrasena.Width = 320 - 50;
        var btnVerConfirmar = new Button
        {
            Text = "🙈",
            Font = new Font("Segoe UI Emoji", 12F, FontStyle.Regular),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(30, 30),
            Location = new Point(320 - 36, 4)
        };
        btnVerConfirmar.FlatAppearance.BorderSize = 0;
        bool confirmVisible = false;
        btnVerConfirmar.Click += (s, e) =>
        {
            confirmVisible = !confirmVisible;
            txtConfirmarContrasena.UseSystemPasswordChar = !confirmVisible;
            btnVerConfirmar.Text = confirmVisible ? "👁" : "🙈";
        };
        pnlConfirmarContrasenaField.Controls.Add(btnVerConfirmar);

        
        var pnlRecomendacion = new Panel
        {
            Location = new Point(376, 426),
            Size = new Size(320, 70),
            BackColor = Color.FromArgb(240, 248, 255)
        };
        pnlRecomendacion.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(180, 210, 245), 1);
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, pnlRecomendacion.Width - 1, pnlRecomendacion.Height - 1), 6);
            e.Graphics.DrawPath(pen, path);
        };
        UiAssets.RedondearControl(pnlRecomendacion, 6);

        var lblRecIcon = new Label
        {
            Text = "ℹ",
            Font = new Font("Segoe UI Symbol", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(10, 10),
            AutoSize = true
        };
        var lblRecTitulo = new Label
        {
            Text = "Recomendación",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(34, 12),
            AutoSize = true
        };
        var lblRecTexto = new Label
        {
            Text = "Usa una contraseña segura con al menos 8 caracteres, combinando letras, números y símbolos.",
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(10, 34),
            Size = new Size(300, 30)
        };
        pnlRecomendacion.Controls.AddRange(new Control[] { lblRecIcon, lblRecTitulo, lblRecTexto });

        // Panel de Alumnos (Dinámico)
        pnlAlumnos = new Panel
        {
            Location = new Point(376, 506),
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
            lblContrasenaField, pnlContrasenaField,
            lblConfirmarContrasenaField, pnlConfirmarContrasenaField,
            pnlAlumnos,
            lblHeaderPersonal, lblHeaderAcceso, pnlRecomendacion
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
                Proyecto_GALAB.Views.CustomMessageBox.Show("Los campos con asterisco (*) son obligatorios.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtCorreo.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tel = txtTelefono.Text.Trim();
            if (tel.Length != 10)
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("El número de teléfono debe tener exactamente 10 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool esEstudiante = cmbRol.SelectedItem?.ToString() == "Usuario";
            if (esEstudiante)
            {
                if (string.IsNullOrWhiteSpace(txtNumeroControl.Text))
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("El Número de Control es obligatorio para los estudiantes.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!long.TryParse(txtNumeroControl.Text.Trim(), out _))
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("El Número de Control debe ser un número válido.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSemestre.Text) || string.IsNullOrWhiteSpace(txtGrupo.Text))
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Semestre y Grupo son requeridos para los estudiantes.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            
            if (_existente == null && string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("La contraseña es obligatoria para nuevos usuarios.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(txtContrasena.Text) || !string.IsNullOrEmpty(txtConfirmarContrasena.Text))
            {
                if (txtContrasena.Text != txtConfirmarContrasena.Text)
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Las contraseñas no coinciden.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else 
                {
                    u.Contrasena = txtContrasena.Text;
                }
            }
            else if (_existente == null)
            {
                // Fallback (though validation above prevents this)
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
        
        ConfigurarAutocompletado();
    }

    private void ConfigurarAutocompletado()
    {
        txtNombre.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtNombre.AutoCompleteSource = AutoCompleteSource.CustomSource;

        txtPrimerApellido.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtPrimerApellido.AutoCompleteSource = AutoCompleteSource.CustomSource;

        txtSegundoApellido.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtSegundoApellido.AutoCompleteSource = AutoCompleteSource.CustomSource;

        txtCorreo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtCorreo.AutoCompleteSource = AutoCompleteSource.CustomSource;

        var sourceNombres = new AutoCompleteStringCollection();
        var sourceApellidos1 = new AutoCompleteStringCollection();
        var sourceApellidos2 = new AutoCompleteStringCollection();
        var sourceCorreos = new AutoCompleteStringCollection();

        sourceCorreos.AddRange(new[] { "gmail.com", "hotmail.com", "yahoo.com", "outlook.com", "instituto.edu.mx" });

        try
        {
            var todos = UsuarioSistemaStore.ObtenerTodos();
            foreach (var u in todos)
            {
                if (!string.IsNullOrWhiteSpace(u.Nombre) && !sourceNombres.Contains(u.Nombre))
                    sourceNombres.Add(u.Nombre);
                
                if (!string.IsNullOrWhiteSpace(u.PrimerApellido) && !sourceApellidos1.Contains(u.PrimerApellido))
                    sourceApellidos1.Add(u.PrimerApellido);
                
                if (!string.IsNullOrWhiteSpace(u.SegundoApellido) && !sourceApellidos2.Contains(u.SegundoApellido))
                    sourceApellidos2.Add(u.SegundoApellido);

                if (!string.IsNullOrWhiteSpace(u.Correo) && !sourceCorreos.Contains(u.Correo))
                    sourceCorreos.Add(u.Correo);
            }
        }
        catch { /* Ignorar si no hay conexión en tiempo de diseño o falla */ }

        txtNombre.AutoCompleteCustomSource = sourceNombres;
        txtPrimerApellido.AutoCompleteCustomSource = sourceApellidos1;
        txtSegundoApellido.AutoCompleteCustomSource = sourceApellidos2;
        txtCorreo.AutoCompleteCustomSource = sourceCorreos;
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
