using Proyecto_GALAB.Views;

namespace Proyecto_GALAB;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}
