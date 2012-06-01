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
    public partial class ProxyMembershipProvider : MembershipProvider
    {
        private static readonly string name = typeof(ProxyMembershipProvider).Name;
        private static ChannelFactory<IWcfMembershipProvider> factory = null;

        private string RemoteProviderName = "";
        private IWcfMembershipProvider RemoteProvider()
        {
            OnDebug(this, name + ".RemoteProvider()");

            if (factory == null)
            {
                factory = new ChannelFactory<IWcfMembershipProvider>("RemoteMembershipProvider");
            }

            IWcfMembershipProvider provider = factory.CreateChannel();
            provider.SetProvider(RemoteProviderName);

            return provider;
        }

        private void DisposeRemoteProvider(IWcfMembershipProvider RemoteProvider)
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
                    OnDebug(this, name + ": RemoteProviderName = '" + RemoteProviderName + "'");
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            OnDebug(this, name + ".ChangePassword()");

            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ChangePassword(username, oldPassword, newPassword);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".ChangePassword(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Changed password for user '" + username + "'.");
                }
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

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            OnDebug(this, name + ".ChangePasswordQuestionAndAnswer()");

            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".ChangePasswordQuestionAndAnswer(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Changed password question and answer for user '" + username + "'.");
                }
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

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            OnDebug(this, name + ".CreateUser()");

            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".CreateUser(" + username + ", " + email + ").", (output != null));

                if (output != null)
                {
                    OnLog(this, name + ": Created user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

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
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteUser(username, deleteAllRelatedData);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".DeleteUser(" + username + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Deleted user '" + username + "'.");
                }
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

        public override bool EnablePasswordReset
        {
            get
            {
                OnDebug(this, name + ".EnablePasswordReset");

                bool output = false;

                try
                {
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.EnablePasswordReset;
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

        public override bool EnablePasswordRetrieval
        {
            get
            {
                OnDebug(this, name + ".EnablePasswordRetrieval");

                bool output = false;

                try
                {
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.EnablePasswordRetrieval;
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

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindUsersByEmail()");

            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".FindUsersByEmail(" + emailToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".FindUsersByName()");

            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".FindUsersByName(" + usernameToMatch + ").", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output.Clear();
                totalRecords = 0;
            }

            return output;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            OnDebug(this, name + ".GetAllUsers()");

            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListAllUsers(pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".GetAllUsers().", (totalRecords > 0));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output.Clear();
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
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetNumberOfUsersOnline();
                DisposeRemoteProvider(remoteProvider);
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = 0;
            }

            return output;
        }

        public override string GetPassword(string username, string answer)
        {
            OnDebug(this, name + ".GetPassword()");

            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetPassword(username, answer);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".GetPassword(" + username + ").", !String.IsNullOrEmpty(output));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = "";
            }

            return output;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            OnDebug(this, name + ".GetUser()");

            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUser(username, userIsOnline);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".GetUser(" + username + ").", (output != null));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
            }

            return output;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            OnDebug(this, name + ".GetUser()");

            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUser(providerUserKey, userIsOnline);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".GetUser(" + providerUserKey.ToString() + ").", (output != null));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = null;
            }

            return output;
        }

        public override string GetUserNameByEmail(string email)
        {
            OnDebug(this, name + ".GetUserNameByEmail()");

            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUserNameByEmail(email);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".GetUserNameByEmail(" + email + ").", !String.IsNullOrEmpty(output));
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = "";
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
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.MaxInvalidPasswordAttempts;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

                    output = 0;
                }

                return output;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                OnDebug(this, name + ".MinRequiredAlphaNumericCharacters");

                int output = 0;

                try
                {
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.MinRequiredNonAlphanumericCharacters;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

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
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.MinRequiredPasswordLength;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

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
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.PasswordAttemptWindow;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

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
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.PasswordFormat;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

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

                string output = "";

                try
                {
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.PasswordStrengthRegularExpression;
                    DisposeRemoteProvider(remoteProvider);
                }
                catch (Exception ex)
                {
                    if (!OnError(this, ex))
                    {
                        throw;
                    }

                    output = "";
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
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.RequiresQuestionAndAnswer;
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

        public override bool RequiresUniqueEmail
        {
            get
            {
                OnDebug(this, name + ".RequiresUniqueEmail");

                bool output = false;

                try
                {
                    IWcfMembershipProvider remoteProvider = RemoteProvider();
                    output = remoteProvider.RequiresUniqueEmail;
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

        public override string ResetPassword(string username, string answer)
        {
            OnDebug(this, name + ".ResetPassword()");

            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ResetPassword(username, answer);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".ResetPassword(" + username + ").", !String.IsNullOrEmpty(output));

                if (!String.IsNullOrEmpty(output))
                {
                    OnLog(this, name + ": Reset password for user '" + username + "'.");
                }
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }

                output = "";
            }

            return output;
        }

        public override bool UnlockUser(string userName)
        {
            OnDebug(this, name + ".UnlockUser()");

            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.UnlockUser(userName);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".UnlockUser(" + userName + ").", output);

                if (output)
                {
                    OnLog(this, name + ": Unlocked user '" + userName + "'.");
                }
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

        public override void UpdateUser(MembershipUser user)
        {
            OnDebug(this, name + ".UpdateUser()");

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                remoteProvider.UpdateUser(user);
                DisposeRemoteProvider(remoteProvider);
                OnLog(this, name + ": Updated user '" + user.UserName + "'.");
            }
            catch (Exception ex)
            {
                if (!OnError(this, ex))
                {
                    throw;
                }
            }

        }

        public override bool ValidateUser(string username, string password)
        {
            OnDebug(this, name + ".ValidateUser()");

            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ValidateUser(username, password);
                DisposeRemoteProvider(remoteProvider);
                OnSecurityAudit(this, name + ".ValidateUser(" + username + ").", output);
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
