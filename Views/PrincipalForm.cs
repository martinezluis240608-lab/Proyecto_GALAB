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
        UiAssets.PrepararPantallaCompleta(this);
        _presenter = new PrincipalPresenter(this);
    }

    public void NavegarAGestion()   => UiAssets.AbrirCerrandoActual(this, new GestionIncidenciasForm());
    public void NavegarAPerfil()    => UiAssets.AbrirCerrandoActual(this, new PerfilForm());
    public void MostrarAyuda()
    {
        UiAssets.AbrirCerrandoActual(this, new AyudaForm());
    }

    public void MostrarContacto()
    {
        UiAssets.AbrirCerrandoActual(this, new ContactoForm());
    }

    private void btnPerfil_Click(object s, EventArgs e)    => OnPerfil?.Invoke(this, e);
    private void btnGestion_Click(object s, EventArgs e)   => OnGestionIncidencias?.Invoke(this, e);
    private void btnAyuda_Click(object s, EventArgs e)     => OnAyuda?.Invoke(this, e);
    private void btnContacto_Click(object s, EventArgs e)  => OnContacto?.Invoke(this, e);
}
