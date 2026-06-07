using System.Drawing.Drawing2D;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Services;

namespace Proyecto_GALAB.Views.Admin;

public class AdminInicioForm : Form
{
    public AdminInicioForm()
    {
        Text = "GALAB - Inicio";
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10F);
        UiAssets.PrepararPantallaCompleta(this);
        CrearInterfaz();
    }

    private void CrearInterfaz()
    {
        Controls.Clear();

        var panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        var header = AdminSidebar.CrearHeader();
        var contenido = CrearContenido();
        var footer = new Label
        {
            Text = "© 2024 GALAB - Panel administrador",
            Dock = DockStyle.Bottom,
            Height = 34,
            BackColor = UiAssets.AzulClaro,
            ForeColor = UiAssets.AzulOscuro,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F)
        };

        panelDerecho.Controls.Add(contenido); // Dock.Fill
        panelDerecho.Controls.Add(header);    // Dock.Top

        Controls.Add(panelDerecho);
        Controls.Add(footer);  // Dock.Bottom
    }

    private Panel CrearContenido()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };

        var lblBienvenido = new Label
        {
            Text = "BIENVENIDO",
            Font = new Font("Segoe UI", 36F, FontStyle.Bold),
            ForeColor = Color.FromArgb(20, 20, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var lblDescripcion = new Label
        {
            Text = "GALAB es un sistema web para registrar y dar seguimiento a incidencias en\nlaboratorios de cómputo de manera rápida y organizada.",
            Font = new Font("Segoe UI", 14F),
            ForeColor = Color.FromArgb(35, 45, 65),
            BackColor = Color.White,
            TextAlign = ContentAlignment.TopCenter,
            AutoSize = false,
            UseMnemonic = false,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // Sección de accesos directos estilo alumno
        var btnGestion = MkMenuBtn("☰  GESTIÓN DE\n    INCIDENCIAS", () => UiAssets.AbrirCerrandoActual(this, new AdminGestionIncidenciasForm()));
        var btnUsuarios = MkMenuBtn("👥  GESTIÓN DE\n    USUARIOS", () => UiAssets.AbrirCerrandoActual(this, new AdminGestionUsuariosForm()));
        var btnPerfil = MkMenuBtn("👤  MI PERFIL", () => UiAssets.AbrirCerrandoActual(this, new AdminPerfilForm()));

        var btnCerrarSesion = new Button
        {
            Text = "↪  CERRAR SESIÓN",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 82, 170),
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrarSesion.FlatAppearance.BorderColor = Color.FromArgb(0, 82, 170);
        btnCerrarSesion.FlatAppearance.BorderSize = 2;
        UiAssets.RedondearControl(btnCerrarSesion, 10);
        btnCerrarSesion.Click += (_, _) => UiAssets.CerrarSesion(this);

        panel.Controls.AddRange(new Control[] { 
            lblBienvenido, 
            lblDescripcion,
            btnGestion, btnUsuarios, btnPerfil, btnCerrarSesion
        });

        panel.Resize += (s, e) =>
        {
            if (WindowState == FormWindowState.Minimized) return;
            int cw = panel.ClientSize.Width;
            int ch = panel.ClientSize.Height;
            
            // Layout de botones rápidos (estilo alumno)
            int bw = Math.Min(370, Math.Max(250, cw / 4));
            int bh = Math.Min(160, Math.Max(120, ch / 6));
            int btnGap = Math.Min(38, Math.Max(22, cw / 50));

            float fontSize = bw > 300 ? 15F : 13F;
            var btnFont = new Font("Segoe UI", fontSize, FontStyle.Bold);
            btnGestion.Font = btnFont;
            btnUsuarios.Font = btnFont;
            btnPerfil.Font = btnFont;

            int totalBtnW = bw * 3 + btnGap * 2;
            if (totalBtnW > cw - 60)
            {
                bw = Math.Max(190, (cw - 60 - btnGap * 2) / 3);
                bh = Math.Max(90, bh);
                totalBtnW = bw * 3 + btnGap * 2;
            }

            int btnStartX = (cw - totalBtnW) / 2;
            int gridH = bh;
            int row1Y = Math.Max(270, 245 + (ch - 245 - gridH) / 2);

            lblBienvenido.Left = 0;
            lblBienvenido.Top = 108;
            lblBienvenido.Width = cw - 2;
            lblBienvenido.Height = 80;

            lblDescripcion.Left = 20;
            lblDescripcion.Width = cw - 40;
            int descripcionHeight = Math.Max(66, TextRenderer.MeasureText(
                lblDescripcion.Text,
                lblDescripcion.Font,
                new Size(Math.Max(200, cw - 40), int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height + 4);
            lblDescripcion.Height = descripcionHeight;
            int espacioSuperior = lblBienvenido.Bottom + 12;
            int espacioInferior = row1Y - 12;
            lblDescripcion.Top = Math.Max(espacioSuperior, espacioSuperior + Math.Max(0, (espacioInferior - espacioSuperior - descripcionHeight) / 2));

            btnPerfil.SetBounds(btnStartX, row1Y, bw, bh);
            btnGestion.SetBounds(btnStartX + bw + btnGap, row1Y, bw, bh);
            btnUsuarios.SetBounds(btnStartX + (bw + btnGap) * 2, row1Y, bw, bh);

            int btnLogoutW = Math.Max(220, bw - 40);
            int btnLogoutH = 50;
            btnCerrarSesion.SetBounds((cw - btnLogoutW) / 2, row1Y + bh + 60, btnLogoutW, btnLogoutH);
        };

        return panel;
    }

    private static Button MkMenuBtn(string text, Action onClick)
    {
        var btn = new Button
        {
            Text      = text,
            BackColor = Color.FromArgb(210, 235, 250),
            ForeColor = Color.FromArgb(30, 30, 30),
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 12F, FontStyle.Bold),
            Cursor    = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        btn.FlatAppearance.BorderSize = 0;
        UiAssets.RedondearControl(btn, 10);
        btn.Click += (_, _) => onClick();
        return btn;
    }
}
