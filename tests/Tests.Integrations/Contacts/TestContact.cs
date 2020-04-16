using System.Collections.Generic;
using HubSpot;

namespace Tests.Contacts
{
    public class TestContact : HubSpot.Contacts.Contact
    {
        [CustomProperty("customProperty")]
        public string CustomProperty { get; set; }

        [CustomProperty("stringListProperty")]
        public List<string> StringListProperty { get; set; }

        [CustomProperty("stringArrayProperty")]
        public string[] StringArrayProperty { get; set; }

        [CustomProperty("stringIListProperty")]
        public IList<string> StringIListProperty { get; set; }

        [CustomProperty("stringIEnumerableProperty")]
        public IEnumerable<string> StringIEnumerableProperty { get; set; }

        [CustomProperty("stringIReadOnlyListProperty")]
        public IReadOnlyList<string> StringIReadOnlyListProperty { get; set; }

    }
}