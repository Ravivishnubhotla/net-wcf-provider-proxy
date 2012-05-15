using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Profile;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Web
{
    class ProxyProfileProvider : ProfileProvider
    {
        private string RemoteProviderName = "";
        private IWcfProfileProvider RemoteProvider()
        {
            ChannelFactory<IWcfProfileProvider> factory = new ChannelFactory<IWcfProfileProvider>("ProfileRoleProvider");
            IWcfProfileProvider provider = factory.CreateChannel();
            provider.SetMembershipProvider(RemoteProviderName);
            return provider;
        }

        private void DisposeRemoteProvider(IWcfProfileProvider RemoteProvider)
        {
            ((IClientChannel)RemoteProvider).Dispose();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
            {
                RemoteProviderName = config["proxyProviderName"];
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override string ApplicationName { get; set; }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            int output = remoteProvider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override int DeleteProfiles(string[] usernames)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            int output = remoteProvider.DeleteProfiles(usernames);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            int output = remoteProvider.DeleteProfiles(profiles);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            ProfileInfoCollection output = remoteProvider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            ProfileInfoCollection output = remoteProvider.FindProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords);
            DisposeRemoteProvider(remoteProvider);
            return output;
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            ProfileInfoCollection output = remoteProvider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            ProfileInfoCollection output = remoteProvider.GetAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            int output = remoteProvider.GetNumberOfInactiveProfiles(authenticationOption, userInactiveSinceDate);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            List<SettingsProperty> properties = new List<SettingsProperty>();

            foreach (SettingsProperty property in collection)
            {
                properties.Add(property);
            }

            SettingsPropertyValueCollection propertyValues = new SettingsPropertyValueCollection();

            foreach (SettingsPropertyValue propertyValue in remoteProvider.GetPropertyValues(context, properties))
            {
                propertyValues.Add(propertyValue);
            }

            return propertyValues;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            IWcfProfileProvider remoteProvider = RemoteProvider();
            List<SettingsPropertyValue> propertyValues = new List<SettingsPropertyValue>();

            foreach (SettingsPropertyValue propertyValue in collection)
            {
                propertyValues.Add(propertyValue);
            }

            remoteProvider.SetPropertyValues(context, propertyValues);
            DisposeRemoteProvider(remoteProvider);

            return;
        }
    }
}
