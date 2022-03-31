using System.Linq;
using AirsoftMeetingGraphQL.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Location = AirsoftMeetingGraphQL.Entities.Location;

namespace AirsoftMeetingGraphQL.GraphQL
{
    public class Query
    {
        //Get dbContext from pool factory and return when used
        [UseDbContext(typeof(AirsoftDbContext))]
        [UsePaging(MaxPageSize = 100)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Event> GetEvents([ScopedService] AirsoftDbContext context)
        {
            return context.Events;
        }
        
        [UseDbContext(typeof(AirsoftDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Player> GetPlayers([ScopedService] AirsoftDbContext context)
        {
            return context.Players;
        }
        
        [UseDbContext(typeof(AirsoftDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Location> GetLocations([ScopedService] AirsoftDbContext context)
        {
            return context.Locations;
        }
        
        [UseDbContext(typeof(AirsoftDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Team> GetTeams([ScopedService] AirsoftDbContext context)
        {
            return context.Teams;
        }
    }
}