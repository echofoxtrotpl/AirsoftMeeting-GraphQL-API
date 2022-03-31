using System.Linq;
using AirsoftMeetingGraphQL.Entities;
using HotChocolate;
using HotChocolate.Types;
using Location = AirsoftMeetingGraphQL.Entities.Location;

namespace AirsoftMeetingGraphQL.GraphQL.Locations
{
    public class LocationType : ObjectType<Location>
    {
        protected override void Configure(IObjectTypeDescriptor<Location> descriptor)
        {
            descriptor
                .Field(l => l.CreateDate).Ignore();
                
            descriptor
                .Field(l => l.DeleteDate).Ignore();

            descriptor
                .Field(l => l.Events)
                .ResolveWith<Resolvers>(r => r.GetLocationEvents(default!, default!))
                .UseDbContext<AirsoftDbContext>();
        }

        private class Resolvers
        {
            public IQueryable<Event> GetLocationEvents([Parent] Location location, [ScopedService] AirsoftDbContext context)
            {
                return context.Events
                    .Where(e => e.LocationId == location.Id);
            }
        }
    }
}