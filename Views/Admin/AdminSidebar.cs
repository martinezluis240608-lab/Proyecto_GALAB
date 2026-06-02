using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

internal enum AdminModulo
{
    Perfil,
    Incidencias,
    Usuarios
}

internal static class AdminSidebar
{
    public static Panel Crear(Form dueño, AdminModulo activo)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 290,
            BackColor = Color.White
        };

        int y = 56;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, activo == AdminModulo.Perfil,
            () => Navegar(dueño, new AdminPerfilForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestión de incidencias", y, activo == AdminModulo.Incidencias,
            () => Navegar(dueño, new AdminGestionIncidenciasForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("👥", "Gestión de usuarios", y, activo == AdminModulo.Usuarios,
            () => Navegar(dueño, new AdminGestionUsuariosForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesión",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 48),
            Location = new Point(40, 575),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        cerrar.FlatAppearance.BorderColor = UiAssets.Borde;
        cerrar.FlatAppearance.BorderSize = 1;
        cerrar.Click += (_, _) =>
        {
            SesionActual.Cerrar();
            UiAssets.CerrarSesion(dueño);
        };
        panel.Resize += (_, _) => cerrar.Top = panel.Height - 78;
        panel.Controls.Add(cerrar);
        UiAssets.RedondearControl(cerrar, 8);

        return panel;
    }

    public static Panel CrearHeader()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 126,
            BackColor = UiAssets.AzulClaro
        };

        var titulo = new Label
        {
            Text = "GALAB — Administración",
            Font = new Font("Segoe UI", 24F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(30, 34),
            AutoSize = true
        };

        var usuario = new Label
        {
            Text = $"Usuario: {SesionActual.NombreUsuario}",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            AutoSize = true
        };
        panel.Resize += (_, _) =>
        {
            usuario.Left = panel.Width - usuario.Width - 30;
            usuario.Top = 48;
        };

        panel.Controls.AddRange(new Control[] { titulo, usuario });
        return panel;
    }

    private static void Navegar(Form actual, Form destino) =>
        UiAssets.AbrirCerrandoActual(actual, destino);
}
