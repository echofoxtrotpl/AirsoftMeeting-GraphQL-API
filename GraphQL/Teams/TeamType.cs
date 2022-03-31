using System.Linq;
using AirsoftMeetingGraphQL.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace AirsoftMeetingGraphQL.GraphQL.Teams
{
    public class TeamType : ObjectType<Team>
    {
        protected override void Configure(IObjectTypeDescriptor<Team> descriptor)
        {
            descriptor
                .Field(t => t.TeamCrew)
                .ResolveWith<Resolvers>(r => r.GetTeamCrew(default!, default!))
                .UseDbContext<AirsoftDbContext>();
        }

        private class Resolvers
        {
            public IQueryable<Player> GetTeamCrew([Parent] Team team, [ScopedService] AirsoftDbContext context)
            {
                return context.Players
                    .Where(p => p.TeamId == team.Id);
            }
        }
    }
}