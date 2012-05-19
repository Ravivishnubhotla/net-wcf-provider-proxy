using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WCFProviderProxy.Host
{
    public partial class ProxyMembershipProvider
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyMembershipProvider));

        public static void OpenServiceHost()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                if (!OnError(typeof(ProxyMembershipProvider), ex))
                {
                    throw;
                }
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
                if (!OnError(typeof(ProxyMembershipProvider), ex))
                {
                    throw;
                }
            }
        }
    }

    public partial class ProxyProfileProvider
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyProfileProvider));

        public static void OpenServiceHost()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                if (!OnError(typeof(ProxyProfileProvider), ex))
                {
                    throw;
                }
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
                if (!OnError(typeof(ProxyProfileProvider), ex))
                {
                    throw;
                }
            }
        }
    }

    public partial class ProxyRoleProvider
    {
        private static readonly ServiceHost serviceHost = new ServiceHost(typeof(ProxyRoleProvider));

        public static void OpenServiceHost()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception ex)
            {
                if (!OnError(typeof(ProxyRoleProvider), ex))
                {
                    throw;
                }
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
                if (!OnError(typeof(ProxyRoleProvider), ex))
                {
                    throw;
                }
            }
        }
    }
}
