using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace Tests
{
    public class LookupBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type typeRequest
                && typeRequest.IsGenericType
                && typeRequest.GetGenericTypeDefinition() == typeof(ILookup<,>))
            {
                var args = typeRequest.GetGenericArguments();
                return this.GetType()
                           .GetMethod(nameof(GenerateTypedLookup), BindingFlags.Static | BindingFlags.NonPublic)
                           .MakeGenericMethod(args)
                           .Invoke(null, new object[] { context });
            }

            return new NoSpecimen();
        }

        private static ILookup<TKey, TValue> GenerateTypedLookup<TKey, TValue>(ISpecimenContext context)
        {
            return context.CreateMany<KeyValuePair<TKey, IEnumerable<TValue>>>()
                          // Make sequence flat to later generate lookup from it.
                          .SelectMany(kv => kv.Value.Select(v => new KeyValuePair<TKey, TValue>(kv.Key, v)))
                          .ToLookup(kv => kv.Key, kv => kv.Value);
        }
    }
}