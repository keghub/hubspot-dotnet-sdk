using System;
using System.Collections.Generic;
using System.Reflection;

namespace HubSpot.Internal
{
    public interface ITypeManager<in THubSpot, in TEntity>
        where TEntity : class, IHubSpotEntity
    {
        T ConvertFrom<T>(THubSpot item)
            where T : class, TEntity, new();

        IReadOnlyList<(string name, PropertyInfo property, CustomPropertyAttribute metadata)> GetCustomProperties<T>(Func<CustomPropertyAttribute, bool> filter)
            where T : class, TEntity, new();

        IReadOnlyList<(string name, string value)> GetModifiedProperties<T>(T item)
            where T : class, TEntity, new();
    }

    public static class TypeManager
    {
        public static readonly Func<CustomPropertyAttribute, bool> AllProperties = p => true;

        public static readonly Func<CustomPropertyAttribute, bool> OnlyReadOnlyProperties = p => p.IsReadOnly;

        public static readonly Func<CustomPropertyAttribute, bool> ModifiableProperties = p => !p.IsReadOnly;
    }
}