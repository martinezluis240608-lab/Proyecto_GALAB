using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Npgsql;

namespace Proyecto_GALAB.Views;

public class NotificacionForm : Form
{
    private readonly string _mensaje;
    private readonly string _campo;
    private readonly string _titulo;

    public NotificacionForm(string titulo, string mensaje, string campo)
    {
        _titulo = titulo;
        _mensaje = mensaje;
        _campo = campo;

        Text = titulo;
        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(460, 250);
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
        // Paint subtle border
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
            using var brush = new SolidBrush(Color.FromArgb(254, 242, 242));
            using var pen = new Pen(Color.FromArgb(252, 165, 165), 1);
            e.Graphics.FillEllipse(brush, 0, 0, 46, 46);
            e.Graphics.DrawEllipse(pen, 0, 0, 46, 46);

            using var font = new Font("Segoe UI", 20F, FontStyle.Bold);
            using var brushText = new SolidBrush(Color.FromArgb(239, 68, 68));
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString("!", font, brushText, new RectangleF(0, 0, 46, 46), sf);
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
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(75, 85, 99),
            Location = new Point(88, 56),
            Width = 348,
            AutoSize = true,
            MaximumSize = new Size(348, 400)
        };

        // Add message label to controls so we can use its Bottom property
        Controls.Add(lblMensaje);

        int pnlErrorY = lblMensaje.Bottom + 12;
        var pnlError = new Panel
        {
            Location = new Point(88, pnlErrorY),
            Size = new Size(348, 48),
            BackColor = Color.FromArgb(254, 243, 199)
        };
        pnlError.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(253, 230, 138), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, pnlError.Width - 1, pnlError.Height - 1);
        };

        var lblErrorEtiqueta = new Label
        {
            Text = "Por favor verifique el campo:",
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(146, 64, 14),
            Location = new Point(12, 4),
            AutoSize = true
        };

        var lblErrorCampo = new Label
        {
            Text = _campo,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(180, 83, 9),
            Location = new Point(12, 22),
            AutoSize = true
        };
        pnlError.Controls.AddRange(new Control[] { lblErrorEtiqueta, lblErrorCampo });

        int btnAceptarY = pnlError.Bottom + 16;
        var btnAceptar = new Button
        {
            Text = "Aceptar",
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

        // Hover effect
        btnAceptar.MouseEnter += (s, e) => btnAceptar.BackColor = UiAssets.AzulOscuro;
        btnAceptar.MouseLeave += (s, e) => btnAceptar.BackColor = UiAssets.AzulPrincipal;

        // Set form height based on bottom button position
        this.Height = btnAceptar.Bottom + 24;

        Controls.AddRange(new Control[] { pbIcono, lblTitulo, pnlError, btnAceptar });
    }

    public static void Mostrar(Form parent, string titulo, string mensaje, string campo)
    {
        using var dlg = new NotificacionForm(titulo, mensaje, campo);
        dlg.ShowDialog(parent);
    }

    public static void MostrarExcepcion(Form parent, Exception ex, string tituloPredeterminado = "Atención")
    {
        string titulo = tituloPredeterminado;
        string mensaje = ex.Message;
        string campo = "Datos del formulario";

        Exception? inner = ex;
        while (inner != null)
        {
            if (inner is PostgresException pgEx)
            {
                if (pgEx.SqlState == "23505") // Código de violación de unicidad en PostgreSQL
                {
                    titulo = "Información Duplicada";
                    string constraint = pgEx.ConstraintName ?? "";
                    
                    if (constraint.Contains("correo", StringComparison.OrdinalIgnoreCase))
                    {
                        mensaje = "El correo electrónico ingresado ya se encuentra registrado para otro alumno o administrador.";
                        campo = "Correo electrónico";
                    }
                    else if (constraint.Contains("control", StringComparison.OrdinalIgnoreCase))
                    {
                        mensaje = "El número de control ingresado ya está asignado a otro alumno registrado.";
                        campo = "Número de control";
                    }
                    else if (constraint.Contains("id_alumno", StringComparison.OrdinalIgnoreCase) || constraint.Contains("pkey", StringComparison.OrdinalIgnoreCase))
                    {
                        mensaje = "El identificador del alumno ya existe en el sistema. Se le asignará uno automáticamente.";
                        campo = "ID Alumno (Matrícula)";
                    }
                    else if (constraint.Contains("usuario", StringComparison.OrdinalIgnoreCase))
                    {
                        mensaje = "El nombre de usuario autogenerado ya se encuentra asignado a otra persona.";
                        campo = "Nombre de usuario";
                    }
                    else
                    {
                        mensaje = "Ya existe un registro con algunos de estos datos duplicados en el sistema.";
                        campo = "Campo Único Duplicado";
                    }
                    break;
                }
                else
                {
                    // Para otros errores específicos de base de datos
                    mensaje = pgEx.MessageText;
                    campo = "Base de Datos (" + pgEx.SqlState + ")";
                }
            }
            inner = inner.InnerException;
        }

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            mensaje = "No se pudieron guardar los cambios debido a un inconveniente con los datos del formulario.";
        }

        using var dlg = new NotificacionForm(titulo, mensaje, campo);
        dlg.ShowDialog(parent);
    }
}
