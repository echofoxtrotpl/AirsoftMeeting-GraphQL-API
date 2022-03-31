using AirsoftMeetingGraphQL.Entities;

namespace AirsoftMeetingGraphQL.GraphQL.Events
{
    public record EventPayload(
        Event singleEvent
    );
}