using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminUsuarioEditorForm : Form
{
    private readonly UsuarioSistema? _existente;
    private TextBox txtNombre = null!;
    private TextBox txtCorreo = null!;
    private ComboBox cmbRol = null!;
    private ComboBox cmbEstado = null!;

    public AdminUsuarioEditorForm(UsuarioSistema? existente)
    {
        _existente = existente;
        Text = existente == null ? "Nuevo usuario" : "Editar usuario";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(480, 430);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        var lblNombre = Titulo("Nombre completo", 20);
        var pnlNombre = CrearCampoTexto(existente?.NombreCompleto ?? "", out txtNombre, 44);

        var lblCorreo = Titulo("Correo electrónico", 100);
        var pnlCorreo = CrearCampoTexto(existente?.Correo ?? "", out txtCorreo, 124);

        var lblRol = Titulo("Rol", 180);
        var pnlRol = CrearCampoCombo(new[] { "Administrador", "Soporte", "Técnico", "Usuario" }, existente?.Rol ?? "Usuario", out cmbRol, 204);

        var lblEstado = Titulo("Estado", 260);
        var pnlEstado = CrearCampoCombo(new[] { "Activo", "Inactivo" }, existente?.Estado ?? "Activo", out cmbEstado, 284);

        txtNombre.KeyPress += SoloLetras_KeyPress;

        Controls.AddRange(new Control[]
        {
            lblNombre, pnlNombre,
            lblCorreo, pnlCorreo,
            lblRol, pnlRol,
            lblEstado, pnlEstado
        });

        var btnCancelar = new Button 
        { 
            Text = "Cancelar", 
            DialogResult = DialogResult.Cancel, 
            Location = new Point(24, 360), 
            Size = new Size(120, 42),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = Color.White,
            ForeColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        UiAssets.RedondearControl(btnCancelar, 6);

        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(334, 360),
            Size = new Size(120, 42),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btnGuardar, 6);

        // Animaciones Hover
        btnGuardar.MouseEnter += (s, e) => btnGuardar.BackColor = UiAssets.AzulOscuro;
        btnGuardar.MouseLeave += (s, e) => btnGuardar.BackColor = UiAssets.AzulPrincipal;
        btnCancelar.MouseEnter += (s, e) => btnCancelar.BackColor = UiAssets.AzulClaro;
        btnCancelar.MouseLeave += (s, e) => btnCancelar.BackColor = Color.White;

        btnGuardar.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Nombre y correo son obligatorios.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtCorreo.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var u = _existente ?? new UsuarioSistema();
            u.NombreCompleto = txtNombre.Text.Trim();
            u.Correo = email;
            u.Rol = cmbRol.SelectedItem?.ToString() ?? "Usuario";
            u.Estado = cmbEstado.SelectedItem?.ToString() ?? "Activo";
            UsuarioSistemaStore.Guardar(u, _existente == null);
            DialogResult = DialogResult.OK;
            Close();
        };

        Controls.AddRange(new Control[] { btnCancelar, btnGuardar });
        CancelButton = btnCancelar;
    }

    private static Label Titulo(string t, int y) => new()
    {
        Text = t, 
        Font = new Font("Segoe UI", 10F, FontStyle.Bold), 
        ForeColor = UiAssets.AzulOscuro,
        Location = new Point(24, y), 
        AutoSize = true
    };

    private Panel CrearCampoTexto(string valor, out TextBox txtOut, int y)
    {
        var container = new Panel
        {
            Location = new Point(24, y),
            Size = new Size(430, 40),
            BackColor = Color.White
        };

        var txt = new TextBox
        {
            Text = valor,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(10, 10),
            Width = 410
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

    private Panel CrearCampoCombo(object[] items, object seleccionado, out ComboBox cmbOut, int y)
    {
        var container = new Panel
        {
            Location = new Point(24, y),
            Size = new Size(430, 40),
            BackColor = Color.White
        };

        var cmb = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(8, 8),
            Width = 414
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

    private void SoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
        {
            e.Handled = true;
        }
    }
}
