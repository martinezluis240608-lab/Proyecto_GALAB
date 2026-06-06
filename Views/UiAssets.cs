using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Proyecto_GALAB.Services;
using Proyecto_GALAB.Views.Admin;

namespace Proyecto_GALAB.Views;

internal static class UiAssets
{
    public static readonly Color AzulPrincipal = Color.FromArgb(0, 82, 170);
    public static readonly Color AzulOscuro = Color.FromArgb(8, 32, 82);
    public static readonly Color AzulClaro = Color.FromArgb(232, 243, 255);
    public static readonly Color Fondo = Color.FromArgb(248, 251, 255);
    public static readonly Color Borde = Color.FromArgb(210, 222, 238);

    public const string NombreUsuarioTemporal = "Nombre del usuario";
    public const string RolUsuarioTemporal = "Rol del usuario";

    public static Image? CargarLogoInstitucion()
    {
        return CargarImagen("logo-institucion.jpg") ?? CargarImagenDesdeViews("descargar.jpg");
    }

    public static Image? CargarLogoGalab()
    {
        return CargarImagen("logo-galab.png");
    }

    public static Image? CargarImagen(string nombreArchivo)
    {
        string[] rutas =
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", nombreArchivo),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Assets", "Images", nombreArchivo)
        };

        return CargarPrimeraExistente(rutas);
    }

    public static void NavegarAGestionIncidencias(Form actual)
    {
        if (SesionActual.EsAdministrador)
            AbrirCerrandoActual(actual, new AdminGestionIncidenciasForm());
        else
            AbrirCerrandoActual(actual, new GestionIncidenciasForm());
    }

    public static void AbrirCerrandoActual(Form actual, Form destino)
    {
        PrepararPantallaCompleta(destino);
        destino.StartPosition = FormStartPosition.CenterScreen;
        destino.Show();

        var ventanasACerrar = new List<Form> { actual };
        if (actual.Owner != null)
            ventanasACerrar.Add(actual.Owner);

        foreach (Form form in ventanasACerrar.Distinct().ToList())
        {
            if (!form.IsDisposed && form != destino)
                form.Close();
        }
    }

    public static void CerrarSesion(Form actual)
    {
        Services.SesionActual.Cerrar();
        var login = Application.OpenForms.OfType<LoginForm>().FirstOrDefault();
        if (login == null || login.IsDisposed)
        {
            login = new LoginForm();
        }
        PrepararPantallaCompleta(login);
        login.Show();
        login.BringToFront();

        login.BeginInvoke(new Action(() =>
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != login && !form.IsDisposed)
                {
                    try { form.Close(); } catch { }
                }
            }
        }));
    }

    public static void PrepararPantallaCompleta(Form form)
    {
        form.StartPosition = FormStartPosition.CenterScreen;
        form.WindowState = FormWindowState.Maximized;
        form.MinimumSize = new Size(1050, 680);
    }

    public static void RedondearControl(Control control, int radio)
    {
        control.SizeChanged += (s, e) =>
        {
            if (control.Width > 0 && control.Height > 0)
            {
                using var path = CrearRectanguloRedondo(new Rectangle(0, 0, control.Width, control.Height), radio);
                control.Region = new Region(path);
            }
        };
        if (control.Width > 0 && control.Height > 0)
        {
            using var path = CrearRectanguloRedondo(new Rectangle(0, 0, control.Width, control.Height), radio);
            control.Region = new Region(path);
        }
    }

    public static Button CrearBotonSidebar(string icono, string texto, int y, bool activo, Action? accion)
    {
        string textoLimpio = texto.Replace("   ›", "").Replace(" ›", "").Trim();

        var boton = new Button
        {
            Text = $"{icono}  {textoLimpio}",
            Font = new Font("Segoe UI", 9.5F, activo ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = Color.Black,
            BackColor = activo ? Color.FromArgb(238, 246, 255) : Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = accion == null ? Cursors.Default : Cursors.Hand,
            Location = new Point(12, y),
            Size = new Size(266, 46),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };

        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 246, 255);
        if (accion != null)
            boton.Click += (s, e) => accion();

        RedondearControl(boton, 8);

        return boton;
    }

    public static GraphicsPath CrearRectanguloRedondo(Rectangle rect, int radio)
    {
        int diametro = radio * 2;
        var path = new GraphicsPath();
        path.AddArc(rect.Left, rect.Top, diametro, diametro, 180, 90);
        path.AddArc(rect.Right - diametro, rect.Top, diametro, diametro, 270, 90);
        path.AddArc(rect.Right - diametro, rect.Bottom - diametro, diametro, diametro, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - diametro, diametro, diametro, 90, 90);
        path.CloseFigure();
        return path;
    }

    private static Image? CargarImagenDesdeViews(string nombreArchivo)
    {
        string[] rutas =
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", nombreArchivo),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Views", nombreArchivo)
        };

        return CargarPrimeraExistente(rutas);
    }

    private static Image? CargarPrimeraExistente(IEnumerable<string> rutas)
    {
        foreach (string ruta in rutas)
        {
            string rutaCompleta = Path.GetFullPath(ruta);
            if (File.Exists(rutaCompleta))
                return Image.FromFile(rutaCompleta);
        }

        return null;
    }

    public static Image? CargarFotoPerfil(string control)
    {
        if (string.IsNullOrWhiteSpace(control))
            return null;

        string nombreArchivo = $"perfil_{control}.png";
        string[] rutas =
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagenes", nombreArchivo),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "imagenes", nombreArchivo)
        };

        return CargarPrimeraExistenteSinBloqueo(rutas);
    }

    public static Image CrearAvatarConIniciales(string nombreCompleto, int ancho, int alto)
    {
        var bmp = new Bitmap(ancho, alto);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            string iniciales = "U";
            if (!string.IsNullOrWhiteSpace(nombreCompleto))
            {
                var partes = nombreCompleto.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length == 1 && partes[0].Length > 0)
                {
                    iniciales = partes[0].Substring(0, 1).ToUpper();
                }
                else if (partes.Length >= 2)
                {
                    string p1 = partes[0].Length > 0 ? partes[0].Substring(0, 1) : "";
                    string p2 = partes[partes.Length - 1].Length > 0 ? partes[partes.Length - 1].Substring(0, 1) : "";
                    iniciales = (p1 + p2).ToUpper();
                    if (string.IsNullOrEmpty(iniciales)) iniciales = "U";
                }
            }

            int hash = Math.Abs(nombreCompleto.GetHashCode());
            Color[] colores = {
                Color.FromArgb(0, 82, 170),   // AzulPrincipal
                Color.FromArgb(8, 32, 82),    // AzulOscuro
                Color.FromArgb(34, 166, 88),   // Green
                Color.FromArgb(227, 115, 0),   // Orange
                Color.FromArgb(161, 25, 209),  // Purple
                Color.FromArgb(217, 48, 37)    // Red
            };
            Color colorFondo = colores[hash % colores.Length];

            using (var brush = new SolidBrush(colorFondo))
            {
                g.FillEllipse(brush, 0, 0, ancho - 1, alto - 1);
            }

            float fontSize = (float)(ancho * 0.35);
            using (var font = new Font("Segoe UI", fontSize, FontStyle.Bold))
            using (var brushText = new SolidBrush(Color.White))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(iniciales, font, brushText, new RectangleF(0, 0, ancho, alto), sf);
            }
        }
        return bmp;
    }

    public static void GuardarFotoPerfil(string control, string rutaTemporal)
    {
        if (string.IsNullOrWhiteSpace(control) || string.IsNullOrWhiteSpace(rutaTemporal) || !File.Exists(rutaTemporal))
            return;

        string nombreArchivo = $"perfil_{control}.png";
        
        string dirBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagenes");
        if (!Directory.Exists(dirBin))
        {
            try { Directory.CreateDirectory(dirBin); } catch { }
        }

        string rutaDestinoBin = Path.Combine(dirBin, nombreArchivo);

        try
        {
            File.Copy(rutaTemporal, rutaDestinoBin, true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error copying profile photo to bin: " + ex.Message);
        }

        string dirSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "imagenes");
        if (Directory.Exists(dirSource))
        {
            string rutaDestinoSrc = Path.Combine(dirSource, nombreArchivo);
            try
            {
                File.Copy(rutaTemporal, rutaDestinoSrc, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error copying profile photo to source: " + ex.Message);
            }
        }
    }

    private static Image? CargarPrimeraExistenteSinBloqueo(IEnumerable<string> rutas)
    {
        foreach (string ruta in rutas)
        {
            string rutaCompleta = Path.GetFullPath(ruta);
            if (File.Exists(rutaCompleta))
            {
                try
                {
                    using var stream = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read);
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    return Image.FromStream(ms);
                }
                catch
                {
                    // Ignorar y probar la siguiente ruta
                }
            }
        }

        return null;
    }
}
