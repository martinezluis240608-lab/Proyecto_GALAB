using Proyecto_GALAB.Views;

namespace Proyecto_GALAB;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        ApplicationConfiguration.Initialize();
        
        // Inicializar/Verificar esquema de la base de datos
        Proyecto_GALAB.Services.DatabaseService.InicializarBaseDeDatos();
        
        Application.Run(new LoginForm());
    }
}
