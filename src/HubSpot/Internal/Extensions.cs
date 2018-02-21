using System;
using System.Collections.Generic;
using System.Linq;
using HubSpot.Deals;

namespace HubSpot.Internal
{
    internal static class Extensions
    {
        public static IEnumerable<T> NotIn<T>(this IEnumerable<T> items, IEnumerable<T> others)
            where T : IEquatable<T>
        {
            return items.Where(i => !others.Contains(i));
        }

        public static IReadOnlyDictionary<T1, ILookup<T2, TValue>> ToNestedLookup<T, T1, T2, TValue>(this IReadOnlyList<T> items, Func<T, T1> externalKeySelector, Func<T, T2> internalKeySelector, Func<T, TValue> valueSelector)
        {
            if (items == null || items.Count == 0)
            {
                return new Dictionary<T1, ILookup<T2, TValue>>();
            }

            IReadOnlyDictionary<T1, ILookup<T2, TValue>> dictionaries = items.GroupBy(externalKeySelector).ToDictionary(k => k.Key, v => v.ToLookup(internalKeySelector, valueSelector));

            return dictionaries;
        }

        public static IReadOnlyDictionary<T1, ILookup<T2, T>> ToNestedLookup<T, T1, T2>(this IReadOnlyList<T> items, Func<T, T1> externalKeySelector, Func<T, T2> internalKeySelector) => ToNestedLookup(items, externalKeySelector, internalKeySelector, i => i);

        public static IReadOnlyList<TValue> GetValues<T1, T2, TValue>(this IReadOnlyDictionary<T1, ILookup<T2, TValue>> items, T1 outerKey, T2 innerKey)
        {
            if (items.TryGetValue(outerKey, out var lookup) && lookup.Contains(innerKey))
            {
                return lookup[innerKey].ToArray();
            }

            return Array.Empty<TValue>();
        }
    }
}