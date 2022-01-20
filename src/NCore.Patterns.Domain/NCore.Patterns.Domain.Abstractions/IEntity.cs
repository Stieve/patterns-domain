namespace NCore.Patterns.Domain.Abstractions
{

    public interface IEntity : IEntity<string> { }

    public interface IEntity<TKey>
    {
        public TKey Key { get; }
    }
}