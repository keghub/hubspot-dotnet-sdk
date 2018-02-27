using System.Collections.Generic;
using System.Threading.Tasks;
using Moq.Language.Flow;
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

        public static IReturnsResult<T> CompletesAsync<T>(this ISetup<T, Task> setup) where T : class
        {
            return setup.Returns(Task.CompletedTask);
        }
    }
}