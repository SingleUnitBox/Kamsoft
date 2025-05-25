using System.Collections.Concurrent;
using KamSoft.Models;

namespace KamSoft.Decider;

// zastosuj decider
// public abstract record Event(Guid Id, DateTime OccuredAt);
//
// public record Booked(Guid Id, DateTime Start, DateTime End, DateTime OccuredAt) : Event(Id, OccuredAt);
//
// public record Cancelled(Guid Id, DateTime OccuredAt) : Event(Id, OccuredAt);

// same as above
public abstract record SlotEvent(Guid Id, DateTime OccuredAt)
{
    public record Booked(Guid Id, DateTime Start, DateTime End, DateTime OccuredAt) : SlotEvent(Id, OccuredAt);
    public record Cancelled(Guid Id, DateTime OccuredAt) : SlotEvent(Id, OccuredAt);
}

public abstract record Command
{
    public record Create(DateTime Start, DateTime End) : Command;
    public record Cancel : Command;
}

public abstract record SlotState
{
    public sealed record Initial : SlotState;
    public sealed record Free(Guid Id) : SlotState;
    public sealed record Booked(Guid Id, DateTime Start, DateTime End) : SlotState;
}

public static class Decider
{
    private static ConcurrentQueue<SlotEvent> _events = new();
    
    public static SlotState Fold(this IEnumerable<SlotEvent> events, SlotState state)
        => events.Aggregate(state, Evolve);

    public static SlotState Fold(this IEnumerable<SlotEvent> events, Slot slot)
        => events.Fold(new SlotState.Initial());

    public static SlotState Evolve(SlotState state, SlotEvent @event) =>
        (state, @event) switch
        {
            (SlotState.Initial, SlotEvent.Booked e) => new SlotState.Booked(Guid.NewGuid(), e.Start, e.End),
            (SlotState.Free ssf, SlotEvent.Booked e) => new SlotState.Booked(ssf.Id, e.Start, e.End),
            (SlotState.Booked ssb, SlotEvent.Cancelled) => new SlotState.Free(ssb.Id),
            _ => state
        };
    

    public static IEnumerable<SlotEvent> Decide(this SlotState state, Command command) =>
        (state, command) switch
        {
            (SlotState.Initial, Command.Create c) => Book(c.Start, c.End),
            (SlotState.Free, Command.Create c) => Book(c.Start, c.End),
            (SlotState.Booked, Command.Cancel) => Cancel(),
            _ => throw new NotImplementedException()
        };

    public static IEnumerable<SlotEvent> Book(DateTime start, DateTime end)
    {
        var @event = new SlotEvent.Booked(Guid.NewGuid(), start, end, DateTime.UtcNow);
        _events.Enqueue(@event);
        
        return _events;
    }

    public static IEnumerable<SlotEvent> Cancel()
    {
        var @event = new SlotEvent.Cancelled(Guid.NewGuid(), DateTime.UtcNow);
        _events.Enqueue(@event);
        
        return _events;
    }
}