namespace KamSoft.Handlers;

public interface IModuleClient
{
    Task PublishAsync(object @event);
}

public class ModuleClient  : IModuleClient
{
    private readonly IModuleRegistry _registry;
    private readonly ISerializer _serializer;

    public ModuleClient(IModuleRegistry registry, ISerializer serializer)
    {
        _registry = registry;
        _serializer = serializer;
    }

    public async Task PublishAsync(object @event)
    {
        var reg = _registry.Resolve(@event.GetType().Name);
        if (reg is not null)
        {
            var translatedEvent = Translate(@event, reg.RequestType);
            await reg.Action(translatedEvent);
        }
    }

    private object Translate(object @event, Type eventType)
        => _serializer.Deserialize(_serializer.Serialize(@event), eventType);
}