# Json.NET.FlexibleContractResolver
A ContractResolver enables Json.NET (Newtonsoft.Json) to work with third-party classes and without using JsonProperty.

You can configuration which property to ignore or custom name mapping easily.

# NuGet
https://www.nuget.org/packages/Json.NET.FlexibleContractResolver/

# Usage

### Configuration

```cs
JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
    ContractResolver = new FlexibleContractResolver()
        .AddNameMappings<Model1>(new Dictionary<string, string> {
            [nameof(Model1.Property1)] = "json_property1",
            [nameof(Model1.Property2)] = "json_property2",
            [nameof(Model1.Property3)] = "json_property3"

        }).AddNameMappings<Model2>(new Dictionary<string, string> {
            [nameof(Model2.Property11)] = "json_property11",
            [nameof(Model2.Property22)] = "json_property22",
            [nameof(Model2.Property33)] = "json_property33"

        }).AddIgnores<Model1>(new[] {
            nameof(Model1.ShouldBeIgnore)

        }).AddIgnores<Model2>(nameof(Model2.ShouldBeIgnore))
};
```

### Name Mapping Serialization Example
```cs
var jsonString = JsonConvert.SerializeObject(new ModelContainer {
    Model1 = new Model1 {
        Property1 = 123,
        Property2 = "123",
        Property3 = true
    },
    Model2 = new Model2 {
        Property11 = 456,
        Property22 = "456",
        Property33 = true
    }
});
```

### Name Mapping Deserialization Example
```cs
var modelContainer = JsonConvert.DeserializeObject<ModelContainer>(jsonString);

Assert.IsNotNull(modelContainer);
Assert.IsNotNull(modelContainer.Model1);
Assert.IsNotNull(modelContainer.Model2);
Assert.AreEqual(modelContainer.Model1.Property1, 123);
Assert.AreEqual(modelContainer.Model1.Property2, "123");
Assert.AreEqual(modelContainer.Model1.Property3, true);
Assert.AreEqual(modelContainer.Model2.Property11, 456);
Assert.AreEqual(modelContainer.Model2.Property22, "456");
Assert.AreEqual(modelContainer.Model2.Property33, true);
```

### Property Ignore Serialization Exmaple
```cs
var jsonString = JsonConvert.SerializeObject(new ModelContainer {
    Model1 = new Model1 {
        Property1 = 123,
        ShouldBeIgnore = "ABC"
    },
    Model2 = new Model2 {
        Property11 = 456,
        ShouldBeIgnore = 123.456m
    }
});

// no "ShouldBeIgnore" inside the serialized json string
Assert.IsFalse(jsonString.Contains(nameof(Model1.ShouldBeIgnore), StringComparison.OrdinalIgnoreCase));
```

### Property Ignore Deserialization Exmaple
```cs
jsonString = @"
{
    ""Model1"": {
        ""json_property1"": 123,
        ""ShouldBeIgnore"": ""ABC""
    },
    ""Model2"": {
        ""json_property11"": 456,
        ""ShouldBeIgnore"": ""123.456""
    }
}
";

var container = JsonConvert.DeserializeObject<ModelContainer>(jsonString);

Assert.AreEqual(container.Model1.Property1, 123);
Assert.AreNotEqual(container.Model1.ShouldBeIgnore, "ABC");
Assert.IsNull(container.Model1.ShouldBeIgnore);
Assert.AreEqual(container.Model2.Property11, 456);
Assert.AreNotEqual(container.Model2.ShouldBeIgnore, 123.456m);
Assert.AreEqual(container.Model2.ShouldBeIgnore, decimal.Zero);
```


#### More details see the unit test project.
