using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NCore.Patterns.Domain.Abstractions.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DomainEventException : Exception
    {
        public DomainEventException()
        {
        }

        protected DomainEventException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DomainEventException(string message) : base(message)
        {
        }
        public DomainEventException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}