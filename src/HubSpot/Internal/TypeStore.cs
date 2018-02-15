using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HubSpot.Converters;

namespace HubSpot.Internal
{
    public interface ITypeStore
    {
        ConstructorInfo GetConstructor<THubSpot, TEntity>()
            where TEntity : HubSpotEntity<THubSpot>;

        IReadOnlyList<PropertyInfo> GetCustomProperties<THubSpot, TEntity>()
            where TEntity : HubSpotEntity<THubSpot>;

        bool TryGetTypeConverter(Type type, out ITypeConverter converter);
    }

    public struct TypeConverterRegistration
    {
        public Type Type { get; set; }

        public ITypeConverter Converter { get; set; }
    }

    public class TypeStore : ITypeStore
    {
        private readonly IReadOnlyDictionary<Type, ITypeConverter> _converters;

        private readonly ConcurrentDictionary<Type, ConstructorInfo> _constructors = new ConcurrentDictionary<Type, ConstructorInfo>();

        private readonly ConcurrentDictionary<Type, PropertyInfo[]> _properties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public TypeStore(IReadOnlyList<TypeConverterRegistration> typeConverterRegistrations)
        {
            if (typeConverterRegistrations == null)
            {
                throw new ArgumentNullException(nameof(typeConverterRegistrations));
            }

            var converters = CreateRegistry(typeConverterRegistrations);

            _converters = converters;
        }

        private static Dictionary<Type, ITypeConverter> CreateRegistry(IReadOnlyList<TypeConverterRegistration> typeConverterRegistrations)
        {
            var converters = new Dictionary<Type, ITypeConverter>
            {
                [typeof(string)] = new StringTypeConverter()
            };

            foreach (var registration in typeConverterRegistrations)
            {
                converters[registration.Type] = registration.Converter;
            }

            return converters;
        }

        public ConstructorInfo GetConstructor<THubSpot, TEntity>()
            where TEntity : HubSpotEntity<THubSpot>
        {
            var constructor = _constructors.GetOrAdd(typeof(TEntity), type => type.GetConstructor(new[] { typeof(THubSpot) }));
            return constructor;
        }

        public IReadOnlyList<PropertyInfo> GetCustomProperties<THubSpot, TEntity>()
            where TEntity : HubSpotEntity<THubSpot>
        {
            var properties = _properties.GetOrAdd(typeof(TEntity), 
                type => type.GetProperties().Where(p => p.GetCustomAttributes<CustomPropertyAttribute>().Any()).ToArray());

            return properties;
        }

        public bool TryGetTypeConverter(Type type, out ITypeConverter converter) => _converters.TryGetValue(type, out converter);
    }
}