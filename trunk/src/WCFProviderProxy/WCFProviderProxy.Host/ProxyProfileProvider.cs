using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Profile;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Host
{
    public partial class ProxyProfileProvider: ProfileProvider, IWcfProfileProvider
    {
        private static readonly string name = typeof(ProxyProfileProvider).Name;
        private ProfileProvider InternalProvider = ProfileManager.Provider;
        private string description = "";

        public void SetProvider(string ProviderName)
        {
            OnDebug(this, name + ".SetProvider()");

            try
            {
                if (String.IsNullOrWhiteSpace(ProviderName))
                {
                    InternalProvider = ProfileManager.Provider;
                }
                else
                {
                    InternalProvider = ProfileManager.Providers[ProviderName];
                }

                OnDebug(this, name + ": InternalProvider = " + InternalProvider.Name);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                InternalProvider = ProfileManager.Provider;
            }
        }

        public override string ApplicationName { get; set; }
        public override string Description { get { return description; } }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            OnDebug(this, name + ".Initialize()");

            try
            {
                if (config == null)
                    throw new ArgumentNullException("config");

                if (!String.IsNullOrWhiteSpace(config["applicationName"]))
                {
                    ApplicationName = config["applicationName"];
                    OnDebug(this, name + ": ApplicationName = " + ApplicationName);
                }

                if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
                {
                    SetProvider(config["proxyProviderName"]);
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

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            OnDebug(this, name + ".DeleteInactiveProfiles()");

            int output = 0;

            try
            {
                output = InternalProvider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles inactive since " + userInactiveSinceDate.ToString("u") + ".");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
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
                output = InternalProvider.DeleteProfiles(usernames);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles (" + string.Join(",", usernames) + ").");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = 0;
            }

            return output;
        }

        public int DeleteProfiles(List<ProfileInfo> profiles)
        {
            OnDebug(this, name + ".DeleteProfiles()");

            int output = 0;

            try
            {
                ProfileInfoCollection profileCollection = new ProfileInfoCollection();

                foreach (ProfileInfo profile in profiles)
                {
                    profileCollection.Add(profile);
                }

                output = InternalProvider.DeleteProfiles(profileCollection);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles.");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
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
                output = InternalProvider.DeleteProfiles(profiles);
                OnLog(this, name + ": Deleted " + output.ToString() + " profiles.");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = 0;
            }

            return output;
        }

        public List<ProfileInfo> ListInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListInactiveProfilesByUserName()");

            List<ProfileInfo> output = new List<ProfileInfo>(); ;

            try
            {
                foreach (ProfileInfo profileInfo in InternalProvider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindInactiveProfilesByUserName()");

            ProfileInfoCollection output = null;

            try
            {
                output = InternalProvider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<ProfileInfo> ListProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListProfilesByUserName()");

            List<ProfileInfo> output = new List<ProfileInfo>();

            try
            {
                foreach (ProfileInfo profileInfo in InternalProvider.FindProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
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
                output = InternalProvider.FindProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<ProfileInfo> ListAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListAllInactiveProfiles()");

            List<ProfileInfo> output = new List<ProfileInfo>();

            try
            {
                foreach (ProfileInfo profileInfo in InternalProvider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
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
                output = InternalProvider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public  List<ProfileInfo> ListAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListAllProfiles()");

            List<ProfileInfo> output = new List<ProfileInfo>();

            try
            {
                foreach (ProfileInfo profileInfo in InternalProvider.GetAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(profileInfo);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
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
                output = InternalProvider.GetAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
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
                output = InternalProvider.GetNumberOfInactiveProfiles(authenticationOption, userInactiveSinceDate);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = 0;
            }

            return output;
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            OnDebug(this, name + ".GetPropertyValues()");

            SettingsPropertyValueCollection output = null;

            try
            {
                output = InternalProvider.GetPropertyValues(context, collection);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
            }

            return output;
        }

        public List<SettingsPropertyValue> GetPropertyValues(SettingsContext context, List<WcfSettingsProperty> collection)
        {
            OnDebug(this, name + ".GetPropertyValues()");

            List<SettingsPropertyValue> output = new List<SettingsPropertyValue>();

            try
            {
                SettingsPropertyCollection propertyCollection = new SettingsPropertyCollection();

                foreach (WcfSettingsProperty property in collection)
                {
                    propertyCollection.Add(property.ToSettingsProperty());
                }

                foreach (SettingsPropertyValue propertyValue in InternalProvider.GetPropertyValues(context, propertyCollection))
                {
                    output.Add(propertyValue);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
            }

            return output;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            OnDebug(this, name + ".SetPropertyValues()");

            try
            {
                InternalProvider.SetPropertyValues(context, collection);
                OnLog(this, name + ": Set property values.");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
            
            return;
        }

        public void SetPropertyValues(SettingsContext context, List<SettingsPropertyValue> collection)
        {
            OnDebug(this, name + ".SetPropertyValues()");

            try
            {
                SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();

                foreach (SettingsPropertyValue propertyValue in collection)
                {
                    propertyValueCollection.Add(propertyValue);
                }

                InternalProvider.SetPropertyValues(context, propertyValueCollection);
                OnLog(this, name + ": Set property values.");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return;
        }
    }
}
