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
        ClientSize = new Size(480, 300);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        txtNombre = Campo(existente?.NombreCompleto ?? "", 56);
        txtCorreo = Campo(existente?.Correo ?? "", 116);
        cmbRol = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(24, 176), Size = new Size(420, 32) };
        cmbRol.Items.AddRange(new[] { "Administrador", "Soporte", "Técnico", "Usuario" });
        cmbRol.SelectedItem = existente?.Rol ?? "Usuario";
        cmbEstado = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(24, 236), Size = new Size(420, 32) };
        cmbEstado.Items.AddRange(new[] { "Activo", "Inactivo" });
        cmbEstado.SelectedItem = existente?.Estado ?? "Activo";

        Controls.AddRange(new Control[]
        {
            Titulo("Nombre completo", 32), txtNombre,
            Titulo("Correo electrónico", 92), txtCorreo,
            Titulo("Rol", 152), cmbRol,
            Titulo("Estado", 212), cmbEstado
        });

        var btnCancelar = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(24, 252), Size = new Size(110, 36) };
        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(334, 252),
            Size = new Size(110, 36),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Nombre y correo son obligatorios.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var u = _existente ?? new UsuarioSistema();
            u.NombreCompleto = txtNombre.Text.Trim();
            u.Correo = txtCorreo.Text.Trim();
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
        Text = t, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = UiAssets.AzulOscuro,
        Location = new Point(24, y), AutoSize = true
    };

    private static TextBox Campo(string valor, int y) => new()
    {
        Text = valor, Location = new Point(24, y), Size = new Size(420, 32), BorderStyle = BorderStyle.FixedSingle
    };
}
