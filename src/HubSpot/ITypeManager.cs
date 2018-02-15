using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot
{
    public interface ITypeManager<in THubSpot, in TEntity>
        where TEntity : HubSpotEntity<THubSpot>
    {
        Task<T> TransformAsync<T>(THubSpot item)
            where T : TEntity;

        IReadOnlyList<string> GetCustomProperties<T>()
            where T : TEntity;
    }
}