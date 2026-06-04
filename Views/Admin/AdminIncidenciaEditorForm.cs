using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminIncidenciaEditorForm : Form
{
    private readonly IncidenciaListadoItem _item;
    private TextBox txtSolucion = null!;
    private ComboBox cmbEstado = null!;

    public AdminIncidenciaEditorForm(IncidenciaListadoItem item)
    {
        _item = item;
        Text = "Editar solución de incidencia";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(550, 480);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        Controls.Add(new Label
        {
            Text = $"Editar Incidencia: {item.Folio}",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 16),
            AutoSize = true
        });

        // Título original (solo lectura)
        var txtTituloOriginal = new TextBox
        {
            Text = item.Titulo,
            Location = new Point(24, 75),
            Size = new Size(500, 26),
            BorderStyle = BorderStyle.FixedSingle,
            ReadOnly = true,
            BackColor = Color.FromArgb(240, 240, 240)
        };

        // Descripción original (solo lectura, multilínea)
        var txtDescOriginal = new TextBox
        {
            Text = item.Descripcion,
            Location = new Point(24, 135),
            Size = new Size(500, 80),
            BorderStyle = BorderStyle.FixedSingle,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            BackColor = Color.FromArgb(240, 240, 240)
        };

        // Estado (editable)
        cmbEstado = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(24, 250),
            Size = new Size(500, 28)
        };
        cmbEstado.Items.AddRange(new[] { "Activa", "En proceso", "Resuelta" });
        cmbEstado.SelectedItem = item.Estado;

        // Descripción de la solución (editable, multilínea)
        txtSolucion = new TextBox
        {
            Text = item.DescripcionSolucion,
            Location = new Point(24, 310),
            Size = new Size(500, 100),
            BorderStyle = BorderStyle.FixedSingle,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };

        Controls.AddRange(new Control[]
        {
            Etiqueta("Título del reporte", 55), txtTituloOriginal,
            Etiqueta("Descripción del reporte", 115), txtDescOriginal,
            Etiqueta("Estado", 230), cmbEstado,
            Etiqueta("Descripción de solución (mensaje para el alumno)", 290), txtSolucion
        });

        var btnCancelar = new Button
        {
            Text = "Cancelar",
            DialogResult = DialogResult.Cancel,
            Location = new Point(24, 430),
            Size = new Size(120, 36)
        };
        var btnGuardar = new Button
        {
            Text = "Guardar",
            Location = new Point(404, 430),
            Size = new Size(120, 36),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += (_, _) =>
        {
            _item.Estado = cmbEstado.SelectedItem?.ToString() ?? _item.Estado;
            _item.DescripcionSolucion = txtSolucion.Text.Trim();
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
