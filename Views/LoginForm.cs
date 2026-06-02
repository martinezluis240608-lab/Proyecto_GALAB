using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Models;
using Proyecto_GALAB.Presenters;
using Proyecto_GALAB.Views.Admin;

namespace Proyecto_GALAB.Views;

public partial class LoginForm : Form, ILoginView
{
    private readonly LoginPresenter _presenter;

    public string Usuario        => txtUsuario.Text;
    public string Contrasena     => txtContrasena.Text;
    public RolUsuario RolSeleccionado { get; private set; } = RolUsuario.Estudiante;

    public event EventHandler? OnIniciarSesion;

    public void MostrarError(string mensaje) =>
        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public void NavegarComoEstudiante()
    {
        var principal = new PrincipalForm();
        UiAssets.PrepararPantallaCompleta(principal);
        principal.Show();
        Hide();
    }

    public void NavegarComoAdministrador()
    {
        var admin = new AdminGestionIncidenciasForm();
        UiAssets.PrepararPantallaCompleta(admin);
        admin.Show();
        Hide();
    }

    internal void EstablecerRol(RolUsuario rol) => RolSeleccionado = rol;

    public LoginForm()
    {
        InitializeComponent();
        UiAssets.PrepararPantallaCompleta(this);
        _presenter = new LoginPresenter(this);
    }

    private void btnIniciarSesion_Click(object sender, EventArgs e) =>
        OnIniciarSesion?.Invoke(this, EventArgs.Empty);

    private void lblForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) =>
        MessageBox.Show("Contacta al administrador para recuperar tu contraseña.",
                        "Recuperar contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);
}
