using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public class PersonBuilder
{
    private Guid id;
    private string name;
    private readonly IPersonCreationStrategy policy;
    
    public PersonBuilder(IPersonCreationStrategy policy)
    {
        // add policy to return correct person
        // decider/policy/strategy
        this.policy = policy;
    }

    public PersonBuilder AddId(Guid id)
    {
        this.id = id;
        return this;
    }

    public PersonBuilder AddName(string name)
    {
        this.name = name;
        return this;
    }

    public Person Build()
    {
        var person = policy.Create(id, name);
        return person;
    }
}