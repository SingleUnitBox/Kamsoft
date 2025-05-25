namespace KamSoft.Handlers;

public record BroadcastReg(Type RequestType, Func<object, Task> Action)
{
    public string Key { get; } = RequestType.Name;
}