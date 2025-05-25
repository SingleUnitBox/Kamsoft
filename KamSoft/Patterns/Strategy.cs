using KamSoft.Models;

namespace KamSoft.Patterns;

public interface IPersonCreationStrategy
{
    // bool IsValid();
    Person Create(Guid id, string name);
}

public class StrategySimple : IPersonCreationStrategy
{
    public Person Create(Guid id, string name)
        => new Person.Simple(id, name, "Simplowski");

}

public class StrategyWithPesel : IPersonCreationStrategy
{
    public Person Create(Guid id, string name)
        => new Person.WithPesel(id, name, "Peselowski", "0123456789");
}

public class StrategyWithAddress : IPersonCreationStrategy
{
    public Person Create(Guid id, string name)
        => new Person.WithAddress(id, name, "Adresowski",
            new Address("Nowa 7", "Gawrychy", "podlaskie", "18-400"));
}