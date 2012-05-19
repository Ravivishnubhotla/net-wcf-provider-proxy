using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Profile;

namespace WCFProviderProxy.Interfaces
{
    [ServiceContract]
    public interface IWcfProfileProvider
    {
        [OperationContract]
        void SetProvider(string ProviderName);

        string ApplicationName { [OperationContract] get; [OperationContract] set; }

        [OperationContract]
        int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract(Name = "DeleteProfilesByUserName")]
        int DeleteProfiles(string[] usernames);

        [OperationContract(Name = "DeleteProfilesByProfiles")]
        int DeleteProfiles(List<ProfileInfo> profiles);

        [OperationContract]
        List<ProfileInfo> ListInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract]
        List<SettingsPropertyValue> GetPropertyValues(SettingsContext context, List<SettingsProperty> collection);

        [OperationContract]
        void SetPropertyValues(SettingsContext context, List<SettingsPropertyValue> collection);
    }
    
}
