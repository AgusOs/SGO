namespace SGO.Domain.Common;

/// <summary>
/// Clase base para todas las entidades del dominio.
/// Contiene un Id único y manejo básico de eventos de dominio.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();

    // Token de concurrencia (opcional, se usa con EF Core)
    public uint Version { get; internal set; }
}
