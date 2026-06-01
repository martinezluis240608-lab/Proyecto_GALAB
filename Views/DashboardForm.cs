using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views;

public partial class DashboardForm : Form, IDashboardView
{
    private readonly DashboardPresenter _presenter;

    public int TotalEnviadas   { get; set; }
    public int TotalEnProgreso { get; set; }
    public int TotalResueltas  { get; set; }

    public event EventHandler? OnRegistrarIncidencia;
    public event EventHandler? OnVerLista;
    public event EventHandler? OnCargarDatos;

    public DashboardForm()
    {
        InitializeComponent();
        UiAssets.PrepararPantallaCompleta(this);
        _presenter = new DashboardPresenter(this);
        this.Load += (s, e) => OnCargarDatos?.Invoke(this, EventArgs.Empty);
    }

    // ── Navegación ──────────────────────────────────────────────
    public void NavegarARegistro()
    {
        UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm());
    }

    public void NavegarALista()
    {
        MessageBox.Show("Lista completa de incidencias (próximamente).",
                        "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ── Notificaciones ──────────────────────────────────────────
    public void CargarNotificaciones(List<(string admin, string estado, string tiempo)> notificaciones)
    {
        panelNotificaciones.Controls.Clear();
        int y = 0;
        foreach (var (admin, estado, tiempo) in notificaciones)
        {
            var card = CrearTarjetaNotificacion(admin, estado, tiempo);
            card.Top = y;
            panelNotificaciones.Controls.Add(card);
            y += card.Height + 10;
        }

        // Redibujar gráfica con los datos del presenter
        panelGrafica.Invalidate();
    }

    // ── Tarjeta de notificación ─────────────────────────────────
    private Panel CrearTarjetaNotificacion(string admin, string estado, string tiempo)
    {
        var card = new Panel
        {
            Width     = 230,
            Height    = 90,
            BackColor = Color.White,
            Padding   = new Padding(8)
        };
        card.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(210, 210, 220)), 0, 0, card.Width - 1, card.Height - 1);
        };

        var lblAdmin = new Label
        {
            Text      = "⊙  " + admin,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 80),
            Left = 10, Top = 8, AutoSize = true
        };

        var lblEstado = new Label
        {
            Text      = estado,
            Font      = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(80, 80, 80),
            Left = 10, Top = 30, AutoSize = true
        };

        var lblTiempo = new Label
        {
            Text      = tiempo,
            Font      = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(130, 130, 130),
            Left = 120, Top = 30, AutoSize = true
        };

        var btnVer = new Button
        {
            Text      = "Ver",
            Left = 10, Top = 55,
            Width = 50, Height = 24,
            BackColor = Color.FromArgb(30, 30, 30),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 8F),
            Cursor    = Cursors.Hand
        };
        btnVer.FlatAppearance.BorderSize = 0;

        var btnCerrar = new Button
        {
            Text      = "✕",
            Left = card.Width - 28, Top = 5,
            Width = 22, Height = 22,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.Transparent,
            ForeColor = Color.FromArgb(120, 120, 120),
            Font      = new Font("Segoe UI", 8F),
            Cursor    = Cursors.Hand
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click += (s, e) => panelNotificaciones.Controls.Remove(card);

        card.Controls.AddRange(new Control[] { lblAdmin, lblEstado, lblTiempo, btnVer, btnCerrar });
        return card;
    }

    // ── Botones ─────────────────────────────────────────────────
    private void btnRegistrar_Click(object sender, EventArgs e) =>
        OnRegistrarIncidencia?.Invoke(this, EventArgs.Empty);

    private void btnVerLista_Click(object sender, EventArgs e) =>
        OnVerLista?.Invoke(this, EventArgs.Empty);

    // ── Dibujar gráfica de barras ───────────────────────────────
    private void PanelGrafica_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        int[] valores  = { TotalEnviadas, TotalEnProgreso, TotalResueltas };
        string[] labels = { "Enviadas", "En progreso", "Resueltas" };
        Color[] colores = {
            Color.FromArgb(220, 50,  50),
            Color.FromArgb(230, 200, 50),
            Color.FromArgb(80,  200, 80)
        };

        int panW  = panelGrafica.Width;
        int panH  = panelGrafica.Height;
        int maxVal = Math.Max(valores.Max(), 1);
        int padL = 30, padB = 40, padT = 30, barW = 60, gap = 40;
        int areaH = panH - padT - padB;

        // Líneas guía
        using var penGuia = new Pen(Color.FromArgb(220, 220, 220));
        for (int i = 1; i <= 4; i++)
        {
            int y = padT + areaH - (areaH * i / 4);
            g.DrawLine(penGuia, padL, y, panW - 20, y);
        }

        // Barras
        int totalBars = valores.Length;
        int totalW    = totalBars * barW + (totalBars - 1) * gap;
        int startX    = (panW - totalW) / 2;

        for (int i = 0; i < valores.Length; i++)
        {
            int barH = (int)((double)valores[i] / maxVal * areaH);
            int x    = startX + i * (barW + gap);
            int y    = padT + areaH - barH;

            // Sombra suave
            using var brush = new SolidBrush(colores[i]);
            g.FillRectangle(brush, x, y, barW, barH);

            // Valor encima
            using var fVal = new Font("Segoe UI", 8F, FontStyle.Bold);
            var valStr = valores[i].ToString();
            var sz     = g.MeasureString(valStr, fVal);
            g.DrawString(valStr, fVal, Brushes.Black, x + (barW - sz.Width) / 2, y - sz.Height - 2);

            // Etiqueta abajo
            using var fLbl = new Font("Segoe UI", 8F);
            var lbl = labels[i];
            var ls  = g.MeasureString(lbl, fLbl);
            g.DrawString(lbl, fLbl, Brushes.Gray, x + (barW - ls.Width) / 2, padT + areaH + 6);
        }
    }
}
