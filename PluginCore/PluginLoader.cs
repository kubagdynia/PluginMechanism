using System.Reflection;

namespace PluginCore;

public static class PluginLoader<T> where T : class
{
    public static ICollection<T> LoadPlugins(string path, string searchPattern = "*.dll")
    {
        // Check if the directory exists; if not, return an empty list
        if (!Directory.Exists(path))
        {
            return new List<T>();
        }
        
        // Get files matching the search pattern in the directory
        var files = Directory.GetFiles(path, searchPattern);

        // Load assemblies from the files
        var assemblies = new List<Assembly>(files.Length);
        foreach (var file in files)
        {
            var assembly = Assembly.LoadFrom(file);
            assemblies.Add(assembly);
        }

        var pluginType = typeof(T);
        var pluginTypes = new List<Type>();
        
        // Filter types that implement the plugin interface
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(type =>
                type is { IsInterface: false, IsAbstract: false } && type.GetInterface(pluginType.FullName!) != null);
            pluginTypes.AddRange(types);
        }

        var plugins = new List<T>(pluginTypes.Count);
        foreach (var type in pluginTypes)
        {
            if (Activator.CreateInstance(type) is T plugin)
            {
                plugins.Add(plugin);
            }
        }

        return plugins;
    }
}
