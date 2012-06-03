using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WCFProviderProxy.Interfaces
{
    /// <summary>
    /// The service contract for communicating between the
    /// client and host profile provider proxy over WCF.
    /// </summary>
    /// <remarks>
    /// These were left as close as possible to the actual 
    /// RoleProvider abstract class signature.
    /// </remarks>
    [ServiceContract]
    public interface IWcfRoleProvider
    {
        string ApplicationName { [OperationContract] get; [OperationContract] set; }

        [OperationContract]
        void SetProvider(string ProviderName);

        [OperationContract]
        void AddUsersToRoles(string[] usernames, string[] roleNames);

        [OperationContract]
        void CreateRole(string roleName);

        [OperationContract]
        bool DeleteRole(string roleName, bool throwOnPopulatedRole);

        [OperationContract]
        string[] FindUsersInRole(string roleName, string usernameToMatch);

        [OperationContract]
        string[] GetAllRoles();

        [OperationContract]
        string[] GetRolesForUser(string username);

        [OperationContract]
        string[] GetUsersInRole(string roleName);

        [OperationContract]
        bool IsUserInRole(string username, string roleName);

        [OperationContract]
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);

        [OperationContract]
        bool RoleExists(string roleName);
    }
}
