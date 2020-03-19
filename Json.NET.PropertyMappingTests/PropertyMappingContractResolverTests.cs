using Microsoft.VisualStudio.TestTools.UnitTesting;
using Json.NET.PropertyMapping;
using Newtonsoft.Json.PropertyMappingTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Json.NET.PropertyMapping.Tests {
    [TestClass()]
    public class PropertyMappingContractResolverTests {
        [TestMethod()]
        public void PropertyMappingTest() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new PropertyMappingContractResolver {
                    {
                        typeof(Model1), new Dictionary<string, string> {
                            [nameof(Model1.Property1)] = "json_property1",
                            [nameof(Model1.Property2)] = "json_property2",
                            [nameof(Model1.Property3)] = "json_property3"
                        }
                    },
                    {
                        typeof(Model2), new Dictionary<string, string> {
                            [nameof(Model2.Property11)] = "json_property11",
                            [nameof(Model2.Property22)] = "json_property22",
                            [nameof(Model2.Property33)] = "json_property33"
                        }
                    }
                }
            };

            // build json string
            var jsonString = JsonConvert.SerializeObject(new {
                Model1 = new {
                    json_property1 = 123,
                    json_property2 = "123",
                    json_property3 = true
                },
                Model2 = new {
                    json_property11 = 456,
                    json_property22 = "456",
                    json_property33 = true
                }
            });

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
        }
    }
}