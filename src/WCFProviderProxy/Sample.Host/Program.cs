using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            WCFProviderProxy.Host.ProxyMembershipProvider.OpenServiceHost();
            WCFProviderProxy.Host.ProxyRoleProvider.OpenServiceHost();
            WCFProviderProxy.Host.ProxyProfileProvider.OpenServiceHost();

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();

            WCFProviderProxy.Host.ProxyMembershipProvider.CloseServiceHost();
            WCFProviderProxy.Host.ProxyRoleProvider.CloseServiceHost();
            WCFProviderProxy.Host.ProxyProfileProvider.CloseServiceHost();
        }
    }
}
