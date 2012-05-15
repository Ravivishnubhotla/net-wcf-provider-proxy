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
    public class ProxyRoleProvider : RoleProvider, IWcfRoleProvider
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyRoleProvider));

        private RoleProvider InternalProvider = Roles.Provider;
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
                InternalProvider = Roles.Provider;
            }
            else
            {
                InternalProvider = Roles.Providers[ProviderName];
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
                InternalProvider = Roles.Providers[config["proxyProviderName"]];
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            InternalProvider.AddUsersToRoles(usernames, roleNames);
            return;
        }

        public override void CreateRole(string roleName)
        {
            InternalProvider.CreateRole(roleName);
            return;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return InternalProvider.DeleteRole(roleName, throwOnPopulatedRole);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return InternalProvider.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string[] GetAllRoles()
        {
            return InternalProvider.GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            return InternalProvider.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return InternalProvider.GetUsersInRole(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return InternalProvider.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            InternalProvider.RemoveUsersFromRoles(usernames, roleNames);
            return;
        }

        public override bool RoleExists(string roleName)
        {
            return InternalProvider.RoleExists(roleName);
        }
    }
}
