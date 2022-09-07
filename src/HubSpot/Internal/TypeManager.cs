using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public virtual T ConvertTo<T>(THubSpot item)
            where T : class, TEntity, new()
        {
            var entity = new T();

            SetDefaultProperties(item, entity);

            var properties = SetCustomProperties(item, entity);

            entity.Properties = properties.ToDictionary(k => k.PropertyName, i => i.Value);

            return entity;
        }

        protected void SetDefaultProperties<T>(THubSpot item, T entity)
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

        protected IEnumerable<PropertyData> SetCustomProperties<T>(THubSpot item, T entity)
            where T : class, TEntity, new()
        {
            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            var hubspotProperties = GetCustomProperties(item);

            var zipped = from ep in customProperties
                         from hp in hubspotProperties
                         let propertyName = GetPropertyName(ep)
                         where string.Equals(propertyName, hp.Key, StringComparison.OrdinalIgnoreCase)
                         select new
                         {
                             propertyName = hp.Key,
                             property = ep,
                             value = hp.Value,
                             type = ep.PropertyType
                         };
            List<PropertyData> propertyDataList = new List<PropertyData>();
            foreach (var zip in zipped)
            {
                if (_typeStore.TryGetTypeConverter(zip.type, out var converter) && converter.TryConvertTo(zip.value, out var convertedValue))
                {
                    zip.property.SetValue(entity, convertedValue);

                    propertyDataList.Add(new PropertyData(zip.propertyName, convertedValue));
                }
            }
            return propertyDataList;
        }

        private static string GetPropertyName(PropertyInfo property) => property.GetCustomAttribute<CustomPropertyAttribute>().FieldName;

        private static string GetDefaultPropertyName(PropertyInfo property) => property.GetCustomAttribute<DefaultPropertyAttribute>().PropertyName;

        public IReadOnlyList<CustomPropertyInfo> GetCustomProperties<T>(Func<CustomPropertyAttribute, bool> filter)
            where T : class, TEntity, new()
        {
            filter ??= (a => true);

            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            var items = from property in customProperties
                        let attribute = Attribute.GetCustomAttribute(property, typeof(CustomPropertyAttribute)) as CustomPropertyAttribute
                        where filter(attribute)
                        select new CustomPropertyInfo
                        {
                            PropertyName = property.Name,
                            FieldName = attribute.FieldName,
                            IsReadOnly = attribute.IsReadOnly,
                            PropertyType = property.PropertyType,
                            ValueAccessor = property.GetValue
                        };

            return items.ToArray();
        }

        protected abstract IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(THubSpot item);

        protected abstract IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(THubSpot item);

        public IReadOnlyList<PropertyData> GetPropertyData<T>(T item)
            where T : class, TEntity, new()
        {
            var properties = GetCustomProperties<T>(TypeManager.ModifiableProperties);

            var propertyData = from property in properties
                               let value = property.ValueAccessor(item)
                               select new PropertyData
                               {
                                   PropertyName = property.FieldName,
                                   Value = value
                               };

            var converted = ConvertPropertyValues(propertyData);

            return converted.ToList().AsReadOnly();
        }

        private IEnumerable<PropertyData> ConvertPropertyValues(IEnumerable<PropertyData> properties)
        {
            foreach (var property in properties)
            {
                if (!property.Value.IsDefaultValue())
                {
                    if (!_typeStore.TryGetTypeConverter(property.Value.GetType(), out var converter))
                    {
                        throw new Exception($"Converter not found for {property.Value.GetType()}");
                    }

                    if (!converter.TryConvertFrom(property.Value, out var newValue))
                    {
                        throw new Exception($"{converter.GetType()} could not convert {property.Value ?? "<null>"} for {property.PropertyName}");
                    }

                    yield return new PropertyData(property.PropertyName, newValue);
                }
                else
                {
                    yield return new PropertyData(property.PropertyName, null);
                }
            }
        }
    }
}