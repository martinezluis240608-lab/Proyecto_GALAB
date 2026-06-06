using System;
using System.Linq;

namespace Proyecto_GALAB.Models;

/// <summary>
/// Perfil del administrador, mapeado uno a uno con la tabla administradores de la base de datos.
/// </summary>
public class PerfilAdministrador
{
    public string IdAdministrador { get; set; } = string.Empty;
    public string Nombre { get; set; } = "Administrador";
    public string PrimerApellido { get; set; } = string.Empty;
    public string SegundoApellido { get; set; } = string.Empty;
    public string Correo { get; set; } = "admin@itsmg.edu.mx";
    public string Telefono { get; set; } = string.Empty;
    public string Usuario { get; set; } = "admin";
    public string Contrasena { get; set; } = string.Empty;
    public string Rol { get; set; } = "Administrador";
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

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
