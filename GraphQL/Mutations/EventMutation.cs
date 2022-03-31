using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Events;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Location = AirsoftMeetingGraphQL.Entities.Location;

namespace AirsoftMeetingGraphQL.GraphQL.Mutations
{
    public class EventMutation
    {
        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<EventPayload> AddEvent(ClaimsPrincipal claimsPrincipal, EventInput input, [ScopedService] AirsoftDbContext context)
        {
            var player = context.Players
                .FirstOrDefault(p => p.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (player is null) return null;
            
            var newEvent = new Event
            {
                Name = input.Name,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Description = input.Description,
                CreatorId = player.Id,
                ImageId = null,
                Location = new Location
                {
                    City = input.City,
                    Street = input.Street,
                    Region = input.Region,
                    Longitude = input.Longitude,
                    Latitude = input.Latitude,
                    LocationName = input.LocationName,
                }
            };
                
            if (input.Image is not null)
            {
                var image = context.Images.FirstOrDefault(i => input.Image.Equals(i.Url));
                if (image is not null)
                {
                    newEvent.ImageId = image.Id;
                }
            }

            context.Add(newEvent);
            await context.SaveChangesAsync();

            return new EventPayload(newEvent);
        }

        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<EventPayload> CancelEvent(ClaimsPrincipal claimsPrincipal, long id, [ScopedService] AirsoftDbContext context)
        {
            var canceledEvent = context.Events
                .Include(r => r.Creator)
                .FirstOrDefault(e => e.Id == id);
            
            if (canceledEvent is null) return null;
            if (!canceledEvent.Creator.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier))) return null;
            
            canceledEvent.Active = false;
            canceledEvent.DeleteDate = DateTime.Today.AddDays(30);

            await context.SaveChangesAsync();
            return new EventPayload(canceledEvent);
        }

        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<EventPayload> UpdateEvent(ClaimsPrincipal claimsPrincipal, long id, EventInput input, [ScopedService] AirsoftDbContext context)
        {
            var oldEvent = context.Events
                .Include(r => r.Location)
                .Include(r => r.Image)
                .Include(r => r.Creator)
                .FirstOrDefault(e => e.Id == id);

            if (oldEvent is null) return null;
            if (oldEvent.EndDate < DateTime.Today) return null;
            if (!oldEvent.Creator.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier))) return null;
            
            oldEvent.Location.City = input.City;
            oldEvent.Location.Region = input.Region;
            oldEvent.Location.Street = input.Street;
            oldEvent.Location.LocationName = input.LocationName;
            oldEvent.Location.Latitude = input.Latitude;
            oldEvent.Location.Longitude = input.Longitude;

            oldEvent.Name = input.Name;
            oldEvent.StartDate = input.StartDate;
            oldEvent.EndDate = input.EndDate;
            oldEvent.Description = input.Description;

            if (input.Image is not null)
            {
                var image = context.Images.FirstOrDefault(i => input.Image.Equals(i.Url));
                if (image is not null)
                {
                    oldEvent.ImageId = image.Id;
                }
            }

            await context.SaveChangesAsync();

            return new EventPayload(oldEvent);
        }
        
        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<IQueryable<Player>> UpdateEventParticipants(ClaimsPrincipal claimsPrincipal, long id, [ScopedService] AirsoftDbContext context)
        {
            var player = context.Players
                .FirstOrDefault(p => p.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));
            
            if (player is null) return null;

            var editedEvent = context.Events
                .Include(r => r.PlayersJoinedEvent)
                .FirstOrDefault(e => e.Id == id);

            if (editedEvent is null) return null;
            if (editedEvent.EndDate < DateTime.Today) return null;

            var participant = 
                editedEvent.PlayersJoinedEvent.FirstOrDefault(pe => pe.PlayerId == player.Id);

            if (participant is null) {
                editedEvent.PlayersJoinedEvent.Add(new PlayersJoinedEvent
                {
                    EventId = id,
                    PlayerId = player.Id
                });
            } else {
                editedEvent.PlayersJoinedEvent.Remove(participant);
            }
            
            await context.SaveChangesAsync();
            
            return context.PlayersJoinedEvents
                .Where(pe => pe.EventId == id)
                .Select(pe => pe.Player);
        }

    }
}