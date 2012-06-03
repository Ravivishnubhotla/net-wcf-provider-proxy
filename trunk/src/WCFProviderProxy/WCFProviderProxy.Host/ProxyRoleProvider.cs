using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.Configuration;
using System.Web.Security;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Server
{
    public partial class ProxyRoleProvider : RoleProvider, IWcfRoleProvider
    {
        private static readonly string name = typeof(ProxyRoleProvider).Name;
        private RoleProvider InternalProvider = Roles.Provider;
        private string description = "";

        public void SetProvider(string ProviderName)
        {
            OnDebug(this, name + ".SetProvider()");

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

                OnDebug(this, name + ": InternalProvider = " + InternalProvider.Name);
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

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            OnDebug(this, name + ".AddUsersToRoles()");

            try
            {
                InternalProvider.AddUsersToRoles(usernames, roleNames);
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
                InternalProvider.CreateRole(roleName);
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
                output = InternalProvider.DeleteRole(roleName, throwOnPopulatedRole);
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

            string[] output = null;

            try
            {
                output = InternalProvider.FindUsersInRole(roleName, usernameToMatch);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetAllRoles()
        {
            OnDebug(this, name + ".GetAllRoles()");

            string[] output = null;

            try
            {
                output = InternalProvider.GetAllRoles();
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetRolesForUser(string username)
        {
            OnDebug(this, name + ".GetRolesForUser()");

            string[] output = null;

            try
            {
                output = InternalProvider.GetRolesForUser(username);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = new string[] { };
            }

            return output;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            OnDebug(this, name + ".GetUsersInRole()");

            string[] output = null;

            try
            {
                output = InternalProvider.GetUsersInRole(roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
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
                output = InternalProvider.IsUserInRole(username, roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            OnDebug(this, name + ".RemoveUsersFromRoles()");

            try
            {
                InternalProvider.RemoveUsersFromRoles(usernames, roleNames);
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
                output = InternalProvider.RoleExists(roleName);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }
    }
}
