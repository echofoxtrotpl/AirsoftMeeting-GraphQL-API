using System;
using System.Collections.Generic;

namespace AirsoftMeetingGraphQL.Entities
{
    public class Location
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string LocationName { get; set; }
        public virtual List<Event> Events { get; set; }
        
        #nullable enable
        public DateTime? DeleteDate { get; set; }
    }
}