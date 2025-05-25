using KamSoft.Controllers;
using KamSoft.Models;

namespace KamSoft.Patterns;

public interface IMapper<in TInput, out TOutput>
    where TInput : class
    where TOutput : class
{
    TOutput Map(TInput input);
}

public class PersonRequestMapper : IMapper<PersonRequest, Person>
{
    public Person Map(PersonRequest input)
    {
        if (string.IsNullOrEmpty(input.Pesel))
        {
            return new Person.Simple(input.Id, input.Name, input.Surname);
        }
        else
        {
            return new Person.WithPesel(input.Id, input.Name, input.Surname, input.Pesel);
        }
    }
}

public class AddressRequestMapper : IMapper<AddressRequest, Address>
{
    public Address Map(AddressRequest input)
        => new Address(input.Street, input.City, input.State, input.ZipCode);
}