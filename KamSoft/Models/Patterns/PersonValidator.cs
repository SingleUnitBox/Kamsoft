using KamSoft.Patterns;

namespace KamSoft.Models.Patterns;

public class CompletePersonValidator : Validator<Person>
{
    public CompletePersonValidator()
    {
        Add(new Rule<Person>(new HasAddress().And(new HasPesel())));
    }
}

public class AdultPersonValidator : Validator<Person>
{
    public AdultPersonValidator()
    {
        Add(new Rule<Person>(new IsAdult()));
    }
}