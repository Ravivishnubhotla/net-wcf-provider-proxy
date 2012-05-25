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
    public partial class ProxyProfileProvider : ProfileProvider
    {
        private static readonly string name = typeof(ProxyProfileProvider).Name;
        private static readonly ChannelFactory<IWcfProfileProvider> factory = new ChannelFactory<IWcfProfileProvider>("ProfileRoleProvider");

        private string RemoteProviderName = "";
        private IWcfProfileProvider RemoteProvider()
        {
            OnDebug(this, name + ".RemoteProvider()");

            IWcfProfileProvider provider = factory.CreateChannel();
            provider.SetProvider(RemoteProviderName);
            return provider;
        }

        private void DisposeRemoteProvider(IWcfProfileProvider RemoteProvider)
        {
            OnDebug(this, name + ".DisposeRemoteProvider()");

            ((IClientChannel)RemoteProvider).Dispose();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            OnDebug(this, name + ".Initialize()");

            try
            {
                if (config == null)
                    throw new ArgumentNullException("config");

                if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
                {
                    RemoteProviderName = config["proxyProviderName"];
                    OnDebug(this, name + ": RemoteProviderName = '" + RemoteProviderName + "'.");
                }

                // Initialize the abstract base class.
                base.Initialize(name, config);

                OnLog(this, name + ": Initialized.");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }
            }

        }

        public override string ApplicationName { get; set; }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            OnDebug(this, name + ".DeleteInactiveProfiles()");

            int output = 0;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles inactive since " + userInactiveSinceDate.ToString("u") + ".");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = 0;
            }

            return output;
        }

        public override int DeleteProfiles(string[] usernames)
        {
            OnDebug(this, name + ".DeleteProfiles()");

            int output = 0;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteProfiles(usernames);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles (" + string.Join(",", usernames) + ").");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = 0;
            }

            return output;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            OnDebug(this, name + ".DeleteProfiles()");

            int output = 0;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                List<ProfileInfo> profileList = new List<ProfileInfo>();

                foreach (ProfileInfo profile in profiles)
                {
                    profileList.Add(profile);
                }
                
                output = remoteProvider.DeleteProfiles(profileList);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles.");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = 0;
            }

            return output;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindInactiveProfilesByUserName()");

            ProfileInfoCollection output = null;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                
                foreach (ProfileInfo profileInfo in remoteProvider.ListInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }

                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindProfilesByUserName()");

            ProfileInfoCollection output = null;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();

                foreach (ProfileInfo profileInfo in remoteProvider.ListProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }

                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".GetAllInactiveProfiles()");

            ProfileInfoCollection output = null;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();

                foreach (ProfileInfo profileInfo in remoteProvider.ListAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }

                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".GetAllProfiles()");

            ProfileInfoCollection output = null;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();

                foreach (ProfileInfo profileInfo in remoteProvider.ListAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }

                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            OnDebug(this, name + ".GetNumberOfInactiveProfiles()");

            int output = 0;

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetNumberOfInactiveProfiles(authenticationOption, userInactiveSinceDate);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = 0;
            }

            return output;
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            OnDebug(this, name + ".GetPropertyValues()");

            SettingsPropertyValueCollection output = new SettingsPropertyValueCollection();

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                List<SettingsProperty> properties = new List<SettingsProperty>();

                foreach (SettingsProperty property in collection)
                {
                    properties.Add(property);
                }


                foreach (SettingsPropertyValue propertyValue in remoteProvider.GetPropertyValues(context, properties))
                {
                    output.Add(propertyValue);
                }
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output.Clear();
            }

            return output;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            OnDebug(this, name + ".SetPropertyValues()");

            try
            {
                IWcfProfileProvider remoteProvider = RemoteProvider();
                List<SettingsPropertyValue> propertyValues = new List<SettingsPropertyValue>();

                foreach (SettingsPropertyValue propertyValue in collection)
                {
                    propertyValues.Add(propertyValue);
                }

                remoteProvider.SetPropertyValues(context, propertyValues);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Set property values.");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }
            }

            return;
        }
    }
}
