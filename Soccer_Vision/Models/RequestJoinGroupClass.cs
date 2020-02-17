using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class RequestJoinGroupClass
    {
        public int userID { get; set; }
        public int groupID { get; set; }
        public int maxPlayer { get; set; }
        public bool IsInvite { get; set; }
    }
}