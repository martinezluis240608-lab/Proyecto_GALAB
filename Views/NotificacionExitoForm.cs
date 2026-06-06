using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Proyecto_GALAB.Views;

public class NotificacionExitoForm : Form
{
    private readonly string _mensaje;
    private readonly string _titulo;

    public NotificacionExitoForm(string titulo, string mensaje)
    {
        _titulo = titulo;
        _mensaje = mensaje;

        Text = titulo;
        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(460, 200);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        ShowInTaskbar = false;

        CrearInterfaz();
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
            return cp;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        UiAssets.RedondearControl(this, 12);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    private void CrearInterfaz()
    {
        var pbIcono = new PictureBox
        {
            Size = new Size(48, 48),
            Location = new Point(24, 24),
            BackColor = Color.Transparent
        };
        pbIcono.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(Color.FromArgb(220, 252, 231));
            using var pen = new Pen(Color.FromArgb(134, 239, 172), 1);
            e.Graphics.FillEllipse(brush, 0, 0, 46, 46);
            e.Graphics.DrawEllipse(pen, 0, 0, 46, 46);

            using var font = new Font("Segoe UI", 20F, FontStyle.Bold);
            using var brushText = new SolidBrush(Color.FromArgb(34, 197, 94));
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString("✓", font, brushText, new RectangleF(0, 0, 46, 46), sf);
        };

        var lblTitulo = new Label
        {
            Text = _titulo,
            Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39),
            Location = new Point(88, 24),
            Size = new Size(348, 30),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblMensaje = new Label
        {
            Text = _mensaje,
            Font = new Font("Segoe UI", 10.5F),
            ForeColor = Color.FromArgb(75, 85, 99),
            Location = new Point(88, 56),
            Width = 348,
            AutoSize = true,
            MaximumSize = new Size(348, 400)
        };

        Controls.Add(lblMensaje);

        int btnAceptarY = lblMensaje.Bottom + 24;
        var btnAceptar = new Button
        {
            Text = "Excelente",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(120, 38),
            Location = new Point(316, btnAceptarY),
            Cursor = Cursors.Hand
        };
        btnAceptar.FlatAppearance.BorderSize = 0;
        btnAceptar.Click += (s, e) => Close();
        UiAssets.RedondearControl(btnAceptar, 6);

        btnAceptar.MouseEnter += (s, e) => btnAceptar.BackColor = UiAssets.AzulOscuro;
        btnAceptar.MouseLeave += (s, e) => btnAceptar.BackColor = UiAssets.AzulPrincipal;

        this.Height = btnAceptar.Bottom + 24;

        Controls.AddRange(new Control[] { pbIcono, lblTitulo, btnAceptar });
    }

    public static void Mostrar(Form parent, string titulo, string mensaje)
    {
        using var dlg = new NotificacionExitoForm(titulo, mensaje);
        dlg.ShowDialog(parent);
    }
}
