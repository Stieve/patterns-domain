namespace NCore.Patterns.Domain.Abstractions
{
    public interface IAggregateCommand
    {
        string Key { get; }
    }
}