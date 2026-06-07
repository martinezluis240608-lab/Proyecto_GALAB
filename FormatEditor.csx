using System;
using System.IO;
using System.Text.RegularExpressions;

string file = @"C:\Users\admin\source\repos\martinezluis240608-lab\Proyecto_GALAB\Views\Admin\AdminUsuarioEditorForm.cs";
string content = File.ReadAllText(file);

content = content.Replace("Titulo(\"ID (Asignado por el sistema)\", 24, 20)", "Titulo(\"ID (Asignado por el sistema)\", 24, 56)");
content = content.Replace("out txtId, 24, 44, 320", "out txtId, 24, 80, 320");

content = content.Replace("Titulo(\"Nombre(s) *\", 24, 94)", "Titulo(\"Nombre(s) *\", 24, 130)");
content = content.Replace("out txtNombre, 24, 118, 320", "out txtNombre, 24, 154, 320");

content = content.Replace("Titulo(\"Primer apellido *\", 24, 168)", "Titulo(\"Primer apellido *\", 24, 204)");
content = content.Replace("out txtPrimerApellido, 24, 192, 320", "out txtPrimerApellido, 24, 228, 320");

content = content.Replace("Titulo(\"Segundo apellido\", 24, 242)", "Titulo(\"Segundo apellido\", 24, 278)");
content = content.Replace("out txtSegundoApellido, 24, 266, 320", "out txtSegundoApellido, 24, 302, 320");

content = content.Replace("Titulo(\"Correo electrónico *\", 24, 316)", "Titulo(\"Correo electrónico *\", 24, 352)");
content = content.Replace("out txtCorreo, 24, 340, 320", "out txtCorreo, 24, 376, 320");

content = content.Replace("Titulo(\"Teléfono *\", 24, 390)", "Titulo(\"Teléfono *\", 24, 426)");
content = content.Replace("out txtTelefono, 24, 414, 320", "out txtTelefono, 24, 450, 320");

content = content.Replace("Titulo(\"Rol *\", 376, 20)", "Titulo(\"Rol *\", 376, 56)");
content = content.Replace("out cmbRol, 376, 44, 320", "out cmbRol, 376, 80, 320");

content = content.Replace("Titulo(\"Estado *\", 376, 94)", "Titulo(\"Estado *\", 376, 130)");
content = content.Replace("out cmbEstado, 376, 118, 320", "out cmbEstado, 376, 154, 320");

content = content.Replace("Titulo(\"Nombre de usuario *\", 376, 168)", "Titulo(\"Nombre de usuario *\", 376, 204)");
content = content.Replace("out txtUsuario, 376, 192, 320", "out txtUsuario, 376, 228, 320");

content = Regex.Replace(content, @"Titulo\(""Contraseña"".*?, 376, 242\)", m => m.Value.Replace("242", "278"));
content = content.Replace("out txtContrasena, 376, 266, 320", "out txtContrasena, 376, 302, 320");

content = Regex.Replace(content, @"Titulo\(""Confirmar contraseña"".*?, 376, 316\)", m => m.Value.Replace("316", "352"));
content = content.Replace("out txtConfirmarContrasena, 376, 340, 320", "out txtConfirmarContrasena, 376, 376, 320");

content = content.Replace("Location = new Point(376, 390)", "Location = new Point(376, 506)");

string headers = @"
        var lblHeaderPersonal = new Label
        {
            Text = ""👤  Información Personal"",
            Font = new Font(""Segoe UI"", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(24, 16),
            AutoSize = true
        };

        var lblHeaderAcceso = new Label
        {
            Text = ""🔒  Información de Acceso"",
            Font = new Font(""Segoe UI"", 11F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(376, 16),
            AutoSize = true
        };
";
content = content.Replace("// Column 1 (Left - Personal Info)", headers + "\n        // Column 1 (Left - Personal Info)");

string recomendacion = @"
        var pnlRecomendacion = new Panel
        {
            Location = new Point(376, 426),
            Size = new Size(320, 70),
            BackColor = Color.FromArgb(240, 248, 255)
        };
        pnlRecomendacion.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(180, 210, 245), 1);
            using var path = UiAssets.CrearRectanguloRedondo(new Rectangle(0, 0, pnlRecomendacion.Width - 1, pnlRecomendacion.Height - 1), 6);
            e.Graphics.DrawPath(pen, path);
        };
        UiAssets.RedondearControl(pnlRecomendacion, 6);

        var lblRecIcon = new Label
        {
            Text = ""ℹ"",
            Font = new Font(""Segoe UI Symbol"", 12F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(10, 10),
            AutoSize = true
        };
        var lblRecTitulo = new Label
        {
            Text = ""Recomendación"",
            Font = new Font(""Segoe UI"", 9.5F, FontStyle.Bold),
            ForeColor = UiAssets.AzulPrincipal,
            Location = new Point(34, 12),
            AutoSize = true
        };
        var lblRecTexto = new Label
        {
            Text = ""Usa una contraseña segura con al menos 8 caracteres, combinando letras, números y símbolos."",
            Font = new Font(""Segoe UI"", 8.5F),
            ForeColor = UiAssets.AzulOscuro,
            Location = new Point(10, 34),
            Size = new Size(300, 30)
        };
        pnlRecomendacion.Controls.AddRange(new Control[] { lblRecIcon, lblRecTitulo, lblRecTexto });

";
content = content.Replace("// Panel de Alumnos (Dinámico)", recomendacion + "        // Panel de Alumnos (Dinámico)");

content = content.Replace("pnlAlumnos\r\n        });", "pnlAlumnos,\r\n            lblHeaderPersonal, lblHeaderAcceso, pnlRecomendacion\r\n        });");
content = content.Replace("pnlAlumnos\n        });", "pnlAlumnos,\n            lblHeaderPersonal, lblHeaderAcceso, pnlRecomendacion\n        });");

content = content.Replace("ClientSize = new Size(720, 700);", "ClientSize = new Size(720, 810);");

File.WriteAllText(file, content, System.Text.Encoding.UTF8);
Console.WriteLine("Done");
