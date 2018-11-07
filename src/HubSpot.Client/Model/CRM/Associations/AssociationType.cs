using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubSpot.Model.CRM.Associations
{
    [JsonConverter(typeof(AssociationTypeConverter))]
    public class AssociationType
    {
        public AssociationType(int associationTypeId)
        {
            Id = associationTypeId;
        }

        public int Id { get; }

        public static implicit operator AssociationType(int associationTypeId) => new AssociationType(associationTypeId);
    }

    public static class AssociationTypes
    {
        public static readonly AssociationType ContactToCompany = 1;
        public static readonly AssociationType CompanyToContact = 2;
        public static readonly AssociationType DealToContact = 3;
        public static readonly AssociationType ContactToDeal = 4;
        public static readonly AssociationType DealToCompany = 5;
        public static readonly AssociationType CompanyToDeal = 6;
        public static readonly AssociationType CompanyToEngagement = 7;
        public static readonly AssociationType EngagementToCompany = 8;
        public static readonly AssociationType ContactToEngagement = 9;
        public static readonly AssociationType EngagementToContact = 10;
        public static readonly AssociationType DealToEngagement = 11;
        public static readonly AssociationType EngagementToDeal = 12;
        public static readonly AssociationType ParentCompanyToChild = 13;
        public static readonly AssociationType ChildCompanyToParent = 14;
        public static readonly AssociationType ContactToTicket = 15;
        public static readonly AssociationType TicketToContact = 16;
        public static readonly AssociationType TicketToEngagement = 17;
        public static readonly AssociationType EngagementToTicket = 18;
        public static readonly AssociationType DealToLineItem = 19;
        public static readonly AssociationType LineItemToDeal = 20;
        public static readonly AssociationType CompanyToTicket = 25;
        public static readonly AssociationType TicketToCompany = 26;
        public static readonly AssociationType DealToTicket = 27;
        public static readonly AssociationType TicketToDeal = 28;
    }

    public static class CompanyAssociationTypes
    {
        public static readonly AssociationType AdvisorToCompany = 33;
        public static readonly AssociationType CompanyToAdvisor = 34;
        public static readonly AssociationType BoardMemberToCompany = 35;
        public static readonly AssociationType CompanyToBoardMember = 36;
        public static readonly AssociationType ContractorToCompany = 37;
        public static readonly AssociationType CompanyToContractor = 38;
        public static readonly AssociationType ManagerToCompany = 39;
        public static readonly AssociationType CompanyToManager = 40;
        public static readonly AssociationType BusinessOwnerToCompany = 41;
        public static readonly AssociationType CompanyToBusinessOwner = 42;
        public static readonly AssociationType PartnerToCompany = 43;
        public static readonly AssociationType CompanyToPartner = 44;
        public static readonly AssociationType ResellerToCompany = 45;
        public static readonly AssociationType CompanyToReseller = 46;

    }

    public class AssociationTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((AssociationType)value).Id);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var id = serializer.Deserialize<int?>(reader);

            if (id == null)
            {
                return null;
            }

            return new AssociationType(id.Value);
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(AssociationType);
    }
}