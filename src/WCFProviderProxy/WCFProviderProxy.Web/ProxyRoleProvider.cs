using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Security;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Web
{
    class ProxyRoleProvider : RoleProvider
    {
        private string RemoteProviderName = "";
        private IWcfRoleProvider RemoteProvider()
        {
            ChannelFactory<IWcfRoleProvider> factory = new ChannelFactory<IWcfRoleProvider>("RemoteRoleProvider");
            IWcfRoleProvider provider = factory.CreateChannel();
            provider.SetProvider(RemoteProviderName);
            return provider;
        }

        private void DisposeRemoteProvider(IWcfRoleProvider RemoteProvider)
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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            remoteProvider.AddUsersToRoles(usernames, roleNames);
            DisposeRemoteProvider(remoteProvider);
            return;
        }

        public override void CreateRole(string roleName)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            remoteProvider.CreateRole(roleName);
            DisposeRemoteProvider(remoteProvider);
            return;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.DeleteRole(roleName, throwOnPopulatedRole);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            string[] output = remoteProvider.FindUsersInRole(roleName, usernameToMatch);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string[] GetAllRoles()
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            string[] output = remoteProvider.GetAllRoles();
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string[] GetRolesForUser(string username)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            string[] output = remoteProvider.GetRolesForUser(username);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            string[] output = remoteProvider.GetUsersInRole(roleName);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.IsUserInRole(username, roleName);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            remoteProvider.RemoveUsersFromRoles(usernames, roleNames);
            DisposeRemoteProvider(remoteProvider);
            return;
        }

        public override bool RoleExists(string roleName)
        {
            IWcfRoleProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.RoleExists(roleName);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }
    }
}
