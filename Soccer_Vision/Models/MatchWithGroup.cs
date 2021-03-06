﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class MatchWithGroup
    {
        public int MatchID { get; set; }
        public string MatchName { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public int FieldID { get; set; }
        public int CityID { get; set; }
        public bool IsPrivate { get; set; }
        public int UserID { get; set; }
        public string MatchKey { get; set; }
        public bool IsActive { get; set; }
        public int MaxPlayers { get; set; }
        public int PlayersJoined { get; set; }
        public string PhotoUrl { get; set; }
        public int PlayTime { get; set; }
        public List<int> UsersInGroup { get; set; }
    }
}