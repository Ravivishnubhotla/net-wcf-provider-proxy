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
    public partial class ProxyRoleProvider : RoleProvider
    {
        private static readonly ChannelFactory<IWcfRoleProvider> factory = new ChannelFactory<IWcfRoleProvider>("RemoteRoleProvider");

        private string RemoteProviderName = "";
        private IWcfRoleProvider RemoteProvider()
        {
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
            try
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
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }
            }
        }

        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.AddUsersToRoles(usernames, roleNames);
                DisposeRemoteProvider(remoteProvider);
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

        public override void CreateRole(string roleName)
        {
            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.CreateRole(roleName);
                DisposeRemoteProvider(remoteProvider);
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

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool output = false;

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteRole(roleName, throwOnPopulatedRole);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex) && throwOnPopulatedRole)
                {
                    throw;
                }

                output = false;
            }

            return output;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            string[] output = { };

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.FindUsersInRole(roleName, usernameToMatch);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = new string[] { };
            }

            return output;
        }

        public override string[] GetAllRoles()
        {
            string[] output = { };

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetAllRoles();
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = new string[] { };
            }

            return output;
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] output = { };

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetRolesForUser(username);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = new string[] { };
            }

            return output;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            string[] output = { };

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUsersInRole(roleName);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = new string[] { };
            }

            return output;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool output = false;

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.IsUserInRole(username, roleName);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = false;
            }

            return output;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.RemoveUsersFromRoles(usernames, roleNames);
                DisposeRemoteProvider(remoteProvider);
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

        public override bool RoleExists(string roleName)
        {
            bool output = false;

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.RoleExists(roleName);
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = false;
            }

            return output;
        }
    }
}