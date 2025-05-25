namespace KamSoft.Models;

public abstract record BasicResource;
public class Resource<T>(T data) where T : BasicResource;

public record struct Slot(DateTime Start, DateTime End);
public record Visit(Guid Id, Person Patient, Slot Slot) : BasicResource;