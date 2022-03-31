using System.Linq;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Images;
using HotChocolate;
using HotChocolate.Types;

namespace AirsoftMeetingGraphQL.GraphQL.Players
{
    public class PlayerType : ObjectType<Player>
    {
        protected override void Configure(IObjectTypeDescriptor<Player> descriptor)
        {
            descriptor
                .Field(p => p.JwtPlayerId).Ignore();

            descriptor
                .Field(p => p.HostedEvents)
                .UseDbContext<AirsoftDbContext>()
                .UsePaging()
                .UseSorting()
                .ResolveWith<Resolvers>(r => r.GetHostedEvents(default!, default!));

            descriptor
                .Field(p => p.JoinedEvents)
                .UseDbContext<AirsoftDbContext>()
                .UsePaging()
                .UseSorting()
                .ResolveWith<Resolvers>(r => r.GetPlayerJoinedEvents(default!, default!));

            descriptor
                .Field(p => p.Images)
                .ResolveWith<Resolvers>(r => r.GetPlayerProfileImage(default!, default!))
                .UseDbContext<AirsoftDbContext>();
            
            descriptor
                .Field(p => p.Team)
                .ResolveWith<Resolvers>(r => r.GetPlayerTeam(default!, default!))
                .UseDbContext<AirsoftDbContext>();
        }
        
        private class Resolvers
        {
            public IQueryable<Event> GetHostedEvents([Parent] Player player, [ScopedService] AirsoftDbContext context)
            {
                return context.Events
                    .Where(e => e.CreatorId == player.Id);
            }
            
            public IQueryable<Team> GetPlayerTeam([Parent] Player player, [ScopedService] AirsoftDbContext context)
            {
                return context.Teams
                    .Where(t=> t.Id == player.TeamId);
            }
            
            public IQueryable<Event> GetPlayerJoinedEvents([Parent] Player player, [ScopedService] AirsoftDbContext context)
            {
                return context.PlayersJoinedEvents
                    .Where(pe => pe.PlayerId == player.Id)
                    .Select(pe => pe.Event);
            }
            
            public IQueryable<ImageModel> GetPlayerProfileImage([Parent] Player player, [ScopedService] AirsoftDbContext context)
            {
                return context.Images
                    .Where(i => i.CreatorId == player.Id && i.Description.Equals("profile"))
                    .Select(i => new ImageModel
                    {
                        Image = i.Url,
                        Folder = i.Folder,
                        Description = "profile"
                    });
            }
        }
    }
}