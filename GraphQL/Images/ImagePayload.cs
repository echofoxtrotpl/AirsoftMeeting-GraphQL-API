namespace AirsoftMeetingGraphQL.GraphQL.Images
{
    public record ImagePayload(
        string Image,
        string Description, 
        string Folder
    );
}