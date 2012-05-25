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
        private static readonly string name = typeof(ProxyRoleProvider).Name;
        private static readonly ChannelFactory<IWcfRoleProvider> factory = new ChannelFactory<IWcfRoleProvider>("RemoteRoleProvider");

        private string RemoteProviderName = "";
        private IWcfRoleProvider RemoteProvider()
        {
            OnDebug(this, name + ".RemoteProvider()");

            IWcfRoleProvider provider = factory.CreateChannel();
            provider.SetProvider(RemoteProviderName);
            return provider;
        }

        private void DisposeRemoteProvider(IWcfRoleProvider RemoteProvider)
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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            OnDebug(this, name + ".AddUsersToRoles()");

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.AddUsersToRoles(usernames, roleNames);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Added users (" + string.Join(",", usernames) + ") to roles (" + string.Join(",", roleNames) + ").");
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
            OnDebug(this, name + ".CreateRole()");

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.CreateRole(roleName);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Created role '" + roleName + "'");
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
            OnDebug(this, name + ".DeleteRole()");

            bool output = false;

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteRole(roleName, throwOnPopulatedRole);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Deleted role '" + roleName + "'.");
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
            OnDebug(this, name + ".FindUsersInRole()");

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
            OnDebug(this, name + ".GetAllRoles()");

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
            OnDebug(this, name + ".GetRolesForUser()");

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
            OnDebug(this, name + ".GetUsersInRole()");

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
            OnDebug(this, name + ".IsUerInRole()");

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
            OnDebug(this, name + ".RemoveUsersFromRoles()");

            try
            {
                IWcfRoleProvider remoteProvider = RemoteProvider();
                remoteProvider.RemoveUsersFromRoles(usernames, roleNames);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Removed users (" + string.Join(",", usernames) + ") from roles (" + string.Join(",", roleNames) + ").");
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
            OnDebug(this, name + ".RoleExists()");

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