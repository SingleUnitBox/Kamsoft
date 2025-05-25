namespace KamSoft.Handlers;

public interface IEvent
{
    
}

public interface ICommand
{
    
}

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command);
}

public interface IEventHandler<in TEvent> where TEvent : class, IEvent
{
    Task HandleAsync(TEvent command);
}

public interface IEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent command) where TEvent : class, IEvent;
}

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
        var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(typeof(TEvent));
        using var scope = _serviceProvider.CreateScope();
        {
            var eventHandler = scope.ServiceProvider.GetService(eventHandlerType);
            return (Task)eventHandlerType
                ?.GetMethod(nameof(IEventHandler<TEvent>.HandleAsync))
                ?.Invoke(eventHandler, new[] { @event });
        }
    }
}