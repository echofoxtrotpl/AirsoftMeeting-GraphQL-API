using System;
using System.Collections.Generic;

namespace AirsoftMeetingGraphQL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string TeamName { get; set; }
        public List<Player> TeamCrew { get; set; } = new ();
        
        #nullable enable
        public DateTime? DeleteDate { get; set; }
    }
}