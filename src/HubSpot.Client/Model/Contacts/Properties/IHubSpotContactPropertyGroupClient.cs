using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Contacts.Properties {
    public interface IHubSpotContactPropertyGroupClient
    {
        Task<IReadOnlyList<ContactPropertyGroup>> GetAllAsync();

        Task<ContactPropertyGroup> CreateAsync(ContactPropertyGroup group);

        Task<ContactPropertyGroup> UpdateAsync(string groupName, ContactPropertyGroup group);

        Task DeleteAsync(string groupName);
    }
}