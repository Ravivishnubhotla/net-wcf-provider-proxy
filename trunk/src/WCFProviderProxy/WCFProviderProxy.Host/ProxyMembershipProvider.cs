using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using WCFProviderProxy.Interfaces;

namespace WCFProviderProxy.Server
{
    public partial class ProxyMembershipProvider : MembershipProvider, IWcfMembershipProvider
    {
        private static readonly string name = typeof(ProxyMembershipProvider).Name;
        private MembershipProvider InternalProvider = Membership.Provider;
        private string description = "";

        public void SetProvider(string ProviderName)
        {
            OnDebug(this, name + ".SetProvider()");

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

                OnDebug(this, name + ": InternalProvider = " + InternalProvider.Name);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                InternalProvider = Membership.Provider;
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            OnDebug(this, name + ".ChangePassword()");

            bool output = false;

            try
            {
                output = InternalProvider.ChangePassword(username, oldPassword, newPassword);
                OnSecurityAudit(this, name + ".ChangePassword(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Changed password for user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }
        
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            OnDebug(this, name + ".ChangePasswordQuestionAndAnswer()");

            bool output = false;

            try
            {
                output = InternalProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
                OnSecurityAudit(this, name + ".ChangePasswordQuestionAndAnswer(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Changed password question and answer for user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            OnDebug(this, name + ".CreateUser()");

            MembershipUser output = null;

            try
            {
                output = InternalProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
                OnSecurityAudit(this, name + ".CreateUser(" + username + ", " + email + ").", (output != null));

                if (output != null)
                {
                    OnLog(this, name + ": Created user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                status = MembershipCreateStatus.ProviderError;
            }

            return output;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            OnDebug(this, name + ".DeleteUser()");

            bool output = false;

            try
            {
                output = InternalProvider.DeleteUser(username, deleteAllRelatedData);
                OnSecurityAudit(this, name + ".DeleteUser(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Deleted user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }

        public override bool EnablePasswordReset
        {
            get
            {
                OnDebug(this, name + ".EnablePasswordReset");

                bool output = false;

                try
                {
                    output = InternalProvider.EnablePasswordReset;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = false;
                }

                return output;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                OnDebug(this, name + ".EnablePasswordRetrieval");

                bool output = false;

                try
                {
                    output = InternalProvider.EnablePasswordRetrieval;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = false;
                }

                return output;
            }
        }

        public List<MembershipUser> ListUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListUsersByEmail()");

            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                OnSecurityAudit(this, name + ".ListUsersByEmail(" + emailToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindUsersByEmail()");

            MembershipUserCollection output = null;
            try
            {
                output = InternalProvider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
                OnSecurityAudit(this, name + ".FindUsersByEmail(" + emailToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<MembershipUser> ListUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListUsersByName()");

            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                OnSecurityAudit(this, name + ".ListUsersByName(" + usernameToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindUsersByName()");

            MembershipUserCollection output = null;
         
            try
            {
                output = InternalProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
                OnSecurityAudit(this, name + ".FindUsersByName(" + usernameToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public List<MembershipUser> ListAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".ListAllUsers()");

            List<MembershipUser> output = new List<MembershipUser>();

            try
            {
                foreach (MembershipUser user in InternalProvider.GetAllUsers(pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                OnSecurityAudit(this, name + ".ListAllUsers().", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".GetAllUsers()");

            MembershipUserCollection output = null;
         
            try
            {
                output = InternalProvider.GetAllUsers(pageIndex, pageSize, out totalRecords);
                OnSecurityAudit(this, name + ".GetAllUsers().", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
                totalRecords = 0;
            }

            return output;
        }

        public override int GetNumberOfUsersOnline()
        {
            OnDebug(this, name + ".GetNumberOfUsersOnline()");

            int output = 0;

            try
            {
                output = InternalProvider.GetNumberOfUsersOnline();
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = 0;
            }

            return output;
        }

        public override string GetPassword(string username, string answer)
        {
            OnDebug(this, name + ".GetPassword()");

            string output = null;

            try
            {
                output = InternalProvider.GetPassword(username, answer);
                OnSecurityAudit(this, name + ".GetPassword(" + username + ").", !String.IsNullOrEmpty(output));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
            }

            return output;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            OnDebug(this, name + ".GetUser()");

            MembershipUser output = null;

            try
            {
                output = InternalProvider.GetUser(username, userIsOnline);
                OnSecurityAudit(this, name + ".GetUser(" + username + ").", (output != null));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
            }

            return output;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            OnDebug(this, name + ".GertUser()");

            MembershipUser output = null;

            try
            {
                output = InternalProvider.GetUser(providerUserKey, userIsOnline);
                OnSecurityAudit(this, name + ".GetUser(" + providerUserKey.ToString() + ").", (output != null));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = null;
            }

            return output;
        }

        public override string GetUserNameByEmail(string email)
        {
            OnDebug(this, name + ".GetUserNameByEmail()");

            string output = null;

            try
            {
                output = InternalProvider.GetUserNameByEmail(email);
                OnSecurityAudit(this, name + ".GetUserNameByEmail(" + email + ").", !String.IsNullOrEmpty(output));
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return output;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                OnDebug(this, name + ".MaxInvalidPasswordAttempts");

                int output = 0;

                try
                {
                    output = InternalProvider.MaxInvalidPasswordAttempts;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = 0;
                }

                return output;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                OnDebug(this, name + ".MinRequiredNonAlphanumericCharacters");

                int output = 0;

                try
                {
                    output = InternalProvider.MinRequiredNonAlphanumericCharacters;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = 0;
                }

                return output;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                OnDebug(this, name + ".MinRequiredPasswordLength");

                int output = 0;

                try
                {
                    output = InternalProvider.MinRequiredPasswordLength;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = 0;
                }

                return output;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                OnDebug(this, name + ".PasswordAttemptWindow");

                int output = 0;

                try
                {
                    output = InternalProvider.PasswordAttemptWindow;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = 0;
                }

                return output;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                OnDebug(this, name + ".PasswordFormat");

                MembershipPasswordFormat output = MembershipPasswordFormat.Clear;

                try
                {
                    output = InternalProvider.PasswordFormat;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = MembershipPasswordFormat.Clear;
                }

                return output;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                OnDebug(this, name + ".PasswordStrengthRegularExpression");

                string output = null;

                try
                {
                    output = InternalProvider.PasswordStrengthRegularExpression;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                }

                return output;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                OnDebug(this, name + ".RequiresQuestionAndAnswer");

                bool output = false;

                try
                {
                    output = InternalProvider.RequiresQuestionAndAnswer;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = false;
                }

                return output;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                OnDebug(this, name + ".RequiresUniqueEmail");

                bool output = false;

                try
                {
                    output = InternalProvider.RequiresUniqueEmail;
                }
                catch (Exception ex)
                {
                    OnError(this, ex);
                    output = false;
                }

                return output;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            OnDebug(this, name + ".ResetPassword()");

            string output = null;

            try
            {
                output = InternalProvider.ResetPassword(username, answer);
                OnSecurityAudit(this, name + ".ResetPassword(" + username + ").", !String.IsNullOrEmpty(output));
                
                if (!String.IsNullOrEmpty(output))
                {
                    OnLog(this, name + ": Reset password for user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }

            return output;
        }

        public override bool UnlockUser(string userName)
        {
            OnDebug(this, name + ".UnlockUser()");

            bool output = false;

            try
            {
                output = InternalProvider.UnlockUser(userName);
                OnSecurityAudit(this, name + ".UnlockUser(" + userName + ").", output);
                
                if (output)
                {
                    OnLog(this, name + ": Unlocked user '" + userName + "'.");
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex);
                output = false;
            }

            return output;
        }

        public override void UpdateUser(MembershipUser user)
        {
            OnDebug(this, name + ".UpdateUser()");

            try
            {
                InternalProvider.UpdateUser(user);
                OnLog(this, name + ": Updated user '" + user.UserName + "'.");
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            OnDebug(this, name + ".ValidateUser()");

            bool output = false;

            try
            {
                output = InternalProvider.ValidateUser(username, password);
                OnSecurityAudit(this, name + ".ValidateUser(" + username + ")", output);
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
