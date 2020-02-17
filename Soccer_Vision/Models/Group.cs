using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class Group
    {
        public int Group_ID { get; set; }
        public int Admin_ID { get; set; }
        public string Group_Name { get; set; }
        public int Max_Players { get; set; }
        public int Users_Joined { get; set; }
    }
}