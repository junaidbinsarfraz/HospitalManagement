using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HospitalManagament.Models
{
    [DataContract]
    public class DiseasePercentage
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }
        [DataMember(Name = "value")]
        public double Value { get; set; }
        [DataMember(Name = "color")]
        public string Color { get; set; }
        [DataMember(Name = "highlight")]
        public string Highlight { get; set; }
        public int Count { get; set; }
    }
}