using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.Configuration;
using System.Web.Security;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Host
{
    public partial class ProxyRoleProvider : RoleProvider, IWcfRoleProvider
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyRoleProvider));

        private RoleProvider InternalProvider = Roles.Provider;
        private string description = "";

        public static void OpenServiceHost()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                OnError(typeof(ProxyProfileProvider), ex);
            }
        }

        public static void CloseServiceHost()
        {
            try
            {
                if (serviceHost.State == CommunicationState.Opened)
                {
                    serviceHost.Close();
                }
            }
            catch (Exception ex)
            {
                OnError(typeof(ProxyProfileProvider), ex, false);
            }
        }

        public void SetProvider(string ProviderName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(ProviderName))
                {
                    InternalProvider = Roles.Provider;
                }
                else
                {
                    InternalProvider = Roles.Providers[ProviderName];
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                InternalProvider = Roles.Provider;
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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                InternalProvider.AddUsersToRoles(usernames, roleNames);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return;
        }

        public override void CreateRole(string roleName)
        {
            try
            {
                InternalProvider.CreateRole(roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool output = false;

            try
            {
                output = InternalProvider.DeleteRole(roleName, throwOnPopulatedRole);
            }
            catch (Exception ex)
            {
                OnError(this, ex, throwOnPopulatedRole);
                output = false;
            }

            return output;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            string[] output = null;

            try
            {
                output = InternalProvider.FindUsersInRole(roleName, usernameToMatch);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetAllRoles()
        {
            string[] output = null;

            try
            {
                output = InternalProvider.GetAllRoles();
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] output = null;

            try
            {
                output = InternalProvider.GetRolesForUser(username);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            string[] output = null;

            try
            {
                output = InternalProvider.GetUsersInRole(roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = new string[] { };
            }

            return output;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool output = false;

            try
            {
                output = InternalProvider.IsUserInRole(username, roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                InternalProvider.RemoveUsersFromRoles(usernames, roleNames);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
            
            return;
        }

        public override bool RoleExists(string roleName)
        {
            bool output = false;

            try
            {
                output = InternalProvider.RoleExists(roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }
    }
}
