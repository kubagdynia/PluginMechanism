using PluginInterface;

namespace FirstPlugin;

public class FirstPlugin : IPlugin
{
    public string Name => "First Plugin";
    
    public void Do()
    {
        Console.WriteLine("Do {0}", Name);
    }
}