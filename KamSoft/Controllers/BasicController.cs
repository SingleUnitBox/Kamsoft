using KamSoft.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace KamSoft.Controllers;

public record PersonRequest(Guid Id, string Name, string Surname, string? Pesel = null);

public record AddressRequest(string Street, string City, string State, string ZipCode);

[ApiController]
[Route("/[controller]")]
public class BasicController
{
    [HttpPost(Name = "Persons")]
    public async Task<IResult> Create(PersonRequest request)
    {
        var mapper = new PersonRequestMapper();
        var person = mapper.Map(request);
        // TODO
        return Results.Ok(person);
    }
}