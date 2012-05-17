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
    public partial class ProxyMembershipProvider : MembershipProvider, IWcfMembershipProvider 
    {
        private static readonly MembershipSection membershipSection = new MembershipSection();
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyMembershipProvider));
        
        private MembershipProvider InternalProvider = Membership.Provider;
        private string description = "";

        public static void OpenServiceHost()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                OnError(typeof(ProxyMembershipProvider), ex);
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
                OnError(typeof(ProxyMembershipProvider), ex);
            }
        }

        public void SetProvider(string ProviderName)
        {
            try
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
            catch (Exception ex)
            {
                OnError(this, ex);
                InternalProvider = Membership.Provider;
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool output = false;

            try
            {
                output = InternalProvider.ChangePassword(username, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }
        
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            bool output = false;

            try
            {
                output = InternalProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser output = null;

            try
            {
                output = InternalProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
                status = MembershipCreateStatus.ProviderError;
            }

            return output;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            bool output = false;

            try
            {
                output = InternalProvider.DeleteUser(username, deleteAllRelatedData);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }

        public override bool EnablePasswordReset
        {
            get
            {
                bool output = false;

                try
                {
                    output = InternalProvider.EnablePasswordReset;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = false;
                }

                return output;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                bool output = false;

                try
                {
                    output = InternalProvider.EnablePasswordRetrieval;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = false;
                }

                return output;
            }
        }

        public List<MembershipUser> ListUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection output = null;
            try
            {
                output = InternalProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<MembershipUser> ListUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection output = null;
         
            try
            {
                output = InternalProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<MembershipUser> ListAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.GetAllUsers(pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection output = null;
         
            try
            {
                output = InternalProvider.GetAllUsers(pageIndex, pageSize, out totalRecords);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override int GetNumberOfUsersOnline()
        {
            int output = 0;

            try
            {
                output = InternalProvider.GetNumberOfUsersOnline();
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = 0;
            }

            return output;
        }

        public override string GetPassword(string username, string answer)
        {
            string output = null;

            try
            {
                output = InternalProvider.GetPassword(username, answer);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
            }

            return output;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser output = null;

            try
            {
                output = InternalProvider.GetUser(username, userIsOnline);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
            }

            return output;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser output = null;

            try
            {
                output = InternalProvider.GetUser(providerUserKey, userIsOnline);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = null;
            }

            return output;
        }

        public override string GetUserNameByEmail(string email)
        {
            string output = null;

            try
            {
                output = InternalProvider.GetUserNameByEmail(email);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
            }

            return output;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                int output = 0;

                try
                {
                    output = InternalProvider.MaxInvalidPasswordAttempts;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = 0;
                }

                return output;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                int output = 0;

                try
                {
                    output = InternalProvider.MinRequiredNonAlphanumericCharacters;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = 0;
                }

                return output;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                int output = 0;

                try
                {
                    output = InternalProvider.MinRequiredPasswordLength;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = 0;
                }

                return output;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                int output = 0;

                try
                {
                    output = InternalProvider.PasswordAttemptWindow;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = 0;
                }

                return output;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                MembershipPasswordFormat output = MembershipPasswordFormat.Clear;

                try
                {
                    output = InternalProvider.PasswordFormat;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = MembershipPasswordFormat.Clear;
                }

                return output;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                string output = null;

                try
                {
                    output = InternalProvider.PasswordStrengthRegularExpression;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                }

                return output;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                bool output = false;

                try
                {
                    output = InternalProvider.RequiresQuestionAndAnswer;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = false;
                }

                return output;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                bool output = false;

                try
                {
                    output = InternalProvider.RequiresUniqueEmail;
                }
                catch (Exception ex)
                {
                    OnError(this, ex, false);
                    output = false;
                }

                return output;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            string output = null;

            try
            {
                output = InternalProvider.ResetPassword(username, answer);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
            }

            return output;
        }

        public override bool UnlockUser(string userName)
        {
            bool output = false;

            try
            {
                output = InternalProvider.UnlockUser(userName);
            }
            catch (Exception ex)
            {
                OnError(this, ex, false);
                output = false;
            }

            return output;
        }

        public override void UpdateUser(MembershipUser user)
        {
            try
            {
                InternalProvider.UpdateUser(user);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            bool output = false;

            try
            {
                output = InternalProvider.ValidateUser(username, password);
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
