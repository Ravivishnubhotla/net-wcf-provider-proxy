using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.Configuration;
using System.Web.Profile;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Host
{
    public class ProxyProfileProvider: ProfileProvider, IWcfProfileProvider 
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyProfileProvider));

        private ProfileProvider InternalProvider = ProfileManager.Provider;
        private string description = "";

        public static void OpenServiceHost()
        {
            serviceHost.Open();
            return;
        }

        public static void CloseServiceHost()
        {
            serviceHost.Close();
            return;
        }


        public void SetMembershipProvider(string ProviderName)
        {
            if (String.IsNullOrWhiteSpace(ProviderName))
            {
                InternalProvider = ProfileManager.Provider;
            }
            else
            {
                InternalProvider = ProfileManager.Providers[ProviderName];
            }
        }

        public override string ApplicationName { get; set; }
        public override string Description { get { return description; } }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (!String.IsNullOrWhiteSpace(config["applicationName"]))
            {
                ApplicationName = config["applicationName"];
            }

            if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
            {
                InternalProvider = ProfileManager.Providers[config["proxyProviderName"]];
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            return InternalProvider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
        }

        public override int DeleteProfiles(string[] usernames)
        {
            return InternalProvider.DeleteProfiles(usernames);
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            return InternalProvider.DeleteProfiles(profiles);
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.FindProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.GetAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            return InternalProvider.GetNumberOfInactiveProfiles(authenticationOption, userInactiveSinceDate);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            return InternalProvider.GetPropertyValues(context, collection);
        }

        public List<SettingsPropertyValue> GetPropertyValues(SettingsContext context, List<SettingsProperty> collection)
        {
            SettingsPropertyCollection propertyCollection = new SettingsPropertyCollection();

            foreach (SettingsProperty property in collection)
            {
                propertyCollection.Add(property);
            }

            List<SettingsPropertyValue> propertyValueCollection = new List<SettingsPropertyValue>();

            foreach(SettingsPropertyValue propertyValue in InternalProvider.GetPropertyValues(context, propertyCollection))
            {
                propertyValueCollection.Add(propertyValue);
            }

            return propertyValueCollection;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            InternalProvider.SetPropertyValues(context, collection);
            return;
        }

        public void SetPropertyValues(SettingsContext context, List<SettingsPropertyValue> collection)
        {
            SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();

            foreach (SettingsPropertyValue propertyValue in collection)
            {
                propertyValueCollection.Add(propertyValue);
            }

            InternalProvider.SetPropertyValues(context, propertyValueCollection);
            return;
        }
    }
}
