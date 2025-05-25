using KamSoft.Controllers;
using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public class PersonToPersonWithPeselDto : IMapper<Person, PersonWithPeselDto>
{
    public PersonWithPeselDto Map(Person person)
    {
        var pwp = person as Person.WithPesel;
        return new PersonWithPeselDto(pwp.Id, pwp.Name, pwp.Surname, pwp.Pesel);
    }
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