using System;
using System.Linq;

namespace Proyecto_GALAB.Models;

public class PerfilUsuario
{
    public string ControlNumber { get; set; } = string.Empty; // id_alumno
    public long NumeroControl { get; set; } // numero_control
    public string Nombre { get; set; } = string.Empty;
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; } = string.Empty;
    public string Semestre { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int? NumeroAsiento { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public string Rol { get; set; } = "Estudiante"; // rol
    public string Contrasena { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public string Usuario { get; set; } = string.Empty;

    public string NombreCompleto
    {
        get => $"{Nombre} {PrimerApellido} {SegundoApellido}".Trim();
        set
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var partes = value.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length == 1)
            {
                Nombre = partes[0];
                PrimerApellido = string.Empty;
                SegundoApellido = string.Empty;
            }
            else if (partes.Length == 2)
            {
                Nombre = partes[0];
                PrimerApellido = partes[1];
                SegundoApellido = string.Empty;
            }
            else if (partes.Length >= 3)
            {
                SegundoApellido = partes[partes.Length - 1];
                PrimerApellido = partes[partes.Length - 2];
                Nombre = string.Join(" ", partes.Take(partes.Length - 2));
            }
        }
    }
}
