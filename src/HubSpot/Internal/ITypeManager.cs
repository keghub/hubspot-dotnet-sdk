using System;
using System.Collections.Generic;
using System.Reflection;

namespace HubSpot.Internal
{
    public interface ITypeManager<in THubSpot, in TEntity>
        where TEntity : class, IHubSpotEntity
    {
        T ConvertTo<T>(THubSpot item)
            where T : class, TEntity, new();

        IReadOnlyList<CustomPropertyInfo> GetCustomProperties<T>(Func<CustomPropertyAttribute, bool> filter)
            where T : class, TEntity, new();

        IReadOnlyList<(string name, string value)> GetModifiedProperties<T>(T item)
            where T : class, TEntity, new();
    }

    public class CustomPropertyInfo
    {
        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public Func<object, object> ValueAccessor { get; set; }

        public string FieldName { get; set; }

        public bool IsReadOnly { get; set; }
    }

    public static class TypeManager
    {
        public static readonly Func<CustomPropertyAttribute, bool> AllProperties = p => true;

        public static readonly Func<CustomPropertyAttribute, bool> OnlyReadOnlyProperties = p => p.IsReadOnly;

        public static readonly Func<CustomPropertyAttribute, bool> ModifiableProperties = p => !p.IsReadOnly;
    }
}