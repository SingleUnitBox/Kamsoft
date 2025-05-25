namespace KamSoft.Handlers;

public static class Extensions
{
    public static IServiceCollection AddModuleRegistry(this IServiceCollection services)
    {
        services.AddScoped<IModuleClient, ModuleClient>();
        services.AddSingleton<ISerializer, Serializer>();
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(a => a.GetTypes());
        var eventTypes = types.Where(t => t.IsClass && typeof(IEvent).IsAssignableFrom(t) && t != typeof(IEvent));
        
        var registry = new ModuleRegistry();
        services.AddSingleton<IModuleRegistry>(sp =>
        {
            var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
            var eventDispatcherType = eventDispatcher.GetType();

            foreach (var eventType in eventTypes)
            {
                registry.Add(eventType, @event =>
                
                    (Task)eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                        ?.MakeGenericMethod(eventType)
                        ?.Invoke(eventDispatcher, new[] { @event }));
            }

            return registry;
        });

        return services;
    }
}