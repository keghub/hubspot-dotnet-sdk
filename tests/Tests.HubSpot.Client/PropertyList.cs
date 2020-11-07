using System.Collections.Generic;
using HubSpot.Model;
using Moq;
using NUnit.Framework;


namespace Tests
{
    public static class PropertyList
    {
        public static PropertyList<T> Contains<T>(IReadOnlyList<T> items)
            where T : IValuedProperty
        {
            return Match.Create<PropertyList<T>>(list =>
            {
                Assert.That(list.Properties, Is.EquivalentTo(items));
                return true;
            });
        }
    }
}