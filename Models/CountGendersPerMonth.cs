using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagament.Models
{
    public class CountGendersPerMonth
    {
        public string Gender { get; set; }
        public string Month { get; set; }
        public int MonthNumber { get; set; }
        public int Count { get; set; }
        public DateTime ActualDate { get; set; }
    }
}