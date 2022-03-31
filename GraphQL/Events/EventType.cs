using System.Linq;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Images;
using HotChocolate;
using HotChocolate.Types;
using Location = AirsoftMeetingGraphQL.Entities.Location;

namespace AirsoftMeetingGraphQL.GraphQL.Events
{
    public class EventType : ObjectType<Event>
    {
        protected override void Configure(IObjectTypeDescriptor<Event> descriptor)
        {
            descriptor
                .Field(e => e.Creator)
                .ResolveWith<Resolvers>(r => r.GetEventCreator(default! ,default!))
                .UseDbContext<AirsoftDbContext>();
            descriptor
                .Field(e => e.Image)
                .ResolveWith<Resolvers>(r => r.GetEventImage(default!, default!))
                .UseDbContext<AirsoftDbContext>();
            descriptor
                .Field(e => e.Location)
                .ResolveWith<Resolvers>(r => r.GetEventLocation(default!, default!))
                .UseDbContext<AirsoftDbContext>();
            descriptor
                .Field(e => e.PlayersJoinedEvent)
                .ResolveWith<Resolvers>(r => r.GetPlayersJoinedEvent(default!, default!))
                .UseDbContext<AirsoftDbContext>();
        }

        private class Resolvers
        {
            public IQueryable<EventHostModel> GetEventCreator([Parent] Event singleEvent, [ScopedService] AirsoftDbContext context)
            {
                return context.Players
                    .Select(p => new EventHostModel
                    {
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber,
                        TeamName = p.Team.TeamName,
                        Username = p.Username
                    })
                    .Where(p => p.Id == singleEvent.CreatorId);
            }
            
            public IQueryable<ImageModel> GetEventImage([Parent] Event singleEvent, [ScopedService] AirsoftDbContext context)
            {
                return context.Images
                    .Where(i => i.Id == singleEvent.ImageId)
                    .Select(i => new ImageModel
                    {
                        Image = i.Url,
                        Folder = i.Folder,
                        Description = "event"
                    });
            }
            public IQueryable<Location> GetEventLocation([Parent] Event singleEvent, [ScopedService] AirsoftDbContext context)
            {
                return context.Locations.Where(l => l.Id == singleEvent.LocationId);
            }
            
            public IQueryable<Player> GetPlayersJoinedEvent([Parent] Event singleEvent, [ScopedService] AirsoftDbContext context)
            {
                return context.PlayersJoinedEvents
                    .Where(pe => pe.EventId == singleEvent.Id)
                    .Select(pe => pe.Player);
            }
        }
    }
}