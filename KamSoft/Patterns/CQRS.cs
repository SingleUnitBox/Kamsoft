using KamSoft.Models;

namespace KamSoft.Patterns;

public interface ICommand;
public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    public Task Handle(TCommand command);
}

public interface IQuery;
public interface IQuery<TResult> : IQuery;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Query(TQuery query);
}

