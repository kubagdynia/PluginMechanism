namespace PluginInterface;

public interface IPlugin
{
    string Name { get; }

    void Do();
}