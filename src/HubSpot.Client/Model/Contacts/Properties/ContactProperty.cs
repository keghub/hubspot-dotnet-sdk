using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts.Properties
{
    public class ContactPropertyGroup
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("hubspotDefined")]
        public bool IsDefinedByHubSpot { get; set; }
    }

    public class ContactProperty
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("type")]
        public PropertyType PropertyType { get; set; }

        [JsonProperty("fieldType")]
        public FieldType FieldType { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("formField")]
        public bool IsFormField { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("readOnlyValue")]
        public bool IsValueReadOnly { get; set; }

        [JsonProperty("readOnlyDefinition")]
        public bool IsDefinitionReadOnly { get; set; }

        [JsonProperty("hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("mutableDefinitionNotDeletable")]
        public bool IsDefinitionMutableNotDeletable { get; set; }

        [JsonProperty("calculated")]
        public bool IsCalculated { get; set; }

        [JsonProperty("externalOptions")]
        public bool HasExternalOptions { get; set; }

        [JsonProperty("options")]
        public IReadOnlyList<PropertyOption> Options { get; set; }
    }

    public enum PropertyType
    {
        [EnumMember(Value = "string")] String,
        [EnumMember(Value = "number")] Number,
        [EnumMember(Value = "date")] Date,
        [EnumMember(Value = "datetime")] DateTime,
        [EnumMember(Value = "enumeration")] Enumeration
    }

    public enum FieldType
    {
        [EnumMember(Value = "textarea")] TextArea,
        [EnumMember(Value = "text")] Text,
        [EnumMember(Value = "date")] Date,
        [EnumMember(Value = "file")] File,
        [EnumMember(Value = "number")] Number,
        [EnumMember(Value = "select")] Select,
        [EnumMember(Value = "radio")] Radio,
        [EnumMember(Value = "checkbox")] CheckBox,

        [EnumMember(Value = "booleancheckbox")]
        BooleanCheckBox
    }

    public class PropertyOption
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}