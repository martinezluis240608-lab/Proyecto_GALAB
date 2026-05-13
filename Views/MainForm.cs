using Proyecto_GALAB.Interfaces;
using Proyecto_GALAB.Presenters;

namespace Proyecto_GALAB.Views
{
    // La Vista implementa IMainView
    // Solo maneja UI, no contiene lógica de negocio
    public partial class MainForm : Form, IMainView
    {
        private readonly MainPresenter _presenter;

        // --- Implementación de IMainView ---
        public string NombreUsuario => txtNombre.Text;

        public event EventHandler? OnSaludar;

        public void MostrarMensaje(string mensaje)
        {
            lblResultado.Text = mensaje;
        }

        // --- Constructor ---
        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this);
        }

        // --- Eventos de controles ---
        private void btnSaludar_Click(object sender, EventArgs e)
        {
            OnSaludar?.Invoke(this, EventArgs.Empty);
        }
    }
}
