namespace Proyecto_GALAB.Models
{
    // El Model representa los datos y la lógica de negocio
    public class Usuario
    {
        public string Nombre { get; set; } = string.Empty;

        public string ObtenerSaludo()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                return "Por favor ingresa tu nombre.";

            return $"¡Hola, {Nombre}! Bienvenido al proyecto MVP.";
        }
    }
}
