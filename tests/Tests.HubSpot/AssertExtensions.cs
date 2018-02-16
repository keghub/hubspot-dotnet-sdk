using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public static class AssertExtensions
    {
        public static bool IsSupersetOf<T>(this IEnumerable<T> superset, IEnumerable<T> subset)
        {
            CollectionAssert.IsSupersetOf(superset, subset);
            return true;
        }
    }
}