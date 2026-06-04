using Proyecto_GALAB.Models;
using System.Drawing.Drawing2D;

namespace Proyecto_GALAB.Views.Admin;

public class AlumnoDetalleForm : Form
{
    public AlumnoDetalleForm(PerfilUsuario perfil)
    {
        Text = "Ver datos del alumno - " + perfil.ControlNumber;
        Size = new Size(540, 680);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        // Header Panel
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = UiAssets.AzulPrincipal
        };
        
        var lblTitle = new Label
        {
            Text = "PERFIL DE ESTUDIANTE",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(24, 16),
            AutoSize = true
        };
        var lblSubtitle = new Label
        {
            Text = $"Control: {perfil.ControlNumber} | {perfil.NombreCompleto}",
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(220, 235, 255),
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

        int currentY = 16;
        currentY = AgregarSeccion(container, "INFORMACIÓN ACADÉMICA", currentY, new[]
        {
            ("Número de Control", perfil.ControlNumber),
            ("Nombre Completo", perfil.NombreCompleto),
            ("Carrera", perfil.Carrera),
            ("Semestre", perfil.Semestre),
            ("Grupo", perfil.Grupo),
            ("Correo Institucional", perfil.Correo),
            ("Estatus", perfil.Rol)
        });

        currentY = AgregarSeccion(container, "INFORMACIÓN PERSONAL", currentY, new[]
        {
            ("CURP", perfil.Curp),
            ("Fecha de Nacimiento", perfil.FechaNacimiento),
            ("Género", perfil.Genero),
            ("Teléfono", perfil.Telefono)
        });

        currentY = AgregarSeccion(container, "DIRECCIÓN Y UBICACIÓN", currentY, new[]
        {
            ("Calle y Número", perfil.Calle),
            ("Colonia", perfil.Colonia),
            ("Código Postal", perfil.CodigoPostal),
            ("Municipio", perfil.Municipio),
            ("Estado", perfil.Estado)
        });

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
        
        // Fix hierarchy to show bottom and header correctly
        bottom.SendToBack();
        header.SendToBack();
        container.BringToFront();
    }

    private int AgregarSeccion(Panel parent, string title, int startY, (string Label, string Value)[] items)
    {
        var panelSec = new Panel
        {
            Location = new Point(12, startY),
            Size = new Size(470, 36 + items.Length * 28),
            BackColor = Color.White
        };
        panelSec.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawRectangle(pen, 0, 0, panelSec.Width - 1, panelSec.Height - 1);
        };
        UiAssets.RedondearControl(panelSec, 8);

        panelSec.Controls.Add(new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 8),
            AutoSize = true
        });

        int y = 32;
        foreach (var item in items)
        {
            panelSec.Controls.Add(new Label
            {
                Text = $"{item.Label}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(16, y),
                Size = new Size(160, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });

            panelSec.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(item.Value) ? "No especificado" : item.Value,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 60, 80),
                Location = new Point(180, y),
                Size = new Size(270, 20),
                TextAlign = ContentAlignment.MiddleLeft
            });
            y += 24;
        }

        parent.Controls.Add(panelSec);
        return startY + panelSec.Height + 16;
    }
}
