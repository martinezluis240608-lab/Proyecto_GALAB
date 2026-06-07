using Proyecto_GALAB.Models;
using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

public class IncidenciaDetalleForm : Form
{
    public IncidenciaDetalleForm(IncidenciaListadoItem item)
    {
        Text = "Detalle de incidencia - " + item.Folio;
        Size = new Size(560, 720);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        // State color mapping
        Color statusColor = item.Estado switch
        {
            "Resuelta" => Color.FromArgb(34, 166, 88),
            "En proceso" => Color.FromArgb(235, 145, 12),
            _ => Color.FromArgb(0, 120, 215)
        };

        // Header Panel
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = statusColor
        };
        
        var lblTitle = new Label
        {
            Text = $"INCIDENCIA {item.Folio}",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(24, 16),
            AutoSize = true
        };
        var lblSubtitle = new Label
        {
            Text = $"Estado: {item.Estado} | Reportado por: {item.QuienReporta}",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.White,
            Location = new Point(24, 44),
            AutoSize = true
        };
        header.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });
        Controls.Add(header);

        // Scrollable container
        var container = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(24, 12, 24, 20)
        };
        Controls.Add(container);

        // Card 1: Report Details
        var panelReporte = new Panel
        {
            Location = new Point(12, 16),
            Size = new Size(490, 290),
            BackColor = Color.White
        };
        panelReporte.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawRectangle(pen, 0, 0, panelReporte.Width - 1, panelReporte.Height - 1);
        };
        UiAssets.RedondearControl(panelReporte, 8);

        panelReporte.Controls.Add(new Label
        {
            Text = "DETALLES DEL REPORTE",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 8),
            AutoSize = true
        });

        string[] labels = { "ID Incidencia", "Título", "Tipo", "Equipo afectado", "Fecha de reporte" };
        string[] values = { item.IdReal, item.Titulo, item.TipoIncidencia, item.Equipo, item.Fecha.ToString("dd/MM/yyyy HH:mm") };
        int y = 40;
        for (int i = 0; i < labels.Length; i++)
        {
            panelReporte.Controls.Add(new Label
            {
                Text = $"{labels[i]}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(20, y),
                AutoSize = true
            });
            panelReporte.Controls.Add(new Label
            {
                Text = values[i],
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(40, 50, 70),
                Location = new Point(200, y),
                AutoSize = true
            });
            y += 30;
        }

        // Descripcion Textbox multiline
        y += 10;
        panelReporte.Controls.Add(new Label
        {
            Text = "Descripción del problema:",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(20, y),
            AutoSize = true
        });
        y += 24;
        
        var txtDesc = new TextBox
        {
            Text = item.Descripcion,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(20, y),
            Size = new Size(450, 60),
            BackColor = Color.FromArgb(248, 250, 252)
        };
        panelReporte.Controls.Add(txtDesc);
        container.Controls.Add(panelReporte);

        // Card 3: Student Details
        var panelUsuario = new Panel
        {
            Location = new Point(12, 320),
            Size = new Size(490, 190),
            BackColor = Color.White
        };
        panelUsuario.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawRectangle(pen, 0, 0, panelUsuario.Width - 1, panelUsuario.Height - 1);
        };
        UiAssets.RedondearControl(panelUsuario, 8);

        panelUsuario.Controls.Add(new Label
        {
            Text = "DATOS DEL ALUMNO REPORTANTE",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 8),
            AutoSize = true
        });

        string[] uLabels = { "Nombre", "Nº control", "Semestre / Grupo", "Correo", "Teléfono" };
        string[] uValues = { 
            item.QuienReporta, 
            item.NumeroControl, 
            $"{item.Semestre}º - {item.Grupo}",
            item.Correo, 
            item.Telefono
        };

        int uy = 40;
        for (int i = 0; i < uLabels.Length; i++)
        {
            panelUsuario.Controls.Add(new Label
            {
                Text = $"{uLabels[i]}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(20, uy),
                AutoSize = true
            });
            panelUsuario.Controls.Add(new Label
            {
                Text = uValues[i],
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(40, 50, 70),
                Location = new Point(200, uy),
                AutoSize = true
            });
            uy += 28;
        }
        container.Controls.Add(panelUsuario);

        // Card 2: Solution details
        var panelSolucion = new Panel
        {
            Location = new Point(12, 526),
            Size = new Size(490, 160),
            BackColor = Color.White
        };
        panelSolucion.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawRectangle(pen, 0, 0, panelSolucion.Width - 1, panelSolucion.Height - 1);
        };
        UiAssets.RedondearControl(panelSolucion, 8);

        panelSolucion.Controls.Add(new Label
        {
            Text = "SOLUCIÓN / MENSAJE DEL ADMINISTRADOR",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = statusColor,
            Location = new Point(16, 8),
            AutoSize = true
        });

        bool esAdmin = Proyecto_GALAB.Services.SesionActual.EsAdministrador;

        bool resuelta = !string.IsNullOrWhiteSpace(item.DescripcionSolucion);
        var txtSol = new TextBox
        {
            Text = resuelta ? item.DescripcionSolucion : (esAdmin ? "" : "Pendiente de revisión por el administrador."),
            Multiline = true,
            ReadOnly = !esAdmin,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(20, 36),
            Size = new Size(450, 105),
            BackColor = esAdmin ? Color.White : (resuelta ? Color.FromArgb(244, 253, 247) : Color.FromArgb(254, 250, 242)),
            ForeColor = esAdmin ? Color.Black : (resuelta ? Color.FromArgb(30, 80, 45) : Color.FromArgb(120, 75, 10))
        };
        panelSolucion.Controls.Add(txtSol);
        container.Controls.Add(panelSolucion);

        // Bottom panel with Close button
        var bottom = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.White
        };
        bottom.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(UiAssets.Borde), 0, 0, bottom.Width, 0);
        };
        var btnCerrar = new Button
        {
            Text = "Cerrar",
            DialogResult = DialogResult.Cancel,
            Size = new Size(120, 36),
            Location = new Point(410, 12),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        bottom.Controls.Add(btnCerrar);

        if (esAdmin)
        {
            var btnGuardar = new Button
            {
                Text = "Guardar respuesta",
                Size = new Size(140, 36),
                Location = new Point(260, 12),
                BackColor = Color.FromArgb(34, 166, 88),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) => 
            {
                if (string.IsNullOrWhiteSpace(txtSol.Text))
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Escribe una solución o mensaje antes de guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try 
                {
                    using var con = Proyecto_GALAB.Services.DatabaseService.GetConnection();
                    con.Open();
                    using var cmd = new Npgsql.NpgsqlCommand("UPDATE incidencias SET solucion = @sol WHERE id_incidencia = @id", con);
                    cmd.Parameters.AddWithValue("sol", txtSol.Text);
                    cmd.Parameters.AddWithValue("id", long.Parse(item.IdReal));
                    cmd.ExecuteNonQuery();
                    
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Mensaje guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    item.DescripcionSolucion = txtSol.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } 
                catch(Exception ex) 
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            bottom.Controls.Add(btnGuardar);
        }

        Controls.Add(bottom);
        AcceptButton = btnCerrar;

        bottom.SendToBack();
        header.SendToBack();
        container.BringToFront();
    }
}
