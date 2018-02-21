using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HubSpot.Internal;
using HubSpot.Model.Contacts;
using HubSpotDeal = HubSpot.Model.Deals.Deal;

namespace HubSpot.Deals
{
    public interface IDealTypeManager : ITypeManager<HubSpotDeal, Deal>
    {
        IReadOnlyList<(AssociationType type, Operation operation, long id)> GetModifiedAssociations<T>(T item)
            where T : Deal, new();
    }

    public enum Operation
    {
        Added, Removed
    }

    public enum AssociationType
    {
        Company, Deal, Contact
    }

    public class DealTypeManager : TypeManager<HubSpotDeal, Deal>, IDealTypeManager
    {
        private const string AssociatedCompanyIds = "associatedCompanyIds";
        private const string AssociatedContactIds = "associatedVids";
        private const string AssociatedDealIds = "associatedDealIds";

        public DealTypeManager(ITypeStore typeStore) : base(typeStore) { }

        public override T ConvertTo<T>(HubSpotDeal item)
        {
            T entity = new T();

            SetDefaultProperties(item, entity);

            var properties = SetCustomProperties(item, entity);

            ((IHubSpotEntity)entity).Properties = properties.ToDictionary(k => k.PropertyName, i => i.Value);

            ((IHubSpotDealEntity)entity).Associations = new Dictionary<string, IReadOnlyList<long>>
            {
                [AssociatedCompanyIds] = new List<long>(item.Associations.Companies),
                [AssociatedContactIds] = new List<long>(item.Associations.Contacts),
                [AssociatedDealIds] = new List<long>(item.Associations.Deals)
            };

            return entity;
        }

        protected override IReadOnlyList<KeyValuePair<string, string>> GetCustomProperties(HubSpotDeal item)
        {
            var result = new List<KeyValuePair<string, string>>();

            var properties = from kvp in item.Properties
                             let key = kvp.Key
                             let value = kvp.Value.Value
                             select new KeyValuePair<string, string>(key, value);

            result.AddRange(properties);

            return result;
        }

        protected override IReadOnlyList<KeyValuePair<string, object>> GetDefaultProperties(HubSpotDeal item)
        {
            return new[]
            {
                new KeyValuePair<string, object>("portalId", item.PortalId),
                new KeyValuePair<string, object>("dealId", item.Id),
                new KeyValuePair<string, object>("isDeleted", item.IsDeleted)
            };
        }

        protected override bool HasCustomProperty(HubSpotDeal item, string propertyName) => item.Properties.ContainsKey(propertyName);

        public IReadOnlyList<(AssociationType type, Operation operation, long id)> GetModifiedAssociations<T>(T item)
            where T : Deal, new()
        {
            var newAssociations = ((IHubSpotDealEntity)item)?.Associations;

            var result = new List<(AssociationType type, Operation operation, long id)>();

            if (newAssociations != null)
            {
                result.AddRange(AddChangeSet(AssociatedCompanyIds, AssociationType.Company, item.AssociatedCompanyIds));
                result.AddRange(AddChangeSet(AssociatedContactIds, AssociationType.Contact, item.AssociatedContactIds));
                result.AddRange(AddChangeSet(AssociatedDealIds, AssociationType.Deal, item.AssociatedDealIds));
            }

            return result;

            IEnumerable<(AssociationType, Operation, long)> CompareSets(IReadOnlyList<long> newAssociationIds, IReadOnlyList<long> oldAssociationIds, AssociationType type)
            {
                var toBeAdded = newAssociationIds.NotIn(oldAssociationIds);

                foreach (var id in toBeAdded)
                {
                    yield return (type, Operation.Added, id);
                }

                var toBeDeleted = oldAssociationIds.NotIn(newAssociationIds);

                foreach (var id in toBeDeleted)
                {
                    yield return (type, Operation.Removed, id);
                }
            }

            IEnumerable<(AssociationType, Operation, long)> AddChangeSet(string associationKey, AssociationType type, IReadOnlyList<long> oldAssociationIds)
            {
                if (newAssociations.TryGetValue(associationKey, out var ids))
                {
                    return CompareSets(ids, oldAssociationIds, type);
                }

                return Array.Empty<(AssociationType, Operation, long)>();
            }
        }
    }
}