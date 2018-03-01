using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests.HubSpot")]
[assembly: InternalsVisibleTo("Tests.Integrations")]


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

        public static object DefaultValue(this Type type)
        {
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static bool IsDefaultValue(this object obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Equals(obj.GetType().DefaultValue());
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int size) => Batch(items, size, f => f);

        public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> items, int size, Func<IEnumerable<TSource>, TResult> selector)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if (items == null)
            {
                return Enumerable.Empty<TResult>();
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Batch();

            IEnumerable<TResult> Batch()
            {
                TSource[] bucket = null;

                var count = 0;

                using (var enumerator = items.GetEnumerator())
                {

                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;

                        if (bucket == null)
                        {
                            bucket = new TSource[size];
                        }

                        bucket[count++] = item;

                        if (count != size)
                        {
                            continue;
                        }

                        yield return selector(bucket);

                        bucket = null;

                        count = 0;
                    }

                    if (bucket != null && count > 0)
                    {
                        Array.Resize(ref bucket, count);
                        yield return selector(bucket);
                    }
                }
            }
        }
    }
}