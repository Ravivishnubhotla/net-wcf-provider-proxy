using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using WCFProviderProxy;

namespace WCFProviderProxy.Server
{
    class Service
    {
        static readonly string eventSource = new ProxyService().ServiceName;
        static readonly string eventLog = "Application";

        static void Main()
        {
            MembershipService membershipService = new MembershipService();
            ProfileService profileService = new ProfileService();
            RoleService roleService = new RoleService();

            if (Environment.UserInteractive)
            {
                // If running in console mode, attach event handlers to 
                // display any errors to the console.
                ProxyMembershipProvider.SecurityAudit += new ProxySecurityEventHandler(ProxyProvider_ConsoleAudit);

                ProxyMembershipProvider.Debug += new ProxyEventHandler(ProxyProvider_ConsoleInfo);
                ProxyProfileProvider.Debug += new ProxyEventHandler(ProxyProvider_ConsoleInfo);
                ProxyRoleProvider.Debug += new ProxyEventHandler(ProxyProvider_ConsoleInfo);

                ProxyMembershipProvider.Log += new ProxyEventHandler(ProxyProvider_ConsoleInfo);
                ProxyProfileProvider.Log += new ProxyEventHandler(ProxyProvider_ConsoleInfo);
                ProxyRoleProvider.Log += new ProxyEventHandler(ProxyProvider_ConsoleInfo);

                ProxyMembershipProvider.Error += new ProxyEventHandler(ProxyProvider_ConsoleError);
                ProxyProfileProvider.Error += new ProxyEventHandler(ProxyProvider_ConsoleError);
                ProxyRoleProvider.Error += new ProxyEventHandler(ProxyProvider_ConsoleError);

                membershipService.StartService();
                profileService.StartService();
                roleService.StartService();

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();

                membershipService.StopService();
                profileService.StopService();
                roleService.StopService();
            }
            else
            {
                // If running as a windows service, attach event handlers
                // to log all events to the windows application log.
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, eventLog);
                }

                ProxyMembershipProvider.SecurityAudit += new ProxySecurityEventHandler(ProxyProvider_ServiceAudit);

                ProxyMembershipProvider.Log += new ProxyEventHandler(ProxyProvider_ServiceInfo);
                ProxyProfileProvider.Log += new ProxyEventHandler(ProxyProvider_ServiceInfo);
                ProxyRoleProvider.Log += new ProxyEventHandler(ProxyProvider_ServiceInfo);

                ProxyMembershipProvider.Error += new ProxyEventHandler(ProxyProvider_ServiceError);
                ProxyProfileProvider.Error += new ProxyEventHandler(ProxyProvider_ServiceError);
                ProxyRoleProvider.Error += new ProxyEventHandler(ProxyProvider_ServiceError);

                EventLog.WriteEntry(eventSource, "Starting Services", EventLogEntryType.Information);
                ServiceBase.Run(membershipService);
                ServiceBase.Run(profileService);
                ServiceBase.Run(roleService);
                EventLog.WriteEntry(eventSource, "Services Started.", EventLogEntryType.Information);
            }
        }

        static void ProxyProvider_ConsoleError(object sender, ProxyEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        static void ProxyProvider_ConsoleInfo(object sender, ProxyEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void ProxyProvider_ConsoleAudit(object sender, ProxySecurityEventArgs e)
        {
            if (e.Success)
            {
                Console.WriteLine("SUCCESS: " + e.Message);
            }
            else
            {
                Console.WriteLine("FAILED: " + e.Message);
            }
        }

        static void ProxyProvider_ServiceError(object sender, ProxyEventArgs e)
        {
            EventLog.WriteEntry(eventSource, e.Exception.Message, EventLogEntryType.Error);
        }

        static void ProxyProvider_ServiceInfo(object sender, ProxyEventArgs e)
        {
            EventLog.WriteEntry(eventSource, e.Message, EventLogEntryType.Information);
        }

        static void ProxyProvider_ServiceAudit(object sender, ProxySecurityEventArgs e)
        {
            if (e.Success)
            {
                EventLog.WriteEntry(eventSource, e.Message, EventLogEntryType.SuccessAudit);
            }
            else
            {
                EventLog.WriteEntry(eventSource, e.Message, EventLogEntryType.FailureAudit);
            }
        }
    }

    /// <summary>
    /// The abstract class extends the base ServiceBase
    /// to add methods allowing the service start and stop 
    /// methods to be called externally in a non-windows 
    /// service hosted code.
    /// </summary>
    abstract class ProxyService : ServiceBase
    {
        public void StartService() { this.OnStart(null); }
        public void StopService() { this.OnStop(); }
    }

    public class MembershipService : ProxyService
    {
        
        protected override void OnStart(string[] args)
        {
            ProxyMembershipProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyMembershipProvider.CloseServiceHost();
        }
    }

    public class ProfileService : ProxyService
    {
        protected override void OnStart(string[] args)
        {
            ProxyProfileProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyProfileProvider.CloseServiceHost();
        }
    }

    public class RoleService : ProxyService
    {
        protected override void OnStart(string[] args)
        {
            ProxyRoleProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyRoleProvider.CloseServiceHost();
        }
    }

    [RunInstaller(true)]
    public class ServiceInstaller : Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller processInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public ServiceInstaller()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            processInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.NetworkService
            };

            serviceInstaller = new System.ServiceProcess.ServiceInstaller()
            {
                DisplayName = "WCF Provider Proxy",
                ServiceName = "WCFProviderProxy",
                Description = "WCF Provider Host Proxy",
                StartType = ServiceStartMode.Automatic
            };

            Installers.AddRange(new Installer[] { processInstaller, serviceInstaller});
        }
    }
}
