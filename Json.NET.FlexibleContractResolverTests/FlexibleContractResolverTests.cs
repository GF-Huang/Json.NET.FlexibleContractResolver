using Microsoft.VisualStudio.TestTools.UnitTesting;
using Json.NET.ContractResolver;
using Newtonsoft.Json.PropertyMappingTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Json.NET.PropertyMapping.Tests {
    [TestClass]
    public class FlexibleContractResolverTests {
        [TestMethod]
        public void Style1Test() {
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

            MappingTest();

            IgnoreTest();
        }

        [TestMethod]
        public void Style2Test() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new FlexibleContractResolver(resolver => {
                    resolver.AddNameMappings<Model1>(new Dictionary<string, string> {
                        [nameof(Model1.Property1)] = "json_property1",
                        [nameof(Model1.Property2)] = "json_property2",
                        [nameof(Model1.Property3)] = "json_property3"

                    }).AddNameMappings<Model2>(new Dictionary<string, string> {
                        [nameof(Model2.Property11)] = "json_property11",
                        [nameof(Model2.Property22)] = "json_property22",
                        [nameof(Model2.Property33)] = "json_property33"

                    }).AddIgnores<Model1>(new[] {
                        nameof(Model1.ShouldBeIgnore)

                    }).AddIgnores<Model2>(nameof(Model2.ShouldBeIgnore));
                })
            };

            MappingTest();

            IgnoreTest();
        }

        private void MappingTest() {
            // Serialization
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

            // Deserialization
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

        private void IgnoreTest() {
            // Serialization
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

            // Deserialization
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
        }
    }
}