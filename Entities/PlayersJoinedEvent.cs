namespace AirsoftMeetingGraphQL.Entities
{
    public class PlayersJoinedEvent
    {
        public long PlayerId { get; set; }
        public long EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual Player Player { get; set; }
    }
}