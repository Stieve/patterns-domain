using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NCore.Patterns.Domain.Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        private readonly string _aggregateIdentifier;

        public AggregateNotFoundException()
        {
        }

        public AggregateNotFoundException(string aggregateIdentifier)
            : this(aggregateIdentifier,
                $"Aggregate with identifier {aggregateIdentifier} not found.")
        {
        }

        public AggregateNotFoundException(string aggregateIdentifier, string message)
            : base(message)
        {
            _aggregateIdentifier = aggregateIdentifier;
        }

        public AggregateNotFoundException(string aggregateIdentifier, string message, Exception innerException)
            : base(message, innerException)
        {
            _aggregateIdentifier = aggregateIdentifier;
        }

        protected AggregateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string AggregateIdentifier => _aggregateIdentifier;
    }
}