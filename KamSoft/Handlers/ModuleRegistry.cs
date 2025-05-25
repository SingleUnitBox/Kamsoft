namespace KamSoft.Handlers;

public class ModuleRegistry : IModuleRegistry
{
    private readonly List<BroadcastReg> _broadcasts = new();
    
    public BroadcastReg Resolve(string key)
    {
        var reg = _broadcasts.FirstOrDefault(br => br.Key == key);
        if (reg is not null)
        {
            return reg;
        }

        return null;
    }

    public void Add(Type requestType, Func<object, Task> action)
    {
        var reg = new BroadcastReg(requestType, action);
        _broadcasts.Add(reg);
    }
}