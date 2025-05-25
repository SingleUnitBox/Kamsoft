namespace KamSoft.Handlers;

public interface IModuleRegistry
{
    BroadcastReg Resolve(string key);
    void Add(Type requestType, Func<object, Task> action);
}