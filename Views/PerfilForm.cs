using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proyecto_GALAB.Views;

public partial class PerfilForm : Form
{
    private TextBox txtNombre = null!;
    private TextBox txtPrimerApellido = null!;
    private TextBox txtSegundoApellido = null!;
    private TextBox txtTelefono = null!;
    private TextBox txtCorreo = null!;
    private TextBox txtNumeroAsiento = null!;
    
    private TextBox txtControl = null!;
    private TextBox txtNumeroControl = null!;
    private TextBox txtSemestre = null!;
    private TextBox txtGrupo = null!;
    private TextBox txtUsuario = null!;
    private TextBox txtEstatus = null!;
    private TextBox txtEstadoCuenta = null!;
    private TextBox txtFechaRegistro = null!;

    private PictureBox pbFotoPerfil = null!;
    private Button btnSubirFoto = null!;
    private string? rutaFotoTemporal;

    private Button btnEditar = null!;
    private Button btnGuardar = null!;
    private Button btnCancelar = null!;
    private bool modoEdicion;
    private PerfilUsuario? respaldoPerfil;

    public PerfilForm()
    {
        InitializeComponent();
        UiAssets.PrepararPantallaCompleta(this);
        Text = "GALAB - Perfil Alumno";
        BackColor = UiAssets.Fondo;
        Font = new Font("Segoe UI", 10F);
        CrearInterfaz();
        CargarPerfilEnVista();
    }

    private void CrearInterfaz()
    {
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo
        };

        var footer = new Label
        {
            Text = "© 2025 GALAB - Instituto Tecnologico Superior de San Miguel el Grande",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        };

