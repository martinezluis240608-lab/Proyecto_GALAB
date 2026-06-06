using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Proyecto_GALAB.Services;

public class PerfilAdicional
{
    public string Id { get; set; } = string.Empty;
    public string Curp { get; set; } = string.Empty;
    public string FechaNacimiento { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Calle { get; set; } = string.Empty;
    public string Colonia { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string RutaFotoPerfil { get; set; } = string.Empty;
}

public static class PerfilAdicionalStore
{
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "perfiles_adicionales.json");
    private static readonly ConcurrentDictionary<string, PerfilAdicional> Cache = new();

    static PerfilAdicionalStore()
    {
        Cargar();
    }

    private static void Cargar()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var lista = JsonSerializer.Deserialize<List<PerfilAdicional>>(json);
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Id))
                        {
                            Cache[item.Id] = item;
                        }
                    }
                }
            }
        }
        catch
        {
            // Ignorar errores al cargar
        }
    }

    private static void Guardar()
    {
        try
        {
            var lista = Cache.Values.ToList();
            string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch
        {
            // Ignorar errores al guardar
        }
    }

    public static PerfilAdicional Obtener(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return new PerfilAdicional();
        return Cache.TryGetValue(id, out var val) ? val : new PerfilAdicional { Id = id };
    }

    public static void Guardar(string id, PerfilAdicional perfil)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        perfil.Id = id;
        Cache[id] = perfil;
        Guardar();
    }
}
