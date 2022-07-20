namespace Shared.Domain.SeedWork
{
    /// <summary>
    /// Interface that identifies the root entities of your aggregates within domains or services.
    /// </summary>
    /// <example>
    /// Foo class used in this example.
    /// <para>Indicate that class is aggregate root.</para>
    /// <code>
    /// public class Foo : IAggregateRoot { }
    /// </code>
    /// </example>
    public interface IAggregateRoot : IModel
    {
    }
}
