using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Json.NET.PropertyMapping {
    public class PropertyMappingContractResolver : DefaultContractResolver, IEnumerable<KeyValuePair<Type, Dictionary<string, string>>> {
        public Dictionary<Type, Dictionary<string, string>> PropertyMap { get; } = new Dictionary<Type, Dictionary<string, string>>();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if (PropertyMap.TryGetValue(member.DeclaringType, out var propertyDictionary) &&
                propertyDictionary.TryGetValue(member.Name, out var name))
                property.PropertyName = name;

            return property;
        }

        public void Add(Type type, Dictionary<string, string> propertyMappingDictionary) => PropertyMap.Add(type, propertyMappingDictionary);

        #region IEnumerable<KeyValuePair<Type, KeyValuePair<string, string>>>

        public IEnumerator<KeyValuePair<Type, Dictionary<string, string>>> GetEnumerator() => PropertyMap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => PropertyMap.GetEnumerator();

        #endregion
    }
}
