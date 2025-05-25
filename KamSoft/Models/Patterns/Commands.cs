using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public record AddCompletePerson(string Name, string Surname, string Pesel,
    string Street, string City, string Country, string ZipCode) : ICommand;

public class AddPersonCommandHandler : ICommandHandler<AddCompletePerson>
{
    public async Task Handle(AddCompletePerson command)
    {
        var person = new Person.Complete(Guid.NewGuid(), command.Name, command.Surname, command.Pesel,
            new Address(command.Street, command.City, command.Country, command.ZipCode));
        var personValidator = new CompletePersonValidator();
        var validatedPerson = personValidator.Execute(person);
        
        await Task.Delay(1_000);
    }
}

public record AddPesel(Guid PersonId, string Pesel) : ICommand;

public class AddPeselCommandHandler : ICommandHandler<AddPesel>
{
    public Task Handle(AddPesel command)
    {
        throw new NotImplementedException();
    }
}

public record RemoveAddress(Guid PersonId) : ICommand;

public class RemoveAddressCommandHandler : ICommandHandler<RemoveAddress>
{
    public Task Handle(RemoveAddress command)
    {
        throw new NotImplementedException();
    }
}