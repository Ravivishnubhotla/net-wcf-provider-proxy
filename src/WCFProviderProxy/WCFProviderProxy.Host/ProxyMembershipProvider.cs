using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Host
{
    public class ProxyMembershipProvider : MembershipProvider, IWcfMembershipProvider 
    {
        private static readonly MembershipSection membershipSection = new MembershipSection();
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyMembershipProvider));
        
        private MembershipProvider InternalProvider = Membership.Provider;
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
                InternalProvider = Membership.Provider;
            }
            else
            {
                InternalProvider = Membership.Providers[ProviderName];
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
                ApplicationName =config["applicationName"];
            }

            if (!String.IsNullOrWhiteSpace(config["proxyProviderName"]))
            {
                InternalProvider = Membership.Providers[config["proxyProviderName"]];
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return InternalProvider.ChangePassword(username, oldPassword, newPassword);
        }
        
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return InternalProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return InternalProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return InternalProvider.DeleteUser(username, deleteAllRelatedData);
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return InternalProvider.EnablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return InternalProvider.EnablePasswordRetrieval;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return InternalProvider.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfUsersOnline()
        {
            return InternalProvider.GetNumberOfUsersOnline();
        }

        public override string GetPassword(string username, string answer)
        {
            return InternalProvider.GetPassword(username, answer);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return InternalProvider.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return InternalProvider.GetUser(providerUserKey, userIsOnline);
        }

        public override string GetUserNameByEmail(string email)
        {
            return InternalProvider.GetUserNameByEmail(email);
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return InternalProvider.MaxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return InternalProvider.MinRequiredNonAlphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return InternalProvider.MinRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return InternalProvider.PasswordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return InternalProvider.PasswordFormat;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return InternalProvider.PasswordStrengthRegularExpression;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return InternalProvider.RequiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return InternalProvider.RequiresUniqueEmail;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            return InternalProvider.ResetPassword(username, answer);
        }

        public override bool UnlockUser(string userName)
        {
            return InternalProvider.UnlockUser(userName);
        }

        public override void UpdateUser(MembershipUser user)
        {
            InternalProvider.UpdateUser(user);
        }

        public override bool ValidateUser(string username, string password)
        {
            return InternalProvider.ValidateUser(username, password);
        }
    }
}