        var sidebar = CrearSidebar();
        var header = CrearHeader();
        var contenido = CrearContenido();

        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        Controls.Add(panelDerecho);
        Controls.Add(sidebar); // Dock.Left
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearHeader()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 126,
            BackColor = UiAssets.AzulClaro
        };

        var titulo = new Label
        {
            Text = "Instituto Tecnologico Superior de San Miguel el Grande",
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(30, 34),
            AutoSize = true
        };

        var logo = new PictureBox
        {
            Size = new Size(86, 76),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = UiAssets.CargarLogoInstitucion(),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.Transparent
        };
        var instituto = new Label
        {
            Text = "Instituto Tecnologico\nSuperior de San Miguel\nel Grande",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            AutoSize = true
        };
        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            logo.Left = panel.Width - 390;
            logo.Top = 24;
            instituto.Left = logo.Right + 12;
            instituto.Top = 28;
        };

        panel.Controls.AddRange(new Control[] { titulo, logo, instituto });
        return panel;
    }

    private Panel CrearSidebar()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 290,
            BackColor = Color.White
        };

        int y = 56;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("⌂", "Inicio", y, false, () => UiAssets.AbrirCerrandoActual(this, new PrincipalForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☰", "Gestion de incidencias", y, false, () => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("●", "Perfil", y, true, null));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("☎", "Contacto", y, false, () => UiAssets.AbrirCerrandoActual(this, new ContactoForm())));
        y += 72;
        panel.Controls.Add(UiAssets.CrearBotonSidebar("◎", "Ayuda", y, false, () => UiAssets.AbrirCerrandoActual(this, new AyudaForm())));

        var cerrar = new Button
        {
            Text = "↪  Cerrar sesion",
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
        cerrar.Click += (s, e) => UiAssets.CerrarSesion(this);
        panel.Resize += (s, e) => cerrar.Top = panel.Height - 78;
        panel.Controls.Add(cerrar);
        UiAssets.RedondearControl(cerrar, 8);

        return panel;
    }

    private Panel CrearContenido()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiAssets.Fondo,
            AutoScroll = true
        };

        var cardFoto = CrearSeccion("Foto de perfil", 24, 22, 440, 160);
        var cardGeneral = CrearSeccion("Información personal", 24, 198, 440, 350);
        var cardEscolar = CrearSeccion("Información académica y de cuenta", 480, 22, 440, 526);

        pbFotoPerfil = new PictureBox
        {
            Size = new Size(100, 100),
            Location = new Point(24, 52),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(240, 244, 248)
        };
        UiAssets.RedondearControl(pbFotoPerfil, 50);

        btnSubirFoto = new Button
        {
            Text = "Subir foto",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(130, 36),
            Location = new Point(144, 84),
            Visible = false
        };
        btnSubirFoto.FlatAppearance.BorderColor = UiAssets.Borde;
        btnSubirFoto.FlatAppearance.BorderSize = 1;
        UiAssets.RedondearControl(btnSubirFoto, 6);
        btnSubirFoto.Click += (s, e) => SeleccionarFotoPerfil();

        cardFoto.Controls.AddRange(new Control[] { pbFotoPerfil, btnSubirFoto });

        txtNombre = CrearFila(cardGeneral, "Nombre(s)", 56);
        txtPrimerApellido = CrearFila(cardGeneral, "Primer apellido", 104);
        txtSegundoApellido = CrearFila(cardGeneral, "Segundo apellido", 152);
        txtTelefono = CrearFila(cardGeneral, "Teléfono (10 dígitos)", 200);
        txtTelefono.MaxLength = 10;
        txtCorreo = CrearFila(cardGeneral, "Correo electrónico", 248);
        txtNumeroAsiento = CrearFila(cardGeneral, "Número de asiento", 296);

        txtControl = CrearFila(cardEscolar, "ID Alumno (auto)", 56);
        txtControl.ReadOnly = true;  // Siempre de solo lectura — se asigna automáticamente
        txtControl.ForeColor = Color.FromArgb(130, 140, 160);
        txtNumeroControl = CrearFila(cardEscolar, "Número de control", 104);
        txtSemestre = CrearFila(cardEscolar, "Semestre", 152);
        txtGrupo = CrearFila(cardEscolar, "Grupo", 200);
        txtUsuario = CrearFila(cardEscolar, "Nombre de usuario", 248);
        txtEstatus = CrearFila(cardEscolar, "Rol de sistema", 296);
        txtEstadoCuenta = CrearFila(cardEscolar, "Estado de cuenta", 344);
        txtFechaRegistro = CrearFila(cardEscolar, "Fecha de registro de ingreso", 392);

        txtNombre.KeyPress += SoloLetras_KeyPress;
        txtPrimerApellido.KeyPress += SoloLetras_KeyPress;
        txtSegundoApellido.KeyPress += SoloLetras_KeyPress;
        txtTelefono.KeyPress += SoloNumeros_KeyPress;
        txtNumeroControl.KeyPress += SoloNumeros_KeyPress;
        txtSemestre.KeyPress += SoloNumeros_KeyPress;
        txtNumeroAsiento.KeyPress += SoloNumeros_KeyPress;

        txtNombre.TextChanged += ActualizarUsuarioAutocompletado;
        txtPrimerApellido.TextChanged += ActualizarUsuarioAutocompletado;
        txtSegundoApellido.TextChanged += ActualizarUsuarioAutocompletado;

        panel.Controls.AddRange(new Control[] { cardFoto, cardGeneral, cardEscolar });

        btnEditar = new Button
        {
            Text = "Editar perfil",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = UiAssets.AzulPrincipal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(210, 50),
            Location = new Point(710, 560)
        };
        btnEditar.FlatAppearance.BorderSize = 0;
        btnEditar.Click += (_, _) => ActivarEdicion();
        UiAssets.RedondearControl(btnEditar, 10);

        btnGuardar = new Button
        {
            Text = "Guardar",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(34, 166, 88),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(160, 50),
            Location = new Point(604, 560),
            Visible = false
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += (_, _) => GuardarEdicion();
        UiAssets.RedondearControl(btnGuardar, 10);

        btnCancelar = new Button
        {
            Text = "Cancelar",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Size = new Size(160, 50),
            Location = new Point(772, 560),
            Visible = false
        };
        btnCancelar.FlatAppearance.BorderColor = UiAssets.Borde;
        btnCancelar.FlatAppearance.BorderSize = 1;
        btnCancelar.Click += (_, _) => CancelarEdicion();
        UiAssets.RedondearControl(btnCancelar, 10);

        panel.Controls.AddRange(new Control[] { btnEditar, btnGuardar, btnCancelar });

        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int clientW = panel.ClientSize.Width;
            int clientH = panel.ClientSize.Height;

            int padding = 24;
            int gap = 20;

            int availableW = clientW - (padding * 2) - gap;
            if (availableW < 900) availableW = 900;

            int colWidth = availableW / 2;
            colWidth = Math.Max(400, Math.Min(540, colWidth));

            int startX = padding;
            cardFoto.Location = new Point(startX, padding);
            cardFoto.Size = new Size(colWidth, 160);

            cardGeneral.Location = new Point(startX, cardFoto.Bottom + gap);
            cardGeneral.Size = new Size(colWidth, 350);

            int col2X = startX + colWidth + gap;
            cardEscolar.Location = new Point(col2X, padding);
            cardEscolar.Size = new Size(colWidth, 526);

            btnEditar.Location = new Point(col2X + colWidth - btnEditar.Width, cardEscolar.Bottom + gap);
            btnGuardar.Location = new Point(col2X + colWidth - (btnGuardar.Width * 2 + 10), cardEscolar.Bottom + gap);
            btnCancelar.Location = new Point(col2X + colWidth - btnCancelar.Width, cardEscolar.Bottom + gap);

            panel.AutoScrollMinSize = new Size(col2X + colWidth + padding, btnEditar.Bottom + padding);
        };

        return panel;
    }

    private void ActualizarUsuarioAutocompletado(object? sender, EventArgs e)
    {
        string n = txtNombre.Text.Trim().ToLower();
        string p1 = txtPrimerApellido.Text.Trim().ToLower();
        string p2 = txtSegundoApellido.Text.Trim().ToLower();

        var partes = new List<string>();
        if (!string.IsNullOrEmpty(n)) partes.Add(n);
        if (!string.IsNullOrEmpty(p1)) partes.Add(p1);
        if (!string.IsNullOrEmpty(p2)) partes.Add(p2);

        txtUsuario.Text = string.Join(" ", partes);
    }

    private static Panel CrearSeccion(string titulo, int x, int y, int w, int h)
    {
        var card = new Panel
        {
            BackColor = Color.White,
            Location = new Point(x, y),
            Size = new Size(w, h)
        };
        card.Paint += (s, e) =>
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(219, 227, 241)), 0, 0, card.Width - 1, card.Height - 1);
        };
        UiAssets.RedondearControl(card, 10);
        var encabezado = new Panel
        {
            Dock = DockStyle.Top,
            Height = 46,
            BackColor = Color.FromArgb(237, 244, 255)
        };
        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(16, 12),
            AutoSize = true
        };
        encabezado.Controls.Add(lblTitulo);
        card.Controls.Add(encabezado);
        return card;
    }

    private TextBox CrearFila(Control parent, string etiqueta, int y)
    {
        var label = new Label
        {
            Text = $"{etiqueta}:",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(45, 55, 70),
            Location = new Point(24, y + 2),
            Size = new Size(240, 30),
            TextAlign = ContentAlignment.MiddleLeft
        };
        var txt = new TextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(35, 45, 65),
            Location = new Point(270, y + 2),
            Size = new Size(parent.Width - 290, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            TabStop = true
        };
        parent.Controls.Add(label);
        parent.Controls.Add(txt);
        return txt;
    }

    private void ActivarEdicion()
    {
        if (modoEdicion)
            return;

        respaldoPerfil = PerfilUsuarioStore.Obtener();
        modoEdicion = true;
        SetReadOnlyCampos(false);
        // txtControl siempre de solo lectura — el ID se asigna automáticamente
        txtControl.ReadOnly = true;
        txtControl.BorderStyle = BorderStyle.None;
        txtControl.BackColor = Color.FromArgb(240, 244, 248);
        txtControl.ForeColor = Color.FromArgb(130, 140, 160);
        txtUsuario.ReadOnly = true;
        txtUsuario.BorderStyle = BorderStyle.None;
        txtUsuario.BackColor = Color.White;
        btnEditar.Visible = false;
        btnGuardar.Visible = true;
        btnCancelar.Visible = true;
        btnSubirFoto.Visible = true;
    }

    private void SeleccionarFotoPerfil()
    {
        using var ofd = new OpenFileDialog();
        ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
        ofd.Title = "Seleccionar foto de perfil";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            rutaFotoTemporal = ofd.FileName;
            try
            {
                using var stream = new FileStream(rutaFotoTemporal, FileMode.Open, FileAccess.Read);
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                if (pbFotoPerfil.Image != null)
                {
                    var oldImg = pbFotoPerfil.Image;
                    pbFotoPerfil.Image = Image.FromStream(ms);
                    oldImg.Dispose();
                }
                else
                {
                    pbFotoPerfil.Image = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen:\n" + ex.Message, "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void GuardarEdicion()
    {
        if (string.IsNullOrWhiteSpace(txtNumeroControl.Text))
        {
            MessageBox.Show("El Número de Control es obligatorio.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!long.TryParse(txtNumeroControl.Text.Trim(), out long numCtrl))
        {
            MessageBox.Show("El Número de Control debe ser un número válido.", "GALAB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string email = txtCorreo.Text.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("El correo electrónico no tiene un formato válido (ejemplo: usuario@dominio.com).", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string tel = txtTelefono.Text.Trim();
        if (tel.Length != 10)
        {
            MessageBox.Show("El número de teléfono debe tener exactamente 10 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int? asiento = null;
        if (!string.IsNullOrWhiteSpace(txtNumeroAsiento.Text))
        {
            if (int.TryParse(txtNumeroAsiento.Text.Trim(), out int val))
                asiento = val;
        }

        var perfil = PerfilUsuarioStore.Obtener();
        perfil.Nombre = txtNombre.Text.Trim();
        perfil.PrimerApellido = txtPrimerApellido.Text.Trim();
        perfil.SegundoApellido = txtSegundoApellido.Text.Trim();
        perfil.Telefono = txtTelefono.Text.Trim();
        perfil.Correo = email;
        perfil.NumeroAsiento = asiento;
        perfil.NumeroControl = numCtrl;
        perfil.ControlNumber = txtControl.Text.Trim();

        perfil.Semestre = txtSemestre.Text.Trim();
        perfil.Grupo = txtGrupo.Text.Trim();
        
        string username = txtUsuario.Text.Trim();
        perfil.Usuario = username;

        try
        {
            PerfilUsuarioStore.Guardar(perfil);

            if (!string.IsNullOrWhiteSpace(rutaFotoTemporal))
            {
                UiAssets.GuardarFotoPerfil(perfil.ControlNumber, rutaFotoTemporal);
            }

            FinalizarEdicion();
            CargarPerfilEnVista();
        }
        catch (Exception ex)
        {
            NotificacionForm.MostrarExcepcion(this, ex);
        }
    }

    private void CancelarEdicion()
    {
        if (respaldoPerfil != null)
            PerfilUsuarioStore.Guardar(respaldoPerfil);
        CargarPerfilEnVista();
        FinalizarEdicion();
    }

    private void FinalizarEdicion()
    {
        modoEdicion = false;
        SetReadOnlyCampos(true);
        btnEditar.Visible = true;
        btnGuardar.Visible = false;
        btnCancelar.Visible = false;
        btnSubirFoto.Visible = false;
        rutaFotoTemporal = null;
    }

    private void SetReadOnlyCampos(bool readOnly)
    {
        foreach (var txt in ObtenerCamposEditables())
        {
            txt.ReadOnly = readOnly;
            txt.BorderStyle = readOnly ? BorderStyle.None : BorderStyle.FixedSingle;
            txt.BackColor = readOnly ? Color.White : Color.FromArgb(250, 252, 255);
            txt.TabStop = !readOnly;
        }
    }

    private IEnumerable<TextBox> ObtenerCamposEditables()
    {
        // Solo ciertos campos son editables por el propio alumno
        return new[]
        {
            txtNombre, txtPrimerApellido, txtSegundoApellido, txtTelefono, txtCorreo,
            txtNumeroAsiento, txtNumeroControl, txtSemestre, txtGrupo
        };
    }

    private void CargarPerfilEnVista()
    {
        var perfil = PerfilUsuarioStore.Obtener();
        txtNombre.Text = perfil.Nombre;
        txtPrimerApellido.Text = perfil.PrimerApellido;
        txtSegundoApellido.Text = perfil.SegundoApellido;
        txtTelefono.Text = perfil.Telefono;
        txtCorreo.Text = perfil.Correo;
        txtNumeroAsiento.Text = perfil.NumeroAsiento?.ToString() ?? "";
        
        txtControl.Text = perfil.ControlNumber;
        txtNumeroControl.Text = perfil.NumeroControl.ToString();
        txtSemestre.Text = perfil.Semestre;
        txtGrupo.Text = perfil.Grupo;
        txtUsuario.Text = perfil.Usuario;
        txtEstatus.Text = perfil.Rol;
        txtEstadoCuenta.Text = perfil.Activo ? "Activo" : "Inactivo";
        txtFechaRegistro.Text = perfil.FechaRegistro.ToString("g");

        var img = UiAssets.CargarFotoPerfil(perfil.ControlNumber);
        if (img != null)
        {
            if (pbFotoPerfil.Image != null)
            {
                var oldImg = pbFotoPerfil.Image;
                pbFotoPerfil.Image = img;
                oldImg.Dispose();
            }
            else
            {
                pbFotoPerfil.Image = img;
            }
        }
        else
        {
            var initialsImg = UiAssets.CrearAvatarConIniciales(perfil.NombreCompleto, 100, 100);
            if (pbFotoPerfil.Image != null)
            {
                var oldImg = pbFotoPerfil.Image;
                pbFotoPerfil.Image = initialsImg;
                oldImg.Dispose();
            }
            else
            {
                pbFotoPerfil.Image = initialsImg;
            }
        }

        SetReadOnlyCampos(true);
    }

    private void SoloNumeros_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void SoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
        {
            e.Handled = true;
        }
    }
}
