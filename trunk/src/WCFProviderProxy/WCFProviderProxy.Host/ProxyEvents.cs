using System;
using WCFProviderProxy;

namespace WCFProviderProxy.Server
{
    public partial class ProxyMembershipProvider
    {
        // Called at the beginning of each method.
        public static event ProxyEventHandler Debug;

        // Called after initialization and after any data is potentially changed.
        public static event ProxyEventHandler Log;

        // Called for all captured errors.
        // If no handlers and error is not recoverable, error may be re-thrown.
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

        // The OnError event will return a boolean value indicating if their 
        // was an attached delegate. This is to allow suppression of recoverable 
        // errors if they are handled externally. This should be modified to
        // allow the delegate to pass back a boolean value of whether the 
        // error should be suppressed to allow greater consumer control.
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

        // The OnError event will return a boolean value indicating if their 
        // was an attached delegate. This is to allow suppression of recoverable 
        // errors if they are handled externally. This should be modified to
        // allow the delegate to pass back a boolean value of whether the 
        // error should be suppressed to allow greater consumer control.
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

        // The OnError event will return a boolean value indicating if their 
        // was an attached delegate. This is to allow suppression of recoverable 
        // errors if they are handled externally. This should be modified to
        // allow the delegate to pass back a boolean value of whether the 
        // error should be suppressed to allow greater consumer control.
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
