using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagament.Models
{
    public class UserConnection
    {
        public User User { get; set; }
        public bool IsPatient { get; set; }
        public string ConnectionId { get; set; }
    }
}