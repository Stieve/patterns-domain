using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NCore.Patterns.Domain.Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AggregateAlreadyExistsException : AggregateNotFoundException
    {
        public AggregateAlreadyExistsException()
        {
        }

        public AggregateAlreadyExistsException(string aggregateIdentifier)
            : this(aggregateIdentifier,
                $"Aggregate with identifier {aggregateIdentifier} already exists.")
        {
        }

        public AggregateAlreadyExistsException(string aggregateIdentifier, string message)
            : base(aggregateIdentifier, message)
        {
        }

        public AggregateAlreadyExistsException(string aggregateIdentifier, string message, Exception innerException)
            : base(aggregateIdentifier, message, innerException)
        {
        }

        protected AggregateAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}