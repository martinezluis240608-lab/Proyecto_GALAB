using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views;

public partial class PrincipalForm : Form, IPrincipalView
{
    private readonly PrincipalPresenter _presenter;

    public event EventHandler? OnPerfil;
    public event EventHandler? OnGestionIncidencias;
    public event EventHandler? OnAyuda;
    public event EventHandler? OnContacto;

    public PrincipalForm()
    {
        InitializeComponent();
        _presenter = new PrincipalPresenter(this);
    }

    public void NavegarAGestion()   => new IncidenciaForm().Show();
    public void NavegarAPerfil()    => MessageBox.Show("Perfil (próximamente)", "GALAB");
    public void MostrarAyuda()      => MessageBox.Show("Ayuda (próximamente)", "GALAB");
    public void MostrarContacto()   => MessageBox.Show("Contacto: galab@tecnico.edu", "GALAB");

    private void btnPerfil_Click(object s, EventArgs e)    => OnPerfil?.Invoke(this, e);
    private void btnGestion_Click(object s, EventArgs e)   => OnGestionIncidencias?.Invoke(this, e);
    private void btnAyuda_Click(object s, EventArgs e)     => OnAyuda?.Invoke(this, e);
    private void btnContacto_Click(object s, EventArgs e)  => OnContacto?.Invoke(this, e);
}
