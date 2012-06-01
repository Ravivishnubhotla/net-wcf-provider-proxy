using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Profile;

namespace WCFProviderProxy.Interfaces
{
    [ServiceContract]
    public interface IWcfProfileProvider
    {
        [OperationContract]
        void SetProvider(string ProviderName);

        string ApplicationName { [OperationContract] get; [OperationContract] set; }

        [OperationContract]
        int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract(Name = "DeleteProfilesByUserName")]
        int DeleteProfiles(string[] usernames);

        [OperationContract(Name = "DeleteProfilesByProfiles")]
        int DeleteProfiles(List<ProfileInfo> profiles);

        [OperationContract]
        List<ProfileInfo> ListInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        List<ProfileInfo> ListAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate);

        [OperationContract]
        List<WcfSettingsPropertyValue> GetPropertyValues(SettingsContext context, List<WcfSettingsProperty> collection);

        [OperationContract]
        void SetPropertyValues(SettingsContext context, List<WcfSettingsPropertyValue> collection);
    }

    [DataContract]
    public class WcfSettingsProperty
    {
        public WcfSettingsProperty() { }

        public WcfSettingsProperty(SettingsProperty property)
        {
            Attributes = property.Attributes;
            DefaultValue = property.DefaultValue;
            IsReadOnly = property.IsReadOnly;
            Name = property.Name;
            PropertyTypeName = property.PropertyType.FullName;
            //ProviderTypeName = property.Provider.GetType().FullName;
            SerializeAs = property.SerializeAs;
            ThrowOnErrorDeserializing = property.ThrowOnErrorDeserializing;
            ThrowOnErrorSerializing = property.ThrowOnErrorSerializing;
        }

        [DataMember]
        public SettingsAttributeDictionary Attributes { get; set; }

        [DataMember]
        public object DefaultValue { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PropertyTypeName { get; set; }

        //[DataMember]
        //public string ProviderTypeName { get; set; }

        [DataMember]
        public SettingsSerializeAs SerializeAs { get; set; }

        [DataMember]
        public bool ThrowOnErrorDeserializing { get; set; }

        [DataMember]
        public bool ThrowOnErrorSerializing { get; set; }

        public SettingsProperty ToSettingsProperty(SettingsProvider Provider)
        {
            return new SettingsProperty
            (
                Name,
                Type.GetType(PropertyTypeName),
                Provider,
                IsReadOnly,
                DefaultValue,
                SerializeAs,
                Attributes,
                ThrowOnErrorDeserializing,
                ThrowOnErrorSerializing
            );
        }
    }

    [DataContract]
    public class WcfSettingsPropertyValue
    {
        public WcfSettingsPropertyValue() { }

        public WcfSettingsPropertyValue(SettingsPropertyValue value)
        {
            Deserialized = value.Deserialized;
            IsDirty = value.IsDirty;
            Property = new WcfSettingsProperty(value.Property);
            PropertyValue = value.PropertyValue;
            SerializedValue = value.SerializedValue;
        }

        [DataMember]
        public bool Deserialized { get; set; }

        [DataMember]
        public bool IsDirty { get; set; }

        [DataMember]
        public WcfSettingsProperty Property { get; set; }

        [DataMember]
        public object PropertyValue { get; set; }

        [DataMember]
        public object SerializedValue { get; set; }

        public SettingsPropertyValue ToSettingsPropertyValue(SettingsProvider Provider)
        {
            SettingsPropertyValue value = new SettingsPropertyValue(Property.ToSettingsProperty(Provider));

            value.Deserialized = Deserialized;
            value.IsDirty = IsDirty;
            value.PropertyValue = PropertyValue;
            value.SerializedValue = SerializedValue;

            return value;
        }
    }
}
