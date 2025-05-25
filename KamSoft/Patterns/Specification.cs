using KamSoft.Models;

namespace KamSoft.Patterns;

// use specs to validate person
public interface ISpecification<T> where T : class
{
    bool IsSatisfiedBy(T input);
}

public abstract class Specification<T> : ISpecification<T> where T : class
{
    public abstract bool IsSatisfiedBy(T input);
    
    public ISpecification<T> And(ISpecification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }
    
    public ISpecification<T> Or(ISpecification<T> other)
    {
        return new OrSpecification<T>(this, other);
    }

    public ISpecification<T> Not(ISpecification<T> other)
    {
        return new NotSpecification<T>(other);
    }
}

public class AndSpecification<T> : Specification<T> where T : class
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }
    
    public override bool IsSatisfiedBy(T input)
    {
        return _left.IsSatisfiedBy(input) && _right.IsSatisfiedBy(input);
    }
}

public class OrSpecification<T> : Specification<T> where T : class
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }
    
    public override bool IsSatisfiedBy(T input)
    {
        return _left.IsSatisfiedBy(input) || _right.IsSatisfiedBy(input);
    }
}

public class NotSpecification<T> : Specification<T> where T : class
{
    private readonly ISpecification<T> _spec;

    public NotSpecification(ISpecification<T> spec)
    {
        _spec = spec;
    }
    
    public override bool IsSatisfiedBy(T input)
    {
        return !_spec.IsSatisfiedBy(input);
    }
}

public class Rule<T> where T : class
{
    public ISpecification<T> Spec;

    public Rule(ISpecification<T> spec)
    {
        Spec = spec;
    }
}

public abstract class Validator<T> where T : class
{
    private readonly List<Rule<T>> _rules = new();

    public Validator<T> Add(Rule<T> rule)
    {
        _rules.Add(rule);
        return this;
    }

    public T Execute(T input)
    {
        foreach (var rule in _rules)
        {
            if (rule.Spec.IsSatisfiedBy(input) == false)
            {
                throw new Exception($"Validation failed - {rule.Spec.GetType().Name}");
            }
        }
        return input;
    }
}