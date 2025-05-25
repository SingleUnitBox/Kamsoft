using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public record GetPersonByPesel(string Pesel) : IQuery<PersonWithPeselDto>;

public class GetPersonByPeselHandler : IQueryHandler<GetPersonByPesel, PersonWithPeselDto>
{
    public Task<PersonWithPeselDto> Query(GetPersonByPesel query)
    {
        var personFromDb = new Person.WithPesel(Guid.NewGuid(), "Pesel", "Peselowski", query.Pesel);
        var mapper = new PersonToPersonWithPeselDto();
        var personDto = mapper.Map(personFromDb);
        return Task.FromResult(personDto);
    }
}

public record GetPeopleByCity(string City) : IQuery<IEnumerable<PersonSimpleDto>>;

public class GetPeopleByCityHandler : IQueryHandler<GetPeopleByCity, IEnumerable<PersonSimpleDto>>
{
    public Task<IEnumerable<PersonSimpleDto>> Query(GetPeopleByCity query)
    {
        return Task.FromResult(new List<PersonSimpleDto>().AsEnumerable());
    }
}