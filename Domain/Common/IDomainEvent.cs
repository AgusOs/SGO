/// <summary>
/// Marca un evento de dominio, que representa un hecho ocurrido en el modelo.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
