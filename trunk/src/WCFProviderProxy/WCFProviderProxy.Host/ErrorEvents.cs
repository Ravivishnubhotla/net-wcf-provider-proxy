using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFProviderProxy.Host
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception ex)
        {
            Error = ex;
        }

        public Exception Error { get; private set; }
    }

    public partial class ProxyMembershipProvider
    {
        public static  event EventHandler Error;

        protected static void OnError(object sender, Exception ex, bool throwUnhandled = true)
        {
            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
            }
            else if (throwUnhandled)
            {
                throw ex;
            }
        }
    }

    public partial class ProxyProfileProvider
    {
        public static event EventHandler Error;

        protected static void OnError(object sender, Exception ex, bool throwUnhandled = true)
        {
            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
            }
            else if (throwUnhandled)
            {
                throw ex;
            }
        }
    }

    public partial class ProxyRoleProvider
    {
        public static event EventHandler Error;

        protected static void OnError(object sender, Exception ex, bool throwUnhandled = true)
        {
            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
            }
            else if (throwUnhandled)
            {
                throw ex;
            }
        }
    }
}
