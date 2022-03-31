using System;
using System.Collections.Generic;

namespace AirsoftMeetingGraphQL.Entities
{
    public class Event
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = true;
        public virtual Player Creator { get; set; }
        public long CreatorId { get; set; }
        public virtual Image Image { get; set; }
        public virtual Location Location { get; set; }
        public long LocationId { get; set; }
        public virtual List<PlayersJoinedEvent> PlayersJoinedEvent { get; set; } = new ();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        
        #nullable enable
        public DateTime? DeleteDate { get; set; }
        public long? ImageId { get; set; }
    }
}