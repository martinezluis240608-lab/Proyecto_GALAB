using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

string dir = @"C:\Users\admin\source\repos\martinezluis240608-lab\Proyecto_GALAB";
var files = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);

foreach (var f in files)
{
    if (f.EndsWith("CustomMessageBox.cs")) continue;
    
    string text = File.ReadAllText(f, Encoding.UTF8);
    bool modified = false;

    if (text.Contains("MessageBox.Show"))
    {
        text = Regex.Replace(text, @"System\.Windows\.Forms\.MessageBox\.Show", "Proyecto_GALAB.Views.CustomMessageBox.Show");
        text = Regex.Replace(text, @"(?<!Proyecto_GALAB\.Views\.Custom)MessageBox\.Show", "Proyecto_GALAB.Views.CustomMessageBox.Show");
        modified = true;
    }

    if (modified)
    {
        File.WriteAllText(f, text, Encoding.UTF8);
        Console.WriteLine("Updated " + Path.GetFileName(f));
    }
}
