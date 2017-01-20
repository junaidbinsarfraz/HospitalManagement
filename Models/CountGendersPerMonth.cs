using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagament.Models
{
    public class CountGendersPerMonth
    {
        public string Gender { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
        public string Month { get; set; }
        public int MonthNumber { get; set; }
        public int Count { get; set; }
        public DateTime ActualDate { get; set; }

        public CountGendersPerMonth()
        {

        }

        public CountGendersPerMonth(int MaleCount, int FemaleCount, string Month)
        {
            this.MaleCount = MaleCount;
            this.FemaleCount = FemaleCount;
            this.Month = Month;
        }
    }
}