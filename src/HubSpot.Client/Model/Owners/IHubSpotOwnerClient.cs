using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Owners
{
    public interface IHubSpotOwnerClient
    {
        Task<IReadOnlyList<Owner>> GetManyAsync(string email = null);
    }
}
