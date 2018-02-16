using System;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
    public static class ContactProperties
    {
        public static readonly IProperty LastModifiedDate = new Property("lastmodifieddate");
        public static readonly IProperty AssociatedCompanyId = new Property("associatedcompanyid");
        public static readonly IProperty CreateDate = new Property("createdate");

        public static readonly IUpdateableProperty FirstName = new Property("firstname");
        public static readonly IUpdateableProperty LastName = new Property("lastname");
        public static readonly IUpdateableProperty Email = new Property("email");
    }
}