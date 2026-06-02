using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminIncidenciaEditorForm : Form
{
    private readonly IncidenciaListadoItem _item;
    private TextBox txtTitulo = null!;
    private ComboBox cmbTipo = null!;
    private ComboBox cmbEstado = null!;

    public AdminIncidenciaEditorForm(IncidenciaListadoItem item)
    {
        _item = item;
        Text = "Editar incidencia";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(520, 280);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        Controls.Add(new Label
        {
            Text = $"Editar {item.Folio}",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 16),
            AutoSize = true
        });

        txtTitulo = new TextBox { Text = item.Titulo, Location = new Point(24, 72), Size = new Size(460, 32), BorderStyle = BorderStyle.FixedSingle };
        cmbTipo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(24, 132),
            Size = new Size(460, 32)
        };
        cmbTipo.Items.AddRange(new[] { "Hardware y software", "Infraestructura" });
        cmbTipo.SelectedItem = item.TipoIncidencia;

        cmbEstado = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(24, 192),
            Size = new Size(460, 32)
        };
        cmbEstado.Items.AddRange(new[] { "Activa", "En proceso", "Resuelta" });
        cmbEstado.SelectedItem = item.Estado;

        Controls.AddRange(new Control[]
        {
            Etiqueta("Título", 48), txtTitulo,
            Etiqueta("Tipo de incidencia", 108), cmbTipo,
            Etiqueta("Estado", 168), cmbEstado
        });

        var btnCancelar = new Button
        {
            Text = "Cancelar",
            DialogResult = DialogResult.Cancel,
            Location = new Point(24, 232),
            Size = new Size(120, 36)
        };
        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(364, 232),
            Size = new Size(120, 36),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += (_, _) =>
        {
            _item.Titulo = txtTitulo.Text.Trim();
            _item.TipoIncidencia = cmbTipo.SelectedItem?.ToString() ?? _item.TipoIncidencia;
            _item.Estado = cmbEstado.SelectedItem?.ToString() ?? _item.Estado;
            IncidenciaListadoStore.Actualizar(_item);
            DialogResult = DialogResult.OK;
            Close();
        };
        Controls.AddRange(new Control[] { btnCancelar, btnGuardar });
        CancelButton = btnCancelar;
    }

    private static Label Etiqueta(string texto, int y) => new()
    {
        Text = texto,
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
        ForeColor = UiAssets.AzulOscuro,
        Location = new Point(24, y),
        AutoSize = true
    };
}
