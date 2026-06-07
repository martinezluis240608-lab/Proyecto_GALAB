using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Proyecto_GALAB.Views;

public class CustomMessageBox : Form
{
    private Label lblTitulo = null!;
    private Label lblMensaje = null!;
    private PictureBox picIcono = null!;
    private Button btnAceptar = null!;
    private Button btnCancelar = null!;
    private Panel panelSuperior = null!;

    public static DialogResult Show(string text, string caption = "Aviso", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
    {
        using var msgBox = new CustomMessageBox(text, caption, buttons, icon);
        return msgBox.ShowDialog();
    }

    private CustomMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.White;
        this.MinimumSize = new Size(400, 200);
        this.MaximumSize = new Size(600, 400);

        // Borde de la ventana (para pintar en el Paint)
        this.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(200, 200, 200), 2);
            e.Graphics.DrawRectangle(pen, 1, 1, this.Width - 2, this.Height - 2);
        };

        // Panel superior
        panelSuperior = new Panel
        {
            Height = 45,
            Dock = DockStyle.Top,
            BackColor = UiAssets.AzulPrincipal
        };
        
        lblTitulo = new Label
        {
            Text = caption,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(15, 0, 0, 0)
        };
        panelSuperior.Controls.Add(lblTitulo);

        // Configurar ícono según MessageBoxIcon
        picIcono = new PictureBox
        {
            Size = new Size(60, 60),
            Location = new Point(25, 65),
            SizeMode = PictureBoxSizeMode.Zoom
        };
        
        Color iconColor = Color.Gray;
        string iconoChar = "ℹ️";
        switch (icon)
        {
            case MessageBoxIcon.Error:
                iconColor = Color.FromArgb(220, 50, 50);
                iconoChar = "❌";
                panelSuperior.BackColor = iconColor;
                break;
            case MessageBoxIcon.Warning:
                iconColor = Color.FromArgb(235, 145, 12);
                iconoChar = "⚠️";
                panelSuperior.BackColor = iconColor;
                break;
            case MessageBoxIcon.Question:
                iconColor = Color.FromArgb(0, 120, 215);
                iconoChar = "❓";
                break;
            case MessageBoxIcon.Information:
            default:
                iconColor = Color.FromArgb(34, 166, 88);
                iconoChar = "✔️";
                break;
        }

        // Crear una imagen temporal para el emoji
        var bmp = new Bitmap(60, 60);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            using var f = new Font("Segoe UI Emoji", 30F);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(iconoChar, f, Brushes.Black, new Rectangle(0, 0, 60, 60), sf);
        }
        picIcono.Image = bmp;

        // Texto principal
        lblMensaje = new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(50, 50, 50),
            Location = new Point(100, 65),
            AutoSize = false,
            Width = 270,
            Height = 80,
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Medir tamaño del texto
        using (var g = this.CreateGraphics())
        {
            var size = g.MeasureString(text, lblMensaje.Font, lblMensaje.Width);
            if (size.Height > 80)
            {
                lblMensaje.Height = (int)size.Height + 20;
                this.Height = lblMensaje.Bottom + 70; // Expandir formulario
            }
        }

        // Botones
        int btnWidth = 120;
        int btnHeight = 40;
        int btnY = this.Height - btnHeight - 20;

        btnAceptar = new Button
        {
            Text = "Aceptar",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            BackColor = panelSuperior.BackColor, // Usar el mismo color del header
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(btnWidth, btnHeight),
            Cursor = Cursors.Hand,
            DialogResult = DialogResult.OK
        };
        btnAceptar.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btnAceptar, 6);

        if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.OKCancel)
        {
            btnAceptar.Text = "Sí";
            btnAceptar.DialogResult = DialogResult.Yes;

            btnCancelar = new Button
            {
                Text = buttons == MessageBoxButtons.YesNo ? "No" : "Cancelar",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Cursor = Cursors.Hand,
                DialogResult = buttons == MessageBoxButtons.YesNo ? DialogResult.No : DialogResult.Cancel
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            UiAssets.RedondearControl(btnCancelar, 6);

            btnAceptar.Location = new Point(this.Width / 2 - btnWidth - 10, btnY);
            btnCancelar.Location = new Point(this.Width / 2 + 10, btnY);

            this.Controls.Add(btnCancelar);
        }
        else
        {
            btnAceptar.Location = new Point((this.Width - btnWidth) / 2, btnY);
        }

        this.Controls.Add(panelSuperior);
        this.Controls.Add(picIcono);
        this.Controls.Add(lblMensaje);
        this.Controls.Add(btnAceptar);

        this.AcceptButton = btnAceptar;
    }
}
