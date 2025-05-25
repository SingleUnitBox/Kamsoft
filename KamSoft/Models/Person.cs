using System.Text.Json.Serialization;
using KamSoft.Patterns;

namespace KamSoft.Models;

[JsonDerivedType(typeof(Simple), "Simple")]
[JsonDerivedType(typeof(WithPesel), "WithPesel")]
public abstract record Person(Guid Id) : IRepositoryObject
{
    public record Simple(Guid Id, string Name, string Surname) : Person(Id);
    
    public record WithPesel(Guid Id, string Name, string Surname, string Pesel) : Person(Id);
    
    public record WithAddress(Guid Id, string Name, string Surname, Address Address) : Person(Id);
    
    public record Complete(Guid Id, string Name, string Surname, string Pesel, Address Address) : Person(Id);
    
}

public record Address(string Street, string City, string Country, string ZipCode);

public record PersonWithPeselDto(Guid Id, string Name, string Surname, string Pesel);
public record PersonSimpleDto(Guid Id, string Name, string Surname);