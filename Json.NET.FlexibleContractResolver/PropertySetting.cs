using System;
using System.Collections.Generic;
using System.Text;

namespace Json.NET.ContractResolver {
    public class PropertySetting {
        public Dictionary<string, string> NameMappings { get; } = new Dictionary<string, string>();

        public HashSet<string> IgnoreSet { get; } = new HashSet<string>();
    }
}
