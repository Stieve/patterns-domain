using FluentAssertions;
using NCore.Patterns.Domain.UnitTests.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NCore.Patterns.Domain.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class ValueObjectTests
    {
        [Test]
        public void TestEquals()
        {
            var address1 = new Address("Blarenberglaan 3b", "Mechelen", "Antwerpen", "Belgium", "2800");
            var address2 = new Address("Blarenberglaan 3b", "Mechelen", "Antwerpen", "Belgium", "2800");

            address1.Should().BeEquivalentTo(address2);
        }

        [Test]
        public void TestNotEquals()
        {
            var address1 = new Address("Blarenberglaan 3b", "Mechelen", "Antwerpen", "Belgium", "2800");
            var address2 = new Address("Ringwade 1", "Nieuwegein", "", "The Netherlands", "3439 LM");

            address1.Should().NotBeEquivalentTo(address2);
        }
    }
}
