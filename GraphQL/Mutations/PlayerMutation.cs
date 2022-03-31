using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Players;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMeetingGraphQL.GraphQL.Mutations
{
    [ExtendObjectType(typeof(EventMutation))]
    public class PlayerMutation
    {
        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<PlayerPayload> AddPlayer(ClaimsPrincipal claimsPrincipal, string username, [ScopedService] AirsoftDbContext context)
        {
            var playerExists = context.Players.FirstOrDefault(p => p.Username.Equals(username));
            if (playerExists is not null) return null;

            context.Players.Add(new Player
            {
                Username = username,
                JwtPlayerId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            });

            await context.SaveChangesAsync();

            return new PlayerPayload(username);
        }
        
        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<PlayerPayload> UpdatePlayer(ClaimsPrincipal claimsPrincipal, UpdatePlayerInput input, [ScopedService] AirsoftDbContext context)
        {
            var editedPlayer = context.Players
                .Include(r => r.Team)
                .FirstOrDefault(p => p.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (editedPlayer is null) return null;

            if (input.TeamName is not null)
            {
                var team = context.Teams.FirstOrDefault(t => t.TeamName.Equals(input.TeamName)) ?? context.Teams.Add(new Team
                {
                    TeamCrew = new List<Player>(),
                    TeamName = input.TeamName
                }).Entity;
                
                if(!team.TeamCrew.Contains(editedPlayer))
                    team.TeamCrew.Add(editedPlayer);
            }

            editedPlayer.PhoneNumber = input.PhoneNumber;

            await context.SaveChangesAsync();

            return new PlayerPayload(editedPlayer.Username);
        }
    }
}