namespace Proyecto_GALAB.Interfaces;

public interface IPrincipalView
{
    void NavegarAGestion();
    void NavegarAPerfil();
    void MostrarAyuda();
    void MostrarContacto();
    event EventHandler OnPerfil;
    event EventHandler OnGestionIncidencias;
    event EventHandler OnAyuda;
    event EventHandler OnContacto;
}
