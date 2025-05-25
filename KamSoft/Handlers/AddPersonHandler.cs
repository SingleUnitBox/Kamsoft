using KamSoft.Handlers.DTO;
using KamSoft.Models;

namespace KamSoft.Handlers;

public record AddPerson(string Name, string Surname) : ICommand;

public class AddPersonHandler(
    //IEventDispatcher eventDispatcher,
    IModuleClient moduleClient
    ) : ICommandHandler<AddPerson>
{
    public async Task HandleAsync(AddPerson command)
    {
        var person = new Person.Simple(Guid.NewGuid(), command.Name, command.Surname);
        
        var @event = new PersonAdded(person.Id, person.Name, person.Surname);
        //await eventDispatcher.PublishAsync(@event);
        await moduleClient.PublishAsync(@event);
    }
}