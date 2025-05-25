namespace KamSoft.Patterns;

public abstract class Maybe<T>
{
    public abstract Maybe<T1> Map<T1>(Func<T, T1> func);
    public abstract TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern);

    public Maybe<T1> Bind<T1>(Func<T, Maybe<T1>> func) =>
        this.MatchWith((
        none: () => new None<T1>(),
        some: (v) => func(v)
        ));
}

public class None<T> : Maybe<T>
{
    public override Maybe<T1> Map<T1>(Func<T, T1> func)
    {
        throw new NotImplementedException();
    }

    public override TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern)
    {
        throw new NotImplementedException();
    }
}

public class Some<T> : Maybe<T>
{
    private readonly T value;

    public Some(T value)
    {
        this.value = value;
    }

    public override Maybe<T1> Map<T1>(Func<T, T1> func)
    {
        throw new NotImplementedException();
    }

    public override TResult MatchWith<TResult>((Func<TResult> none, Func<T, TResult> some) pattern)
    {
        throw new NotImplementedException();
    }
}