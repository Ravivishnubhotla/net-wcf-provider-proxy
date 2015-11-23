The following is a sample web.config for implementing the built-in ASP.Net SQL Membership, Profile, and Role providers.

```xml

<configuration>

<system.web>
<!--
The normal applicationName parameter will be provided on
the host-side application configuration file and does
not need to be provided here unless accessing programmatically.

The proxyProviderName parameter will match the provider
name you wish to consume from the proxy host.
-->

<membership defaultProvider="WCFProxyMembershipProvider">

<providers>

<add name="WCFProxyMembershipProvider"
type="WCFProviderProxy.Client.ProxyMembershipProvider"
proxyProviderName="AspNetSqlMembershipProvider" />



Unknown end tag for &lt;/providers&gt;





Unknown end tag for &lt;/membership&gt;



<profile defaultProvider="WCFProxyProfileProvider" enabled="true">

<providers>

<add name="WCFProxyProfileProvider"
type="WCFProviderProxy.Client.ProxyProfileProvider"
proxyProviderName="AspNetSqlProfileProvider" />



Unknown end tag for &lt;/providers&gt;



<properties>

<!--
The profile properties must either be provided
here in the client, or through the profile inherits
parameter for using typed profile properties.

These do not need to be duplicated on the host-side
application configuration file.
-->



Unknown end tag for &lt;/properties&gt;





Unknown end tag for &lt;/profile&gt;



<roleManager defaultProvider="WCFProxyRoleProvider" enabled="true">

<providers>

<add name="WCFProxyRoleProvider"
type="WCFProviderProxy.Client.ProxyRoleProvider"
proxyProviderName="AspNetSqlRoleProvider" />



Unknown end tag for &lt;/providers&gt;





Unknown end tag for &lt;/roleManager&gt;



</system.web>

<!--
These are the client WCF endpoints necessary to
connect to the host proxy. The name and contract
parameters need to be as specified.

Only those providers being implemented must be
declared.

The address and binding must match the implementation
provided by your host application configuration.
-->

<system.serviceModel>

<client>

<endpoint
name="RemoteMembershipProvider"
address="net.tcp://localhost:9101/MembershipProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfMembershipProvider" />

<endpoint
name="RemoteRoleProvider"
address="net.tcp://localhost:9101/RoleProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfRoleProvider" />

<endpoint
name="RemoteProfileProvider"
address="net.tcp://localhost:9101/ProfileProvider"
binding="netTcpBinding"
contract="WCFProviderProxy.Interfaces.IWcfProfileProvider" />



Unknown end tag for &lt;/client&gt;



</system.serviceModel>



Unknown end tag for &lt;/configuration&gt;



```