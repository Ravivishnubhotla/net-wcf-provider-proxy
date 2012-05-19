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
        private static readonly ChannelFactory<IWcfMembershipProvider> factory = new ChannelFactory<IWcfMembershipProvider>("RemoteMembershipProvider");

        private string RemoteProviderName = "";
        private IWcfMembershipProvider RemoteProvider()
        {
            IWcfMembershipProvider provider = factory.CreateChannel();
            provider.SetProvider(RemoteProviderName);

            return provider;
        }

        private void DisposeRemoteProvider(IWcfMembershipProvider RemoteProvider)
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ChangePassword(username, oldPassword, newPassword);
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

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
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

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
                DisposeRemoteProvider(remoteProvider);
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
            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.DeleteUser(username, deleteAllRelatedData);
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

        public override bool EnablePasswordReset
        {
            get
            {
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
            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
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
            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
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
            MembershipUserCollection output = new MembershipUserCollection();

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();

                foreach (MembershipUser user in remoteProvider.ListAllUsers(pageIndex, pageSize, out totalRecords))
                {
                    output.Add(user);
                }

                DisposeRemoteProvider(remoteProvider);
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
            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetPassword(username, answer);
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

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUser(username, userIsOnline);
                DisposeRemoteProvider(remoteProvider);
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
            MembershipUser output = null;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUser(providerUserKey, userIsOnline);
                DisposeRemoteProvider(remoteProvider);
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
            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.GetUserNameByEmail(email);
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

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
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
            string output = "";

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ResetPassword(username, answer);
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

        public override bool UnlockUser(string userName)
        {
            bool output = false;

            try{
            IWcfMembershipProvider remoteProvider = RemoteProvider();
             output = remoteProvider.UnlockUser(userName);
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

        public override void UpdateUser(MembershipUser user)
        {
            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                remoteProvider.UpdateUser(user);
                DisposeRemoteProvider(remoteProvider);
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
            bool output = false;

            try
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                output = remoteProvider.ValidateUser(username, password);
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
