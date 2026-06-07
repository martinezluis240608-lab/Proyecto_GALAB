using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminGestionUsuariosForm : Form
{
    private DataGridView grid = null!;
    private TextBox txtBuscar = null!;
    private Label lblPaginacion = null!;
    private int paginaActual = 1;
    private const int FilasPorPagina = 5;

    public AdminGestionUsuariosForm()
    {
        Text = "GALAB - Gestión de usuarios";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        UiAssets.PrepararPantallaCompleta(this);
        CrearInterfaz();
        CargarGrid();
    }

    private void CrearInterfaz()
    {
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo
        };

        var header = AdminSidebar.CrearHeader();
        var sidebar = AdminSidebar.Crear(this, AdminModulo.Usuarios);
        var contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2025 GALAB - Panel administrador",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        };

        // Nest header and contenido into panelDerecho
        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        // Add controls to main Form in correct docking Z-order
        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo, AutoScroll = true };

        var titulo = new Label
        {
            Text = "Gestión de usuarios",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(36, 24),
            AutoSize = true
        };
        var subrayado = new Panel { BackColor = UiAssets.AzulPrincipal, Location = new Point(38, 62), Size = new Size(62, 4) };

        var card = new Panel
        {
            BackColor = Color.White,
            Location = new Point(36, 88),
            Size = new Size(1100, 560)
        };
        card.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 227, 238)), 0, 0, card.Width - 1, card.Height - 1);

        txtBuscar = new TextBox
        {
            PlaceholderText = "Buscar usuario...",
            Location = new Point(20, 20),
            Size = new Size(320, 34),
            BorderStyle = BorderStyle.FixedSingle
        };
        txtBuscar.TextChanged += (_, _) => { paginaActual = 1; CargarGrid(); };

        var btnNuevo = new Button
        {
            Text = "+  Nuevo usuario",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(170, 38),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        btnNuevo.FlatAppearance.BorderSize = 0;
        btnNuevo.Click += (_, _) =>
        {
            using var dlg = new AdminUsuarioEditorForm(null);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                CargarGrid();
        };
        UiAssets.RedondearControl(btnNuevo, 8);

        grid = new DataGridView
        {
            Location = new Point(20, 68),
            Size = new Size(1060, 430),
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeight = 42,
            RowTemplate = { Height = 44 }
        };
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grid.Columns.Add("Id", "ID");
        grid.Columns.Add("Usuario", "Usuario");
        grid.Columns.Add("Contrasena", "Contraseña");
        grid.Columns.Add("Rol", "Rol");
        grid.Columns.Add("Estado", "Estado");
        grid.Columns.Add(new DataGridViewButtonColumn { Name = "Ver", Text = "👁", UseColumnTextForButtonValue = true });
        grid.Columns.Add(new DataGridViewButtonColumn { Name = "Editar", Text = "✎", UseColumnTextForButtonValue = true });
        grid.Columns.Add(new DataGridViewButtonColumn { Name = "Eliminar", Text = "🗑", UseColumnTextForButtonValue = true });
        grid.CellFormatting += Grid_CellFormatting;
        grid.CellContentClick += Grid_CellContentClick;

        lblPaginacion = new Label { Location = new Point(20, 510), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
        var btnPrev = new Button { Text = "«", Size = new Size(36, 32), Location = new Point(900, 504) };
        var btnNext = new Button { Text = "»", Size = new Size(36, 32), Location = new Point(1040, 504) };
        btnPrev.Click += (_, _) => { if (paginaActual > 1) { paginaActual--; CargarGrid(); } };
        btnNext.Click += (_, _) => { paginaActual++; CargarGrid(); };

        card.Controls.AddRange(new Control[] { txtBuscar, btnNuevo, grid, lblPaginacion, btnPrev, btnNext });
        card.Resize += (s, e) =>
        {
            btnNuevo.Left = card.Width - btnNuevo.Width - 20;
            grid.Width = card.Width - 40;
            grid.Height = Math.Max(200, card.Height - 140);
            lblPaginacion.Top = card.Height - 46;
            btnPrev.Top = card.Height - 52;
            btnNext.Top = card.Height - 52;
            btnPrev.Left = card.Width - 180;
            btnNext.Left = card.Width - 60;
        };

        panel.Controls.AddRange(new Control[] { titulo, subrayado, card });
        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int w = Math.Max(1000, panel.ClientSize.Width - 72);
            int startX = (panel.ClientSize.Width - w) / 2;
            titulo.Left = startX;
            subrayado.Left = startX + 2;
            card.Left = startX;
            card.Width = w;
            card.Height = Math.Max(500, panel.ClientSize.Height - 120);
        };
        return panel;
    }

    private void CargarGrid()
    {
        var lista = UsuarioSistemaStore.ObtenerTodos().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
        {
            var q = txtBuscar.Text.Trim();
            lista = lista.Where(u =>
                u.Id.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                u.NombreCompleto.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                u.Correo.Contains(q, StringComparison.OrdinalIgnoreCase));
        }
        var filtrada = lista.ToList();
        int total = filtrada.Count;
        int totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)FilasPorPagina));
        if (paginaActual > totalPaginas) paginaActual = totalPaginas;

        var pagina = filtrada.Skip((paginaActual - 1) * FilasPorPagina).Take(FilasPorPagina);
        grid.Rows.Clear();
        foreach (var u in pagina)
        {
            int idx = grid.Rows.Add(u.Id, u.Usuario, "********", u.Rol, u.Estado, "👁", "✎", "🗑");
            grid.Rows[idx].Tag = u;
        }

        if (grid.Rows.Count == 0)
            grid.Rows.Add("—", "No hay usuarios registrados", "—", "—", "—", "", "", "");

        int desde = total == 0 ? 0 : (paginaActual - 1) * FilasPorPagina + 1;
        int hasta = Math.Min(paginaActual * FilasPorPagina, total);
        lblPaginacion.Text = $"Mostrando {desde} a {hasta} de {total} usuarios";
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "Estado" || e.CellStyle is null) return;
        var activo = e.Value?.ToString()?.Equals("Activo", StringComparison.OrdinalIgnoreCase) == true;
        e.CellStyle.ForeColor = activo ? Color.FromArgb(20, 130, 60) : Color.FromArgb(190, 45, 55);
        e.CellStyle.BackColor = activo ? Color.FromArgb(230, 248, 236) : Color.FromArgb(255, 235, 238);
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var u = grid.Rows[e.RowIndex].Tag as UsuarioSistema;
        if (u == null) return;

        var col = grid.Columns[e.ColumnIndex].Name;
        if (col == "Ver")
        {
            using var dlg = new UsuarioDetalleForm(u.Id, u.Rol);
            dlg.ShowDialog(this);
        }
        else if (col == "Editar")
        {
            using var dlg = new AdminUsuarioEditorForm(u);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                CargarGrid();
        }
        else if (col == "Eliminar")
        {
            if (Proyecto_GALAB.Views.CustomMessageBox.Show($"¿Eliminar usuario {u.Id}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (UsuarioSistemaStore.Eliminar(u.Id, u.Rol))
                {
                    CargarGrid();
                    Proyecto_GALAB.Views.CustomMessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Proyecto_GALAB.Views.CustomMessageBox.Show("No se pudo eliminar el usuario debido a un error en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
