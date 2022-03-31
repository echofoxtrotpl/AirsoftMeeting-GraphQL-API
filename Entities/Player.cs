using System;
using System.Collections.Generic;

namespace AirsoftMeetingGraphQL.Entities
{
    public class Player
    {
        public long Id { get; set; }
        public string JwtPlayerId { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;
        public virtual Team Team { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<PlayersJoinedEvent> JoinedEvents { get; set; }
        public virtual List<Event> HostedEvents { get; set; }
        
        #nullable enable
        public int? TeamId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}