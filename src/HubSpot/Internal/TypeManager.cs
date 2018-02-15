using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HubSpot.Internal
{
    public abstract class TypeManager<THubSpot, TEntity> : ITypeManager<THubSpot, TEntity>
        where TEntity : HubSpotEntity<THubSpot>
    {
        private readonly ITypeStore _typeStore;

        protected TypeManager(ITypeStore typeStore)
        {
            _typeStore = typeStore ?? throw new ArgumentNullException(nameof(typeStore));
        }

        public Task<T> TransformAsync<T>(THubSpot item)
            where T : TEntity
        {
            var constructor = _typeStore.GetConstructor<THubSpot, T>();

            var entity = constructor.Invoke(new object[] { item }) as T;

            SetCustomProperties(item, entity);

            return Task.FromResult(entity);
        }

        private void SetDefaultProperties<T>(THubSpot item, T entity)
            where T : TEntity { }

        private void SetCustomProperties<T>(THubSpot item, T entity)
            where T : TEntity
        {
            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            var hubspotProperties = GetCustomProperties(item);

            var zipped = from ep in customProperties
                         from hp in hubspotProperties
                         let propertyName = GetPropertyName(ep)
                         where string.Equals(propertyName, hp, StringComparison.OrdinalIgnoreCase)
                         where HasCustomProperty(item, hp)
                         select new
                         {
                             propertyName = hp,
                             property = ep,
                             value = GetCustomPropertyValue(item, hp),
                             type = ep.PropertyType
                         };

            foreach (var zip in zipped)
            {
                if (_typeStore.TryGetTypeConverter(zip.type, out var converter))
                {
                    var convertedValue = converter.Convert(zip.value);
                    zip.property.SetValue(entity, convertedValue);
                }
            }
        }

        private string GetPropertyName(PropertyInfo property) => property.GetCustomAttribute<CustomPropertyAttribute>().PropertyName;

        public IReadOnlyList<string> GetCustomProperties<T>()
            where T : TEntity
        {
            var customProperties = _typeStore.GetCustomProperties<THubSpot, T>();

            return customProperties.Select(p => p.Name).ToArray();
        }

        protected abstract IReadOnlyList<string> GetCustomProperties(THubSpot item);

        protected abstract bool HasCustomProperty(THubSpot item, string propertyName);

        protected abstract string GetCustomPropertyValue(THubSpot item, string propertyName);
    }
}