using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json.NET.ContractResolver {
    public class FlexibleContractResolver : DefaultContractResolver {
        public Dictionary<Type, PropertySetting> PropertySettings { get; }

        public FlexibleContractResolver() {
            PropertySettings = new Dictionary<Type, PropertySetting>();
        }

        public FlexibleContractResolver(Action<FlexibleContractResolver> configuration) : this() {
            configuration(this);
        }

        #region Override

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (PropertySettings.TryGetValue(member.DeclaringType, out var setting)) {
                if (setting.IgnoreSet.Contains(member.Name))
                    property.Ignored = true;
                else if (setting.NameMappings.TryGetValue(member.Name, out var name))
                    property.PropertyName = name;
            }

            return property;
        }

        #endregion

        #region Public

        public FlexibleContractResolver AddMapping<T>(string propertyName, string jsonFieldName) {
            GetSetting<T>().NameMappings[propertyName] = jsonFieldName;

            return this;
        }

        public FlexibleContractResolver AddNameMappings<T>(IEnumerable<KeyValuePair<string, string>> mappings) {
            var setting = GetSetting<T>();
            foreach (var mapping in mappings)
                setting.NameMappings.Add(mapping.Key, mapping.Value);

            return this;
        }

        public FlexibleContractResolver AddNameMappings<T>(params KeyValuePair<string, string>[] mappings) =>
            AddNameMappings<T>(mappings.AsEnumerable());

        public FlexibleContractResolver AddIgnores<T>(IEnumerable<string> ignores) {
            var setting = GetSetting<T>();
            foreach (var ignore in ignores)
                setting.IgnoreSet.Add(ignore);

            return this;
        }

        public FlexibleContractResolver AddIgnores<T>(params string[] ignores) => AddIgnores<T>(ignores.AsEnumerable());

        #endregion

        #region Private

        private PropertySetting GetSetting<T>() {
            var type = typeof(T);
            if (!PropertySettings.TryGetValue(type, out var setting)) {
                setting = new PropertySetting();
                PropertySettings[type] = setting;
            }

            return setting;
        }

        #endregion
    }
}
