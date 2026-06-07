using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Proyecto_GALAB.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Proyecto_GALAB.Presenters
{
    public class AyudaPresenter
    {
        private readonly IAyudaView _view;

        public AyudaPresenter(IAyudaView view)
        {
            _view = view;
            _view.CargarManualPdf += OnCargarManualPdf;
            _view.AbrirGuiaRapida += OnAbrirGuiaRapida;
            _view.AbrirFaq += OnAbrirFaq;
            _view.AbrirPlantillas += OnAbrirPlantillas;
            _view.ContactarSoporte += OnContactarSoporte;
        }

        private void OnCargarManualPdf(object? sender, EventArgs e)
        {
            string ruta = Path.Combine(Application.StartupPath, "Recursos", "manual_usuario.pdf");
            if (File.Exists(ruta))
                _view.AbrirPdf(ruta);
            else
                Proyecto_GALAB.Views.CustomMessageBox.Show("El manual no está disponible.", "Ayuda");
        }

        private void OnAbrirGuiaRapida(object? sender, EventArgs e)
        {
            _view.MostrarContenido("Guía rápida",
                "<html><body><h2>Guía rápida</h2><ol><li>Inicia sesión</li><li>Ve a Gestión de incidencias</li><li>Haz clic en 'Nueva incidencia'</li><li>Completa el formulario</li><li>Guarda</li></ol></body></html>");
        }

        private void OnAbrirFaq(object? sender, EventArgs e)
        {
            _view.MostrarContenido("Preguntas frecuentes",
                "<html><body><h2>FAQ</h2><b>¿Cómo registro una incidencia?</b><br>Desde Gestión → Nueva incidencia.<br><br><b>¿Puedo editar una incidencia?</b><br>Solo si está en borrador.<br><br><b>¿Cómo contacto con soporte?</b><br>Usa el botón 'Contáctanos' de esta ventana.</body></html>");
        }

        private void OnAbrirPlantillas(object? sender, EventArgs e)
        {
            string carpeta = Path.Combine(Application.StartupPath, "Recursos", "Plantillas");
            if (Directory.Exists(carpeta))
                Process.Start("explorer.exe", carpeta);
            else
                Proyecto_GALAB.Views.CustomMessageBox.Show("Carpeta de plantillas no encontrada.", "Ayuda");
        }

        private void OnContactarSoporte(object? sender, EventArgs e)
        {
            // Aquí puedes mostrar tu formulario de contacto
            Proyecto_GALAB.Views.CustomMessageBox.Show("Abrir formulario de contacto (próximamente)", "Contacto");
        }
    }
}
