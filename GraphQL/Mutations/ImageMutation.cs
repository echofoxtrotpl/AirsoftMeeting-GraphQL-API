using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Images;
using AirsoftMeetingGraphQL.Services;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace AirsoftMeetingGraphQL.GraphQL.Mutations
{
    [ExtendObjectType(typeof(EventMutation))]
    public class ImageMutation
    {
        private readonly IImageService _imageService;
        public ImageMutation(IImageService imageService)
        {
            _imageService = imageService;
        }
        
        [Authorize]
        [UseDbContext(typeof(AirsoftDbContext))]
        public async Task<ImagePayload> UploadImage(ClaimsPrincipal claimsPrincipal, IFile file, string desc, [ScopedService] AirsoftDbContext context)
        {
            if (file.Length <= 0) return null;

            var player = context.Players.FirstOrDefault(p =>
                p.JwtPlayerId.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (player is null) return null;
            
            var (name,folder) = await _imageService.Process(new ImageUploadModel
            {
                Name = file.Name,
                Content = file.OpenReadStream()
            }, desc, context, player.Id);

            return new ImagePayload(name, desc, folder);
        }
    }
}