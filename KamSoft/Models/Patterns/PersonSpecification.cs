using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public class HasPesel : Specification<Person>
{
    public override bool IsSatisfiedBy(Person person)
    {
        if (person is Person.WithPesel)
        {
            return true;
        }
        
        return false;
    }
}

public class HasAddress : Specification<Person>
{
    public override bool IsSatisfiedBy(Person person)
    {
        if (person is Person.WithAddress)
        {
            return true;
        }
        
        return false;
    }
}

public class IsAdult : Specification<Person>
{
    private readonly HasPesel _hasPesel = new HasPesel();
    
    public override bool IsSatisfiedBy(Person person)
    {
        if (_hasPesel.IsSatisfiedBy(person))
        {
            int.TryParse((person as Person.WithPesel).Pesel.Substring(0, 2), out int year);
            if (year >= 50)
            {
                return true;
            }
        }
        
        return false;
    }
}

public class IsPolishResident : Specification<Person>
{
    private readonly HasAddress _hasAddress = new HasAddress();
    
    public override bool IsSatisfiedBy(Person person)
    {
        if (_hasAddress.IsSatisfiedBy(person))
        {
            if ((person as Person.WithAddress).Address.Country.ToLowerInvariant() == "poland")
            {
                return true;
            }
        }

        return false;
    }
}
