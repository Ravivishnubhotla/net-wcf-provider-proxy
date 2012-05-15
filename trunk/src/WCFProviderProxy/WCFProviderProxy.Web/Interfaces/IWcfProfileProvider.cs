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
        void SetMembershipProvider(string ProviderName);

        string ApplicationName { [OperationContract] get; [OperationContract] set; }

        [OperationContract]
          int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract(Name = "DeleteProfilesByUserName")]
          int DeleteProfiles(string[] usernames);

        [OperationContract(Name = "DeleteProfilesByProfiles")]
          int DeleteProfiles(ProfileInfoCollection profiles);

        [OperationContract]
          ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
          ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
          ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
          ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
          int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract]
        List<SettingsPropertyValue> GetPropertyValues(SettingsContext context, List<SettingsProperty> collection);

        [OperationContract]
        void SetPropertyValues(SettingsContext context, List<SettingsPropertyValue> collection);
    }
    
}
