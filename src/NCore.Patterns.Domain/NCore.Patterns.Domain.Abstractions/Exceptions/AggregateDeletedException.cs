using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NCore.Patterns.Domain.Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AggregateDeletedException : AggregateNotFoundException
    {
        public AggregateDeletedException()
        {
        }

        public AggregateDeletedException(string aggregateIdentifier)
            : this(aggregateIdentifier,
                $"Aggregate with identifier {aggregateIdentifier} not found. It has been deleted.")
        {
        }

        public AggregateDeletedException(string aggregateIdentifier, string message)
            : base(aggregateIdentifier, message)
        {
        }

        public AggregateDeletedException(string aggregateIdentifier, string message, Exception innerException)
            : base(aggregateIdentifier, message, innerException)
        {
        }

        protected AggregateDeletedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}