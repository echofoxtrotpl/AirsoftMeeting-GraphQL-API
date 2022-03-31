using System;

namespace AirsoftMeetingGraphQL.GraphQL.Events
{
    public record EventInput(
        string Name,
        DateTime StartDate,
        DateTime EndDate,
        string Description,
        string LocationName,
        double Latitude,
        double Longitude,
        string City,
        string Street,
        string Region,
        string Image,
        string Folder
    );
}