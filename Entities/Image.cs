using System;

namespace AirsoftMeetingGraphQL.Entities
{
    public class Image
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Url { get; set; }
        public string Folder { get; set; }
        public string Description { get; set; }
        public long CreatorId { get; set; }
        public virtual Player Creator { get; set; }
        public virtual Event Event { get; set; }
        
        #nullable enable
        public DateTime? DeleteDate { get; set; }
    }
}