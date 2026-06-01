using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views;

public partial class LoginForm : Form, ILoginView
{
    private readonly LoginPresenter _presenter;

    public string Usuario    => txtUsuario.Text;
    public string Contrasena => txtContrasena.Text;
    public event EventHandler? OnIniciarSesion;

    public void MostrarError(string mensaje) =>
        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public void NavegarARegistro()
    {
        var principal = new PrincipalForm();
        UiAssets.PrepararPantallaCompleta(principal);
        principal.Show();
        this.Hide();
    }

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
