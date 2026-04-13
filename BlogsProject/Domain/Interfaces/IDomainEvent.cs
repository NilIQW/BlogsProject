namespace BlogsProject.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}