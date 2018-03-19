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
        IReadOnlyList<PropertyInfo> GetDefaultProperties<THubSpot, TEntity>()
            where TEntity : class, IHubSpotEntity, new();

        IReadOnlyList<PropertyInfo> GetCustomProperties<THubSpot, TEntity>()
            where TEntity : class, IHubSpotEntity, new();

        bool TryGetTypeConverter(Type type, out ITypeConverter converter);
    }

    public class TypeConverterRegistration
    {
        public Type Type { get; set; }

        public ITypeConverter Converter { get; set; }
    }

    public class TypeStore : ITypeStore
    {
        private readonly IReadOnlyDictionary<Type, ITypeConverter> _converters;

        private readonly ConcurrentDictionary<Type, PropertyInfo[]> _customProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        private readonly ConcurrentDictionary<Type, PropertyInfo[]> _defaultProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public TypeStore(IReadOnlyList<TypeConverterRegistration> typeConverterRegistrations)
        {
            var converters = CreateRegistry(typeConverterRegistrations ?? Array.Empty<TypeConverterRegistration>());

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

        public IReadOnlyList<PropertyInfo> GetCustomProperties<THubSpot, TEntity>()
            where TEntity : class, IHubSpotEntity, new()
        {
            var properties = _customProperties.GetOrAdd(typeof(TEntity),
                type => type.GetProperties().Where(p => p.GetCustomAttributes<CustomPropertyAttribute>().Any()).ToArray());

            return properties;
        }

        public IReadOnlyList<PropertyInfo> GetDefaultProperties<THubSpot, TEntity>()
            where TEntity : class, IHubSpotEntity, new()
        {
            var properties = _defaultProperties.GetOrAdd(typeof(TEntity),
                type => type.GetProperties().Where(p => p.GetCustomAttributes<DefaultPropertyAttribute>().Any()).ToArray());

            return properties;
        }

        public bool TryGetTypeConverter(Type type, out ITypeConverter converter) => _converters.TryGetValue(type, out converter);
    }
}