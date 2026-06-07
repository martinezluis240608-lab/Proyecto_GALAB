using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views;

public class RecuperarContrasenaForm : Form
{
    private string _username = "";
    private string _rol = "";
    private string _datoEsperado = "";

    // Controls for Panel 1
    private Panel pnlPaso1 = null!;
    private TextBox txtUsuario = null!;

    // Controls for Panel 2
    private Panel pnlPaso2 = null!;
    private Label lblPregunta = null!;
    private TextBox txtDatoVerificacion = null!;

    // Controls for Panel 3
    private Panel pnlPaso3 = null!;
    private TextBox txtNuevaClave = null!;
    private TextBox txtConfirmarClave = null!;

    public RecuperarContrasenaForm()
    {
        Text = "Recuperar contraseña";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(480, 390);
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);

        InicializarPaso1();
        InicializarPaso2();
        InicializarPaso3();

        // Show Paso 1 initially
        pnlPaso1.Visible = true;
        pnlPaso2.Visible = false;
        pnlPaso3.Visible = false;

        Controls.AddRange(new Control[] { pnlPaso1, pnlPaso2, pnlPaso3 });
    }

    private void InicializarPaso1()
    {
        pnlPaso1 = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo };

        var lblTitulo = new Label
        {
            Text = "Recuperar Contraseña",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 20),
            AutoSize = true
        };

        var lblInstruccion = new Label
        {
            Text = "Ingrese su número de control (si es alumno) o su correo electrónico registrado (si es administrador o personal):",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(70, 80, 95),
            Location = new Point(24, 60),
            Size = new Size(430, 50)
        };

        var lblUsuario = new Label
        {
            Text = "Usuario o Correo electrónico",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(24, 130),
            AutoSize = true
        };

        var pnlInput = CrearCampoTexto(out txtUsuario, 154, false);

        var btnCancelar = CrearBotonSecundario("Cancelar", 24, 310, 120);
        btnCancelar.Click += (s, e) => Close();

        var btnSiguiente = CrearBotonPrincipal("Siguiente", 294, 310, 160);
        btnSiguiente.Click += (s, e) =>
        {
            _username = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(_username))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("Por favor, ingrese su usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UsuarioSistemaStore.ObtenerDatosVerificacion(_username, out _rol, out _datoEsperado, out string tipoPregunta))
            {
                lblPregunta.Text = tipoPregunta;
                pnlPaso1.Visible = false;
                pnlPaso2.Visible = true;
            }
            else
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("El usuario ingresado no existe en el sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        };

        pnlPaso1.Controls.AddRange(new Control[] { lblTitulo, lblInstruccion, lblUsuario, pnlInput, btnCancelar, btnSiguiente });
    }

    private void InicializarPaso2()
    {
        pnlPaso2 = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo };

        var lblTitulo = new Label
        {
            Text = "Verificación de Identidad",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 20),
            AutoSize = true
        };

        var lblInstruccion = new Label
        {
            Text = "Para verificar su identidad y permitirle cambiar la contraseña, por favor responda la siguiente pregunta de seguridad:",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(70, 80, 95),
            Location = new Point(24, 60),
            Size = new Size(430, 50)
        };

        lblPregunta = new Label
        {
            Text = "Pregunta de seguridad...",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(24, 130),
            Size = new Size(430, 24)
        };

        var pnlInput = CrearCampoTexto(out txtDatoVerificacion, 160, false);

        var btnAtras = CrearBotonSecundario("Regresar", 24, 310, 120);
        btnAtras.Click += (s, e) =>
        {
            pnlPaso2.Visible = false;
            pnlPaso1.Visible = true;
        };

        var btnValidar = CrearBotonPrincipal("Validar", 294, 310, 160);
        btnValidar.Click += (s, e) =>
        {
            string respuesta = txtDatoVerificacion.Text.Trim();
            if (string.IsNullOrWhiteSpace(respuesta))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("Por favor, ingrese la respuesta de verificación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (respuesta.Equals(_datoEsperado.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                pnlPaso2.Visible = false;
                pnlPaso3.Visible = true;
            }
            else
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("El dato ingresado no coincide con nuestros registros de seguridad.", "Verificación fallida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        };

        pnlPaso2.Controls.AddRange(new Control[] { lblTitulo, lblInstruccion, lblPregunta, pnlInput, btnAtras, btnValidar });
    }

    private void InicializarPaso3()
    {
        pnlPaso3 = new Panel { Dock = DockStyle.Fill, BackColor = UiAssets.Fondo };

        var lblTitulo = new Label
        {
            Text = "Establecer Contraseña",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 20),
            AutoSize = true
        };

        var lblInstruccion = new Label
        {
            Text = "Identidad verificada correctamente. Por favor ingrese su nueva contraseña:",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(70, 80, 95),
            Location = new Point(24, 60),
            Size = new Size(430, 24)
        };

        var lblClave = new Label
        {
            Text = "Nueva contraseña",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(24, 100),
            AutoSize = true
        };

        var pnlClave = CrearCampoTexto(out txtNuevaClave, 124, true);

        var lblConfirmar = new Label
        {
            Text = "Confirmar contraseña",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(24, 180),
            AutoSize = true
        };

        var pnlConfirmar = CrearCampoTexto(out txtConfirmarClave, 204, true);

        var btnCancelar = CrearBotonSecundario("Cancelar", 24, 310, 120);
        btnCancelar.Click += (s, e) => Close();

        var btnRestablecer = CrearBotonPrincipal("Restablecer", 294, 310, 160);
        btnRestablecer.Click += (s, e) =>
        {
            string clave = txtNuevaClave.Text;
            string confirmar = txtConfirmarClave.Text;

            if (string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(confirmar))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("Por favor, rellene ambos campos de contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clave != confirmar)
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clave.Length < 4)
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UsuarioSistemaStore.ActualizarContrasena(_username, clave, _rol))
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("Contraseña restablecida con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                Proyecto_GALAB.Views.CustomMessageBox.Show("No se pudo actualizar la contraseña. Ocurrió un problema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        pnlPaso3.Controls.AddRange(new Control[] { lblTitulo, lblInstruccion, lblClave, pnlClave, lblConfirmar, pnlConfirmar, btnCancelar, btnRestablecer });
    }

    private Panel CrearCampoTexto(out TextBox txtOut, int y, bool esPassword)
    {
        var container = new Panel
        {
            Location = new Point(24, y),
            Size = new Size(430, 40),
            BackColor = Color.White
        };

        var txt = new TextBox
        {
            Text = "",
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            Location = new Point(10, 10),
            Width = 410,
            UseSystemPasswordChar = esPassword
        };
        txtOut = txt;

        container.Controls.Add(txt);

        bool isHovered = false;

        container.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var colorBorde = txt.Focused ? UiAssets.AzulPrincipal 
                           : isHovered ? Color.FromArgb(160, 185, 220) 
                           : UiAssets.Borde;
            int grosor = txt.Focused ? 2 : 1;
            using var pen = new Pen(colorBorde, grosor);
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, container.Width - 1, container.Height - 1), 6);
            e.Graphics.DrawPath(pen, path);
        };

        txt.GotFocus += (s, e) => container.Invalidate();
        txt.LostFocus += (s, e) => container.Invalidate();

        container.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        container.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };
        txt.MouseEnter += (s, e) => { isHovered = true; container.Invalidate(); };
        txt.MouseLeave += (s, e) => { isHovered = false; container.Invalidate(); };

        UiAssets.RedondearControl(container, 6);

        return container;
    }

    private Button CrearBotonPrincipal(string texto, int x, int y, int w)
    {
        var btn = new Button
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(w, 42),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = UiAssets.AzulPrincipal,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btn.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btn, 6);

        btn.MouseEnter += (s, e) => btn.BackColor = UiAssets.AzulOscuro;
        btn.MouseLeave += (s, e) => btn.BackColor = UiAssets.AzulPrincipal;

        return btn;
    }

    private Button CrearBotonSecundario(string texto, int x, int y, int w)
    {
        var btn = new Button
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(w, 42),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            BackColor = Color.White,
            ForeColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat
        };
        btn.FlatAppearance.BorderColor = UiAssets.Borde;
        btn.FlatAppearance.BorderSize = 1;
        UiAssets.RedondearControl(btn, 6);

        btn.MouseEnter += (s, e) => btn.BackColor = UiAssets.AzulClaro;
        btn.MouseLeave += (s, e) => btn.BackColor = Color.White;

        return btn;
    }
}
