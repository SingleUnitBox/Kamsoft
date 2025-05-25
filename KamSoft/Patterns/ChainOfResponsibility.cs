namespace KamSoft.Patterns;

public interface IHandle<T>
{
    public Task Handle(T input);
}

public class HandleOne : IHandle<string>
{
    public Task Handle(string input)
    {
        throw new NotImplementedException();
    }
}

public class HandleTwo : IHandle<string>
{
    private readonly IHandle<string> _next;
    public Task Handle(string input)
    {
        throw new NotImplementedException();
    }
}