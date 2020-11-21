using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json.PropertyMappingTests.Models {
    public class ModelContainer {
        public Model1 Model1 { get; set; }

        public Model2 Model2 { get; set; }
    }

    public class Model1 {
        public int Property1 { get; set; }
        public string Property2 { get; set; }
        public bool Property3 { get; set; }
        public string ShouldBeIgnore { get; set; }
    }

    public class Model2 {
        public int Property11 { get; set; }
        public string Property22 { get; set; }
        public bool Property33 { get; set; }
        public decimal ShouldBeIgnore { get; set; }
    }
}
