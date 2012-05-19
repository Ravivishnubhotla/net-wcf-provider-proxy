using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFProviderProxy.Web
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
        public static event EventHandler Error;

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
                output = true;
            }

            return output;
        }
    }

    public partial class ProxyProfileProvider
    {
        public static event EventHandler Error;

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
                output = true;
            }

            return output;
        }
    }

    public partial class ProxyRoleProvider
    {
        public static event EventHandler Error;

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ErrorEventArgs(ex));
                output = true;
            }

            return output;
        }
    }
}
