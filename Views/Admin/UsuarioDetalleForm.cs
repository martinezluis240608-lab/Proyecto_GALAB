using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class UsuarioDetalleForm : Form
{
    private readonly string _id;
    private readonly string _rol;

    public UsuarioDetalleForm(string id, string rol)
    {
        _id = id;
        _rol = rol;

        Text = $"Detalles del usuario - {_id}";
        Size = new Size(540, 680);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        Controls.Clear();

        bool esAdmin = !_rol.Equals("Usuario", StringComparison.OrdinalIgnoreCase) && 
                       !_rol.Equals("Estudiante", StringComparison.OrdinalIgnoreCase);

        string nombreCompleto = "";
        string subtitulo = "";
        string rolDisplay = esAdmin ? "Administrador" : "Estudiante";

        // Cargar datos
        PerfilUsuario? estudiante = null;
        PerfilAdministrador? admin = null;

        if (esAdmin)
        {
            admin = PerfilAdministradorStore.ObtenerPorId(_id);
            nombreCompleto = $"{admin.Nombre} {admin.PrimerApellido} {admin.SegundoApellido}".Trim();
            subtitulo = $"ID: {admin.IdAdministrador} | Usuario: {admin.Usuario}";
        }
        else
        {
            estudiante = PerfilUsuarioStore.ObtenerPorControl(_id);
            nombreCompleto = estudiante.NombreCompleto;
            subtitulo = $"No. de Control: {estudiante.ControlNumber}";
        }

        // Header Panel con Degradado
        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 140,
            BackColor = UiAssets.AzulPrincipal
        };
        header.Paint += (s, e) =>
        {
            var brush = new LinearGradientBrush(header.ClientRectangle, 
                UiAssets.AzulOscuro, UiAssets.AzulPrincipal, LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, header.ClientRectangle);
        };

        // Generar Iniciales para el Avatar
        string iniciales = "";
        if (!string.IsNullOrWhiteSpace(nombreCompleto))
        {
            var partes = nombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length > 0 && partes[0].Length > 0) iniciales += partes[0][0];
            if (partes.Length > 1 && partes[partes.Length - 1].Length > 0) iniciales += partes[partes.Length - 1][0];
        }
        if (string.IsNullOrWhiteSpace(iniciales)) iniciales = "US";

        // Avatar Panel (Circular y Creativo)
        var pnlAvatar = new Panel
        {
            Size = new Size(80, 80),
            Location = new Point(24, 30),
            BackColor = Color.Transparent
        };
        pnlAvatar.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Dibujar fondo del avatar
            using var brush = new LinearGradientBrush(pnlAvatar.ClientRectangle, 
                Color.FromArgb(232, 243, 255), Color.FromArgb(170, 210, 255), LinearGradientMode.Vertical);
            e.Graphics.FillEllipse(brush, 0, 0, pnlAvatar.Width - 1, pnlAvatar.Height - 1);
            
            // Borde
            using var pen = new Pen(Color.White, 3);
            e.Graphics.DrawEllipse(pen, 1, 1, pnlAvatar.Width - 3, pnlAvatar.Height - 3);

            // Texto iniciales
            var font = new Font("Segoe UI", 24F, FontStyle.Bold);
            var colorTexto = UiAssets.AzulOscuro;
            var size = e.Graphics.MeasureString(iniciales.ToUpper(), font);
            e.Graphics.DrawString(iniciales.ToUpper(), font, new SolidBrush(colorTexto), 
                (pnlAvatar.Width - size.Width) / 2, (pnlAvatar.Height - size.Height) / 2 + 1);
        };
        header.Controls.Add(pnlAvatar);

        // Nombre
        var lblNombre = new Label
        {
            Text = nombreCompleto.ToUpper(),
            Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(120, 24),
            AutoSize = true,
            UseMnemonic = false,
            BackColor = Color.Transparent
        };
        header.Controls.Add(lblNombre);

        // Subtítulo
        var lblSub = new Label
        {
            Text = subtitulo,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(215, 232, 255),
            Location = new Point(120, lblNombre.Bottom + 2),
            AutoSize = true,
            UseMnemonic = false,
            BackColor = Color.Transparent
        };
        header.Controls.Add(lblSub);

        // Badge de Rol (Creativo y de color según tipo)
        var pnlBadge = new Panel
        {
            Location = new Point(120, lblSub.Bottom + 8),
            Size = new Size(130, 24),
            BackColor = esAdmin ? Color.FromArgb(254, 243, 199) : Color.FromArgb(220, 252, 231) // Naranja/Amarillo vs Verde
        };
        pnlBadge.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, pnlBadge.Width - 1, pnlBadge.Height - 1), 6);
            
            // Dibujar Texto del Rol
            var font = new Font("Segoe UI", 9F, FontStyle.Bold);
            var brush = new SolidBrush(esAdmin ? Color.FromArgb(180, 83, 9) : Color.FromArgb(21, 128, 61));
            var size = e.Graphics.MeasureString(rolDisplay, font);
            e.Graphics.DrawString(rolDisplay, font, brush, (pnlBadge.Width - size.Width) / 2, (pnlBadge.Height - size.Height) / 2);
        };
        UiAssets.RedondearControl(pnlBadge, 6);
        header.Controls.Add(pnlBadge);

        Controls.Add(header);

        // Scrollable Content Container
        var container = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(20, 10, 20, 20)
        };
        Controls.Add(container);

        int currentY = 16;

        if (esAdmin && admin != null)
        {
            currentY = AgregarSeccion(container, "DETALLES DE LA CUENTA", currentY, new[]
            {
                ("ID Administrador", admin.IdAdministrador),
                ("Nombre de Usuario", admin.Usuario),
                ("Rol en Sistema", admin.Rol),
                ("Estado de Cuenta", admin.Activo ? "Activo" : "Inactivo"),
                ("Fecha de Ingreso", admin.FechaRegistro.ToString("g"))
            });

            currentY = AgregarSeccion(container, "INFORMACIÓN PERSONAL Y DE CONTACTO", currentY, new[]
            {
                ("Nombre completo", nombreCompleto),
                ("Correo electrónico", admin.Correo),
                ("Teléfono de contacto", admin.Telefono)
            });
        }
        else if (estudiante != null)
        {
            currentY = AgregarSeccion(container, "INFORMACIÓN ACADÉMICA Y DE CUENTA", currentY, new[]
            {
                ("ID Alumno (Matrícula)", estudiante.ControlNumber),
                ("Número de Control", estudiante.NumeroControl.ToString()),
                ("Semestre", estudiante.Semestre),
                ("Grupo", estudiante.Grupo),
                ("Correo Institucional", estudiante.Correo),
                ("Nombre de Usuario", estudiante.Usuario)
            });

            currentY = AgregarSeccion(container, "INFORMACIÓN PERSONAL Y DE CONTACTO", currentY, new[]
            {
                ("Nombre Completo", estudiante.NombreCompleto),
                ("Teléfono", estudiante.Telefono),
                ("Número de Asiento", estudiante.NumeroAsiento?.ToString() ?? "No especificado"),
                ("Estado de Cuenta", estudiante.Activo ? "Activo" : "Inactivo"),
                ("Fecha de Registro", estudiante.FechaRegistro.ToString("g"))
            });
        }

        // Bottom panel con botón Aceptar
        var bottom = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 64,
            BackColor = Color.White
        };
        bottom.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(UiAssets.Borde), 0, 0, bottom.Width, 0);
        };
        var btnAceptar = new Button
        {
            Text = "Aceptar",
            DialogResult = DialogResult.OK,
            Size = new Size(130, 38),
            Location = new Point(370, 13),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnAceptar.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btnAceptar, 8);

        // Animación hover del botón
        btnAceptar.MouseEnter += (s, e) => btnAceptar.BackColor = UiAssets.AzulOscuro;
        btnAceptar.MouseLeave += (s, e) => btnAceptar.BackColor = UiAssets.AzulPrincipal;

        bottom.Controls.Add(btnAceptar);
        Controls.Add(bottom);
        AcceptButton = btnAceptar;

        // Mandar paneles traseros al fondo y subir el contenedor
        bottom.SendToBack();
        header.SendToBack();
        container.BringToFront();
    }

    private int AgregarSeccion(Panel parent, string title, int startY, (string Label, string Value)[] items)
    {
        var panelSec = new Panel
        {
            Location = new Point(12, startY),
            Size = new Size(470, 42 + items.Length * 30),
            BackColor = Color.White
        };
        panelSec.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240));
            e.Graphics.DrawRectangle(pen, 0, 0, panelSec.Width - 1, panelSec.Height - 1);
        };
        UiAssets.RedondearControl(panelSec, 10);

        // Icono de Sección
        var pnlIcon = new Panel
        {
            Location = new Point(16, 12),
            Size = new Size(16, 16)
        };
        pnlIcon.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(UiAssets.AzulPrincipal), 0, 0, pnlIcon.Width - 1, pnlIcon.Height - 1);
        };
        panelSec.Controls.Add(pnlIcon);

        panelSec.Controls.Add(new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(38, 10),
            AutoSize = true
        });

        int y = 38;
        foreach (var item in items)
        {
            panelSec.Controls.Add(new Label
            {
                Text = $"{item.Label}:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = UiAssets.AzulOscuro,
                Location = new Point(16, y),
                Size = new Size(220, 22),
                TextAlign = ContentAlignment.MiddleLeft
            });

            // Si es estado, pintarlo de verde si es Activo, rojo si es Inactivo
            bool esEstado = item.Label.Equals("Estado de Cuenta", StringComparison.OrdinalIgnoreCase);
            Color valorColor = Color.FromArgb(70, 80, 95);
            Font valorFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            if (esEstado)
            {
                bool activo = item.Value.Equals("Activo", StringComparison.OrdinalIgnoreCase);
                valorColor = activo ? Color.FromArgb(21, 128, 61) : Color.FromArgb(185, 28, 28);
                valorFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }

            panelSec.Controls.Add(new Label
            {
                Text = string.IsNullOrWhiteSpace(item.Value) ? "No especificado" : item.Value,
                Font = valorFont,
                ForeColor = valorColor,
                Location = new Point(240, y),
                Size = new Size(210, 22),
                TextAlign = ContentAlignment.MiddleLeft
            });
            y += 26;
        }

        parent.Controls.Add(panelSec);
        return startY + panelSec.Height + 16;
    }
}
