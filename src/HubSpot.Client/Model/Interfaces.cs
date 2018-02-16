using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Model
{
    public interface IObjectWithProperties
    {
        IReadOnlyDictionary<string, IVersionedProperty> Properties { get; }
    }

    public interface IVersionedProperty
    {
        string Value { get; }

        IReadOnlyList<ITimestampedValue> Versions { get; }
    }

    public interface ITimestampedValue
    {
        string Value { get; }

        DateTimeOffset Timestamp { get; }
    }
}