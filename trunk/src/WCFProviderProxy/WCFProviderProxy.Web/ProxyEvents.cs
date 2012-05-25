using System;

namespace WCFProviderProxy.Web
{
    public class ProxyEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    public class ProxySecurityEventArgs : EventArgs
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public delegate void ProxyEventHandler(object sender, ProxyEventArgs e);
    public delegate void ProxySecurityEventHandler(object sender, ProxySecurityEventArgs e);

    public partial class ProxyMembershipProvider
    {
        // Called at the beginning of each method.
        public static event ProxyEventHandler Debug;

        // Called after initialization and after any data is potentially changed.
        public static event ProxyEventHandler Log;

        // Called for all captured errors.
        // If no handlers and error is not recoverable, error will be re-thrown.
        public static event ProxyEventHandler Error;

        // Called for all potential security events along with success flag.
        // For successful events, may be duplicate Log event.
        public static event ProxySecurityEventHandler SecurityAudit;

        protected static void OnDebug(object sender, string message)
        {
            if (Debug != null)
            {
                Debug(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static void OnLog(object sender, string message)
        {
            if (Log != null)
            {
                Log(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ProxyEventArgs() { Exception = ex });
                output = true;
            }

            return output;
        }

        protected static void OnSecurityAudit(object sender, string message, bool success)
        {
            if (SecurityAudit != null)
            {
                SecurityAudit(sender, new ProxySecurityEventArgs() { Message = message, Success = success });
            }
        }
    }

    public partial class ProxyProfileProvider
    {
        // Called at the beginning of each method.
        public static event ProxyEventHandler Debug;

        // Called after initialization and after any data is potentially changed.
        public static event ProxyEventHandler Log;

        // Called for all captured errors.
        // If no handlers and error is not recoverable, error will be re-thrown.
        public static event ProxyEventHandler Error;

        protected static void OnDebug(object sender, string message)
        {
            if (Debug != null)
            {
                Debug(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static void OnLog(object sender, string message)
        {
            if (Log != null)
            {
                Log(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ProxyEventArgs() { Exception = ex });
                output = true;
            }

            return output;
        }
    }

    public partial class ProxyRoleProvider
    {
        // Called at the beginning of each method.
        public static event ProxyEventHandler Debug;

        // Called after initialization and after any data is potentially changed.
        public static event ProxyEventHandler Log;

        // Called for all captured errors.
        // If no handlers and error is not recoverable, error will be re-thrown.
        public static event ProxyEventHandler Error;

        protected static void OnDebug(object sender, string message)
        {
            if (Debug != null)
            {
                Debug(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static void OnLog(object sender, string message)
        {
            if (Log != null)
            {
                Log(sender, new ProxyEventArgs() { Message = message });
            }
        }

        protected static bool OnError(object sender, Exception ex)
        {
            bool output = false;

            if (Error != null)
            {
                Error(sender, new ProxyEventArgs() { Exception = ex });
                output = true;
            }

            return output;
        }
    }
}
