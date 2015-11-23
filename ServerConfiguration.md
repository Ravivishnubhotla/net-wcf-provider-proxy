The following is a sample implementation for the server side configuration file
```xml

<configuration>

<!--
The following connectionStrings and system.web configuration
sections should be set up as per your own implementation.

Full documentation for the built in providers can be found
on MSDN. Custom providers are also supported and should
follow appropriate configurations.

Please note: The <profile><properties> section will be configured
in the client configuration file and does not need to be set here.
-->

<connectionStrings>

<add name="ApplicationServices"
connectionString="Data Source=(local);Initial Catalog=ASPNETDB;Integrated Security=True"
providerName="System.Data.SqlClient"/>



Unknown end tag for &lt;/connectionStrings&gt;



<system.web>

<membership>

<providers>

<add name="AspNetSqlMembershipProvider"
type="System.Web.Security.SqlMembershipProvider"
connectionStringName="ApplicationServices"
enablePasswordRetrieval="false"
enablePasswordReset="true"
requiresQuestionAndAnswer="false"
requiresUniqueEmail="false"
maxInvalidPasswordAttempts="5"
minRequiredPasswordLength="6"
minRequiredNonalphanumericCharacters="0"
passwordAttemptWindow="10"
applicationName="/"/>



Unknown end tag for &lt;/providers&gt;





Unknown end tag for &lt;/membership&gt;



<profile enabled="true">

<providers>

<add name="AspNetSqlProfileProvider"
type="System.Web.Profile.SqlProfileProvider"
connectionStringName="ApplicationServices"
applicationName="/"/>



Unknown end tag for &lt;/providers&gt;





Unknown end tag for &lt;/profile&gt;



<roleManager enabled="true">

<providers>

<add name="AspNetSqlRoleProvider"
type="System.Web.Security.SqlRoleProvider"
connectionStringName="ApplicationServices"
applicationName="/"/>



Unknown end tag for &lt;/providers&gt;





Unknown end tag for &lt;/roleManager&gt;



</system.web>

<system.serviceModel>

<services>

<!--
Set the appropriate address and binding for your own implementation.
-->

<service name="WCFProviderProxy.Server.ProxyMembershipProvider">

<endpoint address="net.tcp://localhost:9101/MembershipProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfMembershipProvider"/>



Unknown end tag for &lt;/service&gt;



<!--
Set the appropriate address and binding for your own implementation.
-->

<service name="WCFProviderProxy.Host.ProxyRoleProvider">

<endpoint address="net.tcp://localhost:9101/RoleProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfRoleProvider"/>



Unknown end tag for &lt;/service&gt;



<!--
Set the appropriate address and binding for your own implementation.
-->

<service name="WCFProviderProxy.Host.ProxyProfileProvider">

<endpoint address="net.tcp://localhost:9101/ProfileProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfProfileProvider"/>



Unknown end tag for &lt;/service&gt;





Unknown end tag for &lt;/services&gt;



</system.serviceModel>



Unknown end tag for &lt;/configuration&gt;


```