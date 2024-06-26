using PluginCore;
using PluginInterface;

var plugins = new Dictionary<string, IPlugin>();

var pluginList = PluginLoader<IPlugin>.LoadPlugins("plugins");
foreach (var item in pluginList)
{
    plugins.TryAdd(item.Name, item);
}

foreach (var plugin in plugins)
{
    plugin.Value.Do();
}