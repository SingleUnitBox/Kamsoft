namespace KamSoft.Handlers.LocalDTO;

public record PersonAdded(Guid Id, string Name, string Surname) : IEvent;

public class PersonAddedHandler(ILogger<PersonAddedHandler> logger) : IEventHandler<PersonAdded>
{
    public Task HandleAsync(PersonAdded command)
    {
        logger.LogInformation("Person Added");
        return Task.CompletedTask;;
    }
}