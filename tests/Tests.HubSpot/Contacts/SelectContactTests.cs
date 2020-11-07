using System;
using AutoFixture.NUnit3;
using HubSpot.Contacts;
using HubSpot.Contacts.Selectors;
using NUnit.Framework;

namespace Tests.Contacts
{
    [TestFixture]
    public class SelectContactTests
    {
        [Test, CustomAutoData]
        public void ByEmail_returns_proper_selector(string email)
        {
            Assert.That(SelectContact.ByEmail(email), Is.InstanceOf<EmailContactSelector>());
        }

        [Test, CustomAutoData]
        public void ByEmail_requires_non_null_email()
        {
            Assert.Throws<ArgumentNullException>(() => SelectContact.ByEmail(null));
        }

        [Test, CustomAutoData]
        public void ByUserToken_returns_proper_selector(string userToken)
        {
            Assert.That(SelectContact.ByUserToken(userToken), Is.InstanceOf<UserTokenContactSelector>());
        }

        [Test, CustomAutoData]
        public void ByUserToken_requires_non_null_email()
        {
            Assert.Throws<ArgumentNullException>(() => SelectContact.ByUserToken(null));
        }

        [Test, CustomAutoData]
        public void ById_returns_proper_selector(long contactId)
        {
            Assert.That(SelectContact.ById(contactId), Is.InstanceOf<IdContactSelector>());
        }
    }
}
