using Proyecto_GALAB.Models;
using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views;

public class IncidenciaDetalleForm : Form
{
    public IncidenciaDetalleForm(IncidenciaListadoItem item)
    {
        Text = "Detalle de incidencia - " + item.Folio;
        Size = new Size(540, 620);
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
            Size = new Size(470, 240),
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

        string[] labels = { "Título", "Tipo", "Pc/portátil afectado", "Fecha de reporte" };
        string[] values = { item.Titulo, item.TipoIncidencia, item.Equipo, item.Fecha.ToString("dd/MM/yyyy HH:mm") };
        int y = 32;
        for (int i = 0; i < labels.Length; i++)
        {
            panelReporte.Controls.Add(new Label
            {
                Text = $"{labels[i]}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(16, y),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });
            panelReporte.Controls.Add(new Label
            {
                Text = values[i],
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 60, 80),
                Location = new Point(170, y),
                Size = new Size(280, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });
            y += 24;
        }

        // Descripcion Textbox multiline
        panelReporte.Controls.Add(new Label
        {
            Text = "Descripción del problema:",
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(16, y),
            Size = new Size(400, 20)
        });
        y += 22;
        
        var txtDesc = new TextBox
        {
            Text = item.Descripcion,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(16, y),
            Size = new Size(434, 70),
            BackColor = Color.FromArgb(248, 250, 252)
        };
        panelReporte.Controls.Add(txtDesc);
        container.Controls.Add(panelReporte);

        // Card 3: Student Details
        var panelUsuario = new Panel
        {
            Location = new Point(12, 272),
            Size = new Size(470, 180),
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

        var perfilRep = Proyecto_GALAB.Services.PerfilUsuarioStore.ObtenerPorNombre(item.QuienReporta);
        string[] uLabels = { "Nombre", "Nº control", "Carrera", "Semestre / Grupo", "Correo", "Teléfono" };
        string[] uValues = { 
            perfilRep.NombreCompleto, 
            string.IsNullOrWhiteSpace(perfilRep.ControlNumber) ? "N/A" : perfilRep.ControlNumber, 
            string.IsNullOrWhiteSpace(perfilRep.Carrera) ? "N/A" : perfilRep.Carrera, 
            $"{(string.IsNullOrWhiteSpace(perfilRep.Semestre) ? "N/A" : perfilRep.Semestre)}º - {(string.IsNullOrWhiteSpace(perfilRep.Grupo) ? "N/A" : perfilRep.Grupo)}",
            string.IsNullOrWhiteSpace(perfilRep.Correo) ? "N/A" : perfilRep.Correo, 
            string.IsNullOrWhiteSpace(perfilRep.Telefono) ? "N/A" : perfilRep.Telefono 
        };

        int uy = 32;
        for (int i = 0; i < uLabels.Length; i++)
        {
            panelUsuario.Controls.Add(new Label
            {
                Text = $"{uLabels[i]}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(16, uy),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });
            panelUsuario.Controls.Add(new Label
            {
                Text = uValues[i],
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 60, 80),
                Location = new Point(170, uy),
                Size = new Size(280, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });
            uy += 24;
        }
        container.Controls.Add(panelUsuario);

        // Card 2: Solution details
        var panelSolucion = new Panel
        {
            Location = new Point(12, 468),
            Size = new Size(470, 160),
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

        bool resuelta = !string.IsNullOrWhiteSpace(item.DescripcionSolucion);
        var txtSol = new TextBox
        {
            Text = resuelta ? item.DescripcionSolucion : "Pendiente de revisión por el administrador.",
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(16, 32),
            Size = new Size(434, 110),
            BackColor = resuelta ? Color.FromArgb(244, 253, 247) : Color.FromArgb(254, 250, 242),
            ForeColor = resuelta ? Color.FromArgb(30, 80, 45) : Color.FromArgb(120, 75, 10)
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
            DialogResult = DialogResult.OK,
            Size = new Size(120, 36),
            Location = new Point(380, 12),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        bottom.Controls.Add(btnCerrar);
        Controls.Add(bottom);
        AcceptButton = btnCerrar;

        bottom.SendToBack();
        header.SendToBack();
        container.BringToFront();
    }
}
