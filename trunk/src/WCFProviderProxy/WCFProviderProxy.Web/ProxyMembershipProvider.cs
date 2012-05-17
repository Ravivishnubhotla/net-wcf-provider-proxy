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
    public class ProxyMembershipProvider : MembershipProvider
    {
        private string RemoteProviderName = "";
        private IWcfMembershipProvider RemoteProvider()
        {
            ChannelFactory<IWcfMembershipProvider> factory = new ChannelFactory<IWcfMembershipProvider>("RemoteMembershipProvider");
            IWcfMembershipProvider provider = factory.CreateChannel();
            provider.SetMembershipProvider(RemoteProviderName);
            return provider;
        }

        private void DisposeRemoteProvider(IWcfMembershipProvider RemoteProvider)
        {
            ((IClientChannel)RemoteProvider).Dispose();
        }

        public override void Initialize(string name, NameValueCollection config)
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

        public override string ApplicationName { get; set; }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.ChangePassword(username, oldPassword, newPassword);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUser output = remoteProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.DeleteUser(username, deleteAllRelatedData);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override bool EnablePasswordReset
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                bool output = remoteProvider.EnablePasswordReset;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                bool output = remoteProvider.EnablePasswordRetrieval;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUserCollection output = new MembershipUserCollection();

            foreach (MembershipUser user in remoteProvider.ListUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords))
            {
                output.Add(user);
            }

            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUserCollection output = new MembershipUserCollection();

            foreach (MembershipUser user in remoteProvider.ListUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords))
            {
                output.Add(user);
            }

            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUserCollection output = new MembershipUserCollection();

            foreach (MembershipUser user in remoteProvider.ListAllUsers(pageIndex, pageSize, out totalRecords))
            {
                output.Add(user);
            }

            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override int GetNumberOfUsersOnline()
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            int output = remoteProvider.GetNumberOfUsersOnline();
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string GetPassword(string username, string answer)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            string output = remoteProvider.GetPassword(username, answer);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUser output = remoteProvider.GetUser(username, userIsOnline);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            MembershipUser output = remoteProvider.GetUser(providerUserKey, userIsOnline);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override string GetUserNameByEmail(string email)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            string output = remoteProvider.GetUserNameByEmail(email);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                int output = remoteProvider.MaxInvalidPasswordAttempts;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                int output = remoteProvider.MinRequiredNonAlphanumericCharacters;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                int output = remoteProvider.MinRequiredPasswordLength;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                int output = remoteProvider.PasswordAttemptWindow;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                MembershipPasswordFormat output = remoteProvider.PasswordFormat;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                string output = remoteProvider.PasswordStrengthRegularExpression;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                bool output = remoteProvider.RequiresQuestionAndAnswer;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                IWcfMembershipProvider remoteProvider = RemoteProvider();
                bool output = remoteProvider.RequiresUniqueEmail;
                DisposeRemoteProvider(remoteProvider);
                return output;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            string output = remoteProvider.ResetPassword(username, answer);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override bool UnlockUser(string userName)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.UnlockUser(userName);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }

        public override void UpdateUser(MembershipUser user)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            remoteProvider.UpdateUser(user);
            DisposeRemoteProvider(remoteProvider);
        }

        public override bool ValidateUser(string username, string password)
        {
            IWcfMembershipProvider remoteProvider = RemoteProvider();
            bool output = remoteProvider.ValidateUser(username, password);
            DisposeRemoteProvider(remoteProvider);
            return output;
        }
    }
}
