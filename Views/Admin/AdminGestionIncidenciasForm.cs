using System.Drawing.Drawing2D;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminGestionIncidenciasForm : Form
{
    private DataGridView grid = null!;
    private TextBox txtBuscar = null!;
    private ComboBox cmbEstado = null!;
    private Label lblPaginacion = null!;
    private Label lblTotalCard = null!;
    private Label lblActivasCard = null!;
    private Label lblProcesoCard = null!;
    private Label lblResueltasCard = null!;
    private int paginaActual = 1;
    private const int FilasPorPagina = 5;

    public AdminGestionIncidenciasForm()
    {
        Text = "GALAB - Gestión de incidencias";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        UiAssets.PrepararPantallaCompleta(this);
        CrearInterfaz();
        RefrescarTarjetas();
        CargarGrid();
    }

    private void CrearInterfaz()
    {
        var header = AdminSidebar.CrearHeader();
        var sidebar = AdminSidebar.Crear(this, AdminModulo.Incidencias);
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
        Controls.Add(contenido);
        Controls.Add(sidebar);
        Controls.Add(header);
        Controls.Add(footer);
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo, AutoScroll = true };

        var titulo = new Label
        {
            Text = "Gestión de incidencias",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(36, 24),
            AutoSize = true
        };
        var subrayado = new Panel { BackColor = UiAssets.AzulPrincipal, Location = new Point(38, 62), Size = new Size(62, 4) };

        var resumen = new IncidenciaEstadisticasService().ObtenerResumen();
        var cardTotal = CrearTarjetaMetrica("Total", resumen.Total, "incidencias", "📋", Color.FromArgb(0, 82, 170), 36, 88);
        lblTotalCard = (Label)cardTotal.Controls[1];
        var cardActivas = CrearTarjetaMetrica("Activas", resumen.Activas, "incidencias", "👤", Color.FromArgb(210, 30, 55), 280, 88);
        lblActivasCard = (Label)cardActivas.Controls[1];
        var cardProceso = CrearTarjetaMetrica("En proceso", resumen.EnProceso, "incidencias", "◷", Color.FromArgb(235, 145, 12), 524, 88);
        lblProcesoCard = (Label)cardProceso.Controls[1];
        var cardResueltas = CrearTarjetaMetrica("Resueltas", resumen.Resueltas, "incidencias", "✓", Color.FromArgb(10, 170, 55), 768, 88);
        lblResueltasCard = (Label)cardResueltas.Controls[1];

        var cardLista = new Panel
        {
            BackColor = Color.White,
            Location = new Point(36, 210),
            Size = new Size(1100, 480)
        };
        cardLista.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, cardLista.Width - 1, cardLista.Height - 1), 10);
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawPath(pen, path);
        };

        var lblListado = new Label
        {
            Text = "Listado de incidencias",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(20, 16),
            AutoSize = true
        };

        txtBuscar = new TextBox
        {
            PlaceholderText = "Buscar incidencia...",
            Font = new Font("Segoe UI", 10.5F),
            Location = new Point(20, 52),
            Size = new Size(280, 34),
            BorderStyle = BorderStyle.FixedSingle
        };
        txtBuscar.TextChanged += (_, _) => { paginaActual = 1; CargarGrid(); };

        cmbEstado = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10F),
            Location = new Point(320, 52),
            Size = new Size(180, 34)
        };
        cmbEstado.Items.AddRange(new object[] { "Estado: Todos", "Activa", "En proceso", "Resuelta" });
        cmbEstado.SelectedIndex = 0;
        cmbEstado.SelectedIndexChanged += (_, _) => { paginaActual = 1; CargarGrid(); };

        var btnNueva = new Button
        {
            Text = "+  Nueva incidencia",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(190, 38),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        btnNueva.FlatAppearance.BorderSize = 0;
        btnNueva.Click += (_, _) =>
        {
            var form = new IncidenciaForm();
            UiAssets.PrepararPantallaCompleta(form);
            form.FormClosed += (_, _) => { RefrescarTarjetas(); CargarGrid(); };
            form.Show();
        };
        UiAssets.RedondearControl(btnNueva, 8);

        grid = CrearGrid();
        grid.CellContentClick += Grid_CellContentClick;
        grid.CellFormatting += Grid_CellFormatting;

        lblPaginacion = new Label
        {
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(80, 90, 110),
            Location = new Point(20, 430),
            AutoSize = true
        };

        var btnPrev = new Button { Text = "«", Size = new Size(36, 32), Location = new Point(900, 424) };
        var btnNext = new Button { Text = "»", Size = new Size(36, 32), Location = new Point(1040, 424) };
        btnPrev.Click += (_, _) => { if (paginaActual > 1) { paginaActual--; CargarGrid(); } };
        btnNext.Click += (_, _) => { paginaActual++; CargarGrid(); };

        cardLista.Controls.AddRange(new Control[] { lblListado, txtBuscar, cmbEstado, btnNueva, grid, lblPaginacion, btnPrev, btnNext });
        cardLista.Resize += (s, e) =>
        {
            btnNueva.Left = cardLista.Width - btnNueva.Width - 20;
            btnNueva.Top = 52;
            grid.Width = cardLista.Width - 40;
            grid.Height = cardLista.Height - 120;
            btnPrev.Left = cardLista.Width - 180;
            btnNext.Left = cardLista.Width - 60;
        };

        panel.Controls.AddRange(new Control[] { titulo, subrayado, cardTotal, cardActivas, cardProceso, cardResueltas, cardLista });
        panel.Resize += (s, e) =>
        {
            int w = Math.Max(1000, panel.ClientSize.Width - 72);
            int startX = (panel.ClientSize.Width - w) / 2;
            int gap = (w - 4 * 220) / 3;
            titulo.Left = startX;
            subrayado.Left = startX + 2;
            cardTotal.Left = startX;
            cardActivas.Left = startX + 220 + gap;
            cardProceso.Left = startX + (220 + gap) * 2;
            cardResueltas.Left = startX + (220 + gap) * 3;
            cardLista.Left = startX;
            cardLista.Width = w;
            cardLista.Height = Math.Max(420, panel.ClientSize.Height - 250);
        };

        return panel;
    }

    private static Panel CrearTarjetaMetrica(string titulo, int valor, string unidad, string icono, Color color, int x, int y)
    {
        var card = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(220, 100),
            BackColor = Color.White
        };
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 227, 238));
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };
        card.Controls.Add(new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(50, 58, 72),
            Location = new Point(16, 12),
            AutoSize = true
        });
        card.Controls.Add(new Label
        {
            Text = $"{valor} {unidad}",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = color,
            Location = new Point(16, 40),
            AutoSize = true,
            Tag = "valor"
        });
        card.Controls.Add(new Label
        {
            Text = icono,
            Font = new Font("Segoe UI Symbol", 22F, FontStyle.Bold),
            ForeColor = color,
            Location = new Point(170, 38),
            Size = new Size(40, 40),
            TextAlign = ContentAlignment.MiddleCenter
        });
        return card;
    }

    private DataGridView CrearGrid()
    {
        var g = new DataGridView
        {
            Location = new Point(20, 96),
            Size = new Size(1060, 320),
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeight = 42,
            RowTemplate = { Height = 44 },
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        g.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        g.GridColor = Color.FromArgb(230, 235, 242);

        g.Columns.Add("Folio", "Folio");
        g.Columns.Add("Titulo", "Título");
        g.Columns.Add("Tipo", "Tipo de incidencia");
        g.Columns.Add("Estado", "Estado");
        g.Columns.Add("Fecha", "Fecha");
        var colVer = new DataGridViewButtonColumn { Name = "Ver", HeaderText = "Acciones", Text = "👁", UseColumnTextForButtonValue = true, Width = 50 };
        var colEdit = new DataGridViewButtonColumn { Name = "Editar", Text = "✎", UseColumnTextForButtonValue = true, Width = 50 };
        var colDel = new DataGridViewButtonColumn { Name = "Eliminar", Text = "🗑", UseColumnTextForButtonValue = true, Width = 50 };
        g.Columns.Add(colVer);
        g.Columns.Add(colEdit);
        g.Columns.Add(colDel);
        g.Columns["Folio"]!.FillWeight = 90;
        g.Columns["Titulo"]!.FillWeight = 140;
        g.Columns["Tipo"]!.FillWeight = 110;
        g.Columns["Estado"]!.FillWeight = 80;
        g.Columns["Fecha"]!.FillWeight = 70;
        return g;
    }

    private void RefrescarTarjetas()
    {
        var r = new IncidenciaEstadisticasService().ObtenerResumen();
        lblTotalCard.Text = $"{r.Total} incidencias";
        lblActivasCard.Text = $"{r.Activas} incidencias";
        lblProcesoCard.Text = $"{r.EnProceso} incidencias";
        lblResueltasCard.Text = $"{r.Resueltas} incidencias";
    }

    private List<IncidenciaListadoItem> Filtrar()
    {
        var lista = IncidenciaListadoStore.ObtenerTodas().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
        {
            var q = txtBuscar.Text.Trim();
            lista = lista.Where(i =>
                i.Folio.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.Titulo.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                i.TipoIncidencia.Contains(q, StringComparison.OrdinalIgnoreCase));
        }
        if (cmbEstado.SelectedIndex > 0)
        {
            var estado = cmbEstado.SelectedItem?.ToString() ?? "";
            lista = lista.Where(i => i.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));
        }
        return lista.OrderByDescending(i => i.Fecha).ToList();
    }

    private void CargarGrid()
    {
        var filtrada = Filtrar();
        int total = filtrada.Count;
        int totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)FilasPorPagina));
        if (paginaActual > totalPaginas) paginaActual = totalPaginas;

        var pagina = filtrada.Skip((paginaActual - 1) * FilasPorPagina).Take(FilasPorPagina).ToList();
        grid.Rows.Clear();
        foreach (var item in pagina)
        {
            grid.Rows.Add(item.Folio, item.Titulo, item.TipoIncidencia, item.Estado,
                item.Fecha.ToString("dd/MM/yyyy"), "👁", "✎", "🗑");
            grid.Rows[grid.Rows.Count - 1].Tag = item;
        }

        if (grid.Rows.Count == 0)
            grid.Rows.Add("—", "No hay incidencias registradas", "—", "—", "—", "", "", "");

        int desde = total == 0 ? 0 : (paginaActual - 1) * FilasPorPagina + 1;
        int hasta = Math.Min(paginaActual * FilasPorPagina, total);
        lblPaginacion.Text = $"Mostrando {desde} a {hasta} de {total} incidencias";
        RefrescarTarjetas();
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "Estado") return;
        var estado = e.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = Color.White;
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        e.CellStyle.BackColor = estado switch
        {
            "Resuelta" => Color.FromArgb(34, 166, 88),
            "En proceso" => Color.FromArgb(235, 145, 12),
            _ => Color.FromArgb(0, 120, 215)
        };
    }

    private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var item = grid.Rows[e.RowIndex].Tag as IncidenciaListadoItem;
        if (item == null) return;

        var col = grid.Columns[e.ColumnIndex].Name;
        if (col == "Ver")
        {
            MessageBox.Show(
                $"Folio: {item.Folio}\nTítulo: {item.Titulo}\nTipo: {item.TipoIncidencia}\nEstado: {item.Estado}\nFecha: {item.Fecha:dd/MM/yyyy}\nEquipo: {item.Equipo}\n\n{item.Descripcion}",
                "Detalle de incidencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else if (col == "Editar")
        {
            using var dlg = new AdminIncidenciaEditorForm(item);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                CargarGrid();
        }
        else if (col == "Eliminar")
        {
            if (MessageBox.Show($"¿Eliminar la incidencia {item.Folio}?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                IncidenciaListadoStore.Eliminar(item.Folio);
                CargarGrid();
            }
        }
    }

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        RefrescarTarjetas();
        CargarGrid();
    }
}
