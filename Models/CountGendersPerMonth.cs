using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagament.Models
{
    public class CountGendersPerMonth
    {
        private string Gender { get; set; }
        private string Month { get; set; }
        private int MonthNumber { get; set; }
        private int Count { get; set; }
    }
}