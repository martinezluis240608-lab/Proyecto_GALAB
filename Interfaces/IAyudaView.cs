using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Proyecto_GALAB.Interfaces
{
    public interface IAyudaView
    {
        event EventHandler? CargarManualPdf;
        event EventHandler? AbrirGuiaRapida;
        event EventHandler? AbrirFaq;
        event EventHandler? AbrirVideos;
        event EventHandler? AbrirPlantillas;
        event EventHandler? ContactarSoporte;

        void MostrarContenido(string titulo, string contenidoHtml);
        void AbrirPdf(string ruta);
        void Cerrar();
    }
}