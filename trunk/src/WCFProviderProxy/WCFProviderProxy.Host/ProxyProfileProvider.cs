﻿using System;
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
        private ProfileProvider InternalProvider = ProfileManager.Provider;
        private string description = "";

        public void SetProvider(string ProviderName)
        {
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
            try
            {
                if (config == null)
                    throw new ArgumentNullException("config");

                if (!String.IsNullOrWhiteSpace(config["applicationName"]))
                {
                    ApplicationName = config["applicationName"];
                }

                if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
                {
                    SetProvider(config["proxyProviderName"]);
                }

                // Initialize the abstract base class.
                base.Initialize(name, config);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            int output = 0;

            try
            {
                output = InternalProvider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
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
            int output = 0;

            try
            {
                output = InternalProvider.DeleteProfiles(usernames);
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
            int output = 0;

            try
            {
                ProfileInfoCollection profileCollection = new ProfileInfoCollection();

                foreach (ProfileInfo profile in profiles)
                {
                    profileCollection.Add(profile);
                }

                output = InternalProvider.DeleteProfiles(profileCollection);
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
            int output = 0;

            try
            {
                output = InternalProvider.DeleteProfiles(profiles);
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

        public List<SettingsPropertyValue> GetPropertyValues(SettingsContext context, List<SettingsProperty> collection)
        {
            List<SettingsPropertyValue> output = new List<SettingsPropertyValue>();

            try
            {
                SettingsPropertyCollection propertyCollection = new SettingsPropertyCollection();

                foreach (SettingsProperty property in collection)
                {
                    propertyCollection.Add(property);
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
            try
            {
                InternalProvider.SetPropertyValues(context, collection);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
            
            return;
        }

        public void SetPropertyValues(SettingsContext context, List<SettingsPropertyValue> collection)
        {
            try
            {
                SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();

                foreach (SettingsPropertyValue propertyValue in collection)
                {
                    propertyValueCollection.Add(propertyValue);
                }

                InternalProvider.SetPropertyValues(context, propertyValueCollection);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return;
        }
    }
}
