using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Contacts.Properties
{
    public interface IHubSpotContactPropertyClient
    {
        Task<IReadOnlyList<ContactProperty>> GetAllAsync();

        Task<ContactProperty> GetByNameAsync(string propertyName);

        Task<ContactProperty> CreateAsync(ContactProperty propertyToCreate);

        Task<ContactProperty> UpdateAsync(string propertyName, ContactProperty updatedProperty);

        Task DeleteAsync(string propertyName);
    }
}