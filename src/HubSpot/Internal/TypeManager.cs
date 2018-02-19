using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Internal
{
    public abstract class TypeManager<THubSpot, TEntity> : ITypeManager<THubSpot, TEntity>
        where TEntity : class, IHubSpotEntity, new()
    {
        private readonly ITypeStore _typeStore;

        protected TypeManager(ITypeStore typeStore)
        {
            _typeStore = typeStore ?? throw new ArgumentNullException(nameof(typeStore));
        }

        public T ConvertTo<T>(THubSpot item)
            where T : class, TEntity, new()
        {
            var entity = new T();

            SetDefaultProperties(item, entity);

            var properties = SetCustomProperties(item, entity);

            entity.Properties = properties.ToDictionary(k => k.PropertyName, i => i.Value);

            return entity;
        }

        private void SetDefaultProperties<T>(THubSpot item, T entity)
            where T : class, TEntity, new()
        {
            var defaultProperties = _typeStore.GetDefaultProperties<THubSpot, T>();

            var hubspotProperties = GetDefaultProperties(item);

            var zipped = from ep in defaultProperties
                         from hp in hubspotProperties
                         let propertyName = GetDefaultPropertyName(ep)
                         where string.Equals(propertyName, hp.Key, StringComparison.OrdinalIgnoreCase)
                         select new
                         {
                             propertyName = hp.Key,
                             property = ep,
                             value = hp.Value,
                             type = ep.PropertyType
                         };

            foreach (var zip in zipped)
            {
                zip.property.SetValue(entity, zip.value);
            }
        }

        private IEnumerable<PropertyData> SetCustomProperties<T>(THubSpot item, T entity)
            where T : class, TEntity, new()
        {
            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            var hubspotProperties = GetCustomProperties(item);

            var zipped = from ep in customProperties
                         from hp in hubspotProperties
                         let propertyName = GetPropertyName(ep)
                         where string.Equals(propertyName, hp.Key, StringComparison.OrdinalIgnoreCase)
                         where HasCustomProperty(item, hp.Key)
                         select new
                         {
                             propertyName = hp.Key,
                             property = ep,
                             value = hp.Value,
                             type = ep.PropertyType
                         };

            foreach (var zip in zipped)
            {
                if (_typeStore.TryGetTypeConverter(zip.type, out var converter) && converter.TryConvertTo(zip.value, out var convertedValue))
                {
                    zip.property.SetValue(entity, convertedValue);

                    yield return new PropertyData(zip.propertyName, convertedValue);
                }
            }
        }

        private static string GetPropertyName(PropertyInfo property) => property.GetCustomAttribute<CustomPropertyAttribute>().PropertyName;

        private static string GetDefaultPropertyName(PropertyInfo property) => property.GetCustomAttribute<DefaultPropertyAttribute>().PropertyName;

        public IReadOnlyList<(string name, PropertyInfo property, CustomPropertyAttribute metadata)> GetCustomProperties<T>(Func<CustomPropertyAttribute, bool> filter)
            where T : class, TEntity, new()
        {
            filter = filter ?? (a => true);

            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            var items = from property in customProperties
                        let attribute = Attribute.GetCustomAttribute(property, typeof(CustomPropertyAttribute)) as CustomPropertyAttribute
                        where filter(attribute)
                        select (property.Name, property, attribute);

            return items.ToArray();
        }

        protected abstract IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(THubSpot item);

        protected abstract IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(THubSpot item);

        protected abstract bool HasCustomProperty(THubSpot item, string propertyName);

        public IReadOnlyList<(string name, string value)> GetModifiedProperties<T>(T item)
            where T : class, TEntity, new()
        {
            var properties = GetCustomProperties<T>(TypeManager.ModifiableProperties);

            var data = from property in properties
                       let value = property.property.GetValue(item)
                       select new PropertyData
                       {
                           PropertyName = property.metadata.PropertyName,
                           Value = value
                       };

            var modifiedProperties = GetModifiedProperties(data, item);

            return modifiedProperties.ToArray();
        }

        private IEnumerable<(string name, string value)> GetModifiedProperties(IEnumerable<PropertyData> data, IHubSpotEntity entity)
        {
            foreach (var property in data)
            {
                if (!_typeStore.TryGetTypeConverter(property.Value.GetType(), out var converter))
                {
                    throw new Exception($"Converter not found for {property.Value.GetType()}");
                }

                if (!converter.TryConvertFrom(property.Value, out var newValue))
                {
                    throw new Exception($"{converter.GetType()} could not convert {property.Value ?? "<null>"} for {property.PropertyName}");
                }

                if (!entity.Properties.TryGetValue(property.PropertyName, out var value))
                {
                    yield return (property.PropertyName, newValue);
                }
                else
                {
                    if (!converter.TryConvertFrom(value, out string oldValue))
                    {
                        throw new Exception($"{converter.GetType()} could not convert {value ?? "<null>"} for {property.PropertyName}");
                    }

                    if (!string.Equals(newValue, oldValue))
                    {
                        yield return (property.PropertyName, newValue);
                    }
                }
            }
        }
    }
}