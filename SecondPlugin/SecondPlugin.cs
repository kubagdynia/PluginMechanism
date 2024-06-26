using PluginInterface;

namespace SecondPlugin;

public class SecondPlugin : IPlugin
{
    public string Name => "Sesond Plugin";
    
    public void Do()
    {
        Console.WriteLine("Do {0}", Name);
    }
}
