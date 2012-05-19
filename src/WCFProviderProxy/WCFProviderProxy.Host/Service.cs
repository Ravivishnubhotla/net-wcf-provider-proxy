using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WCFProviderProxy.Host
{
    class Service
    {
        static void Main()
        {
            MembershipService membershipService = new MembershipService();
            ProfileService profileService = new ProfileService();
            RoleService roleService = new RoleService();

            if (Environment.UserInteractive)
            {
                ProxyMembershipProvider.Error += new EventHandler(ProxyMembershipProvider_Error);
                ProxyProfileProvider.Error += new EventHandler(ProxyProfileProvider_Error);
                ProxyRoleProvider.Error += new EventHandler(ProxyRoleProvider_Error);

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
                ServiceBase.Run(new ServiceBase[] { membershipService, profileService, roleService });
            }
        }

        static void ProxyRoleProvider_Error(object sender, EventArgs e)
        {
            Console.WriteLine(((ErrorEventArgs)e).Error.Message);
        }

        static void ProxyProfileProvider_Error(object sender, EventArgs e)
        {
            Console.WriteLine(((ErrorEventArgs)e).Error.Message);
        }

        static void ProxyMembershipProvider_Error(object sender, EventArgs e)
        {
            Console.WriteLine(((ErrorEventArgs)e).Error.Message);
        }
    }

    public class MembershipService : ServiceBase
    {
        public void StartService() { OnStart(null); }
        public void StopService() { OnStop(); }
        
        protected override void OnStart(string[] args)
        {
            ProxyMembershipProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyMembershipProvider.CloseServiceHost();
        }
    }

    public class ProfileService : ServiceBase
    {
        public void StartService() { OnStart(null); }
        public void StopService() { OnStop(); }

        protected override void OnStart(string[] args)
        {
            ProxyProfileProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyProfileProvider.CloseServiceHost();
        }
    }

    public class RoleService : ServiceBase
    {
        public void StartService() { OnStart(null); }
        public void StopService() { OnStop(); }

        protected override void OnStart(string[] args)
        {
            ProxyRoleProvider.OpenServiceHost();
        }
        protected override void OnStop()
        {
            ProxyRoleProvider.CloseServiceHost();
        }
    }


#if INSTALLER
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller processInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public ProjectInstaller()
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
#endif
}
