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
            try
            {
                var assembly = Assembly.LoadFrom(file);
                assemblies.Add(assembly);
            }
            catch (Exception ex) when (ex is BadImageFormatException or FileLoadException)
            {
                // Log or handle the exception if necessary
                Console.WriteLine(ex.Message);
            }
        }

        var pluginType = typeof(T);
        var pluginTypes = new List<Type>();
        
        // Filter types that implement the plugin interface
        foreach (var assembly in assemblies)
        {
            try
            {
                var types = assembly.GetTypes().Where(type =>
                    type is { IsInterface: false, IsAbstract: false } && type.GetInterface(pluginType.FullName!) != null);
                pluginTypes.AddRange(types);
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Log or handle the exception if necessary
                foreach (var type in ex.Types)
                {
                    if (type?.GetInterface(pluginType.FullName!) != null)
                    {
                        pluginTypes.Add(type);
                    }
                }
                Console.WriteLine(ex.Message);
            }
        }

        var plugins = new List<T>(pluginTypes.Count);
        foreach (var type in pluginTypes)
        {
            try
            {
                if (Activator.CreateInstance(type) is T plugin)
                {
                    plugins.Add(plugin);
                }
            }
            catch (Exception ex) when (ex is MissingMethodException or TargetInvocationException)
            {
                // Log or handle the exception if necessary
                Console.WriteLine(ex.Message);
            }
        }

        return plugins;
    }
}
