using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class RequestJoinMatchClass
    {
        public int userID { get; set; }
        public int matchID { get; set; }
        public DateTime matchDate { get; set; }
        public TimeSpan matchTime { get; set; }
        public int playTime { get; set; }
        public int cityID { get; set; }
        public int fieldID { get; set; }
        public int maxPlayer { get; set; }
        public bool IsInvite { get; set; }
    }
}