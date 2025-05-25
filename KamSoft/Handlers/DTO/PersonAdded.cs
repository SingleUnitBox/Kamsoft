namespace KamSoft.Handlers.DTO;

public record PersonAdded(Guid Id, string Name, string Surname) : IEvent;