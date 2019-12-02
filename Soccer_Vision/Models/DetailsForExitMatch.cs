using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class DetailsForExitMatch
    {
        public int MatchID { get; set; }
        public DateTime MatchDate { get; set; }
        public int AdminID { get; set; }
        public int UserID { get; set; }
        public int PlayersJoined { get; set; }
    }
}