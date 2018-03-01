using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using NUnit.Framework;

namespace Tests
{
    public static class TestExtensions
    {
        public static bool IsSupersetOf<T>(this IEnumerable<T> superset, IEnumerable<T> subset)
        {
            CollectionAssert.IsSupersetOf(superset, subset);
            return true;
        }

        public static bool IsEquivalentOf<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            CollectionAssert.AreEquivalent(first, second);
            return true;
        }

        public static ISetupSequentialResult<T> ReturnsSequence<T>(this ISetupSequentialResult<T> setup, IEnumerable<T> results)
        {
            foreach (var item in results)
            {
                setup.Returns(item);
            }

            return setup;
        }

        public static ISetupSequentialResult<Task<T>> ReturnsSequenceAsync<T>(this ISetupSequentialResult<Task<T>> setup, IEnumerable<T> results)
        {
            foreach (var item in results)
            {
                setup.ReturnsAsync(item);
            }

            return setup;
        }

        public static IReturnsResult<TMock> ReturnsAsyncUsingFixture<TMock, TResult>(this IReturns<TMock, Task<TResult>> setup, ISpecimenBuilder fixture)
            where TMock : class
        {
            var context = new SpecimenContext(fixture);

            return setup.ReturnsAsync(() =>
            {
                var obj = context.Resolve(typeof(TResult));
                return (TResult)obj;
            });
        }
    }
}