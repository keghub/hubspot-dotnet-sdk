using System;
using System.Collections.Generic;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public static class HttpQueryStringBuilderExtensions
    {
        public static void AddProperties(this HttpQueryStringBuilder builder, IEnumerable<IProperty> properties, string fieldName = "property")
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            foreach (var property in properties ?? Array.Empty<IProperty>())
            {
                builder.Add(fieldName, property.Name);
            }
        }

        public static void AddShowListMemberships(this HttpQueryStringBuilder builder, bool showListMemberships)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Add("showListMemberships", showListMemberships ? "true" : "false");
        }

        public static void AddFormSubmissionMode(this HttpQueryStringBuilder builder, FormSubmissionMode formSubmissionMode)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Add("formSubmissionMode", GetFormSubmissionMode(formSubmissionMode));

            string GetFormSubmissionMode(FormSubmissionMode mode)
            {
                switch (mode)
                {
                    case FormSubmissionMode.All: return "all";
                    case FormSubmissionMode.Newest: return "newest";
                    case FormSubmissionMode.None: return "none";
                    case FormSubmissionMode.Oldest: return "oldest";

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        public static void AddPropertyMode(this HttpQueryStringBuilder builder, PropertyMode propertyMode)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Add("propertyMode", propertyMode == PropertyMode.ValueAndHistory ? "value_and_history" : "value_only");
        }
    }
}