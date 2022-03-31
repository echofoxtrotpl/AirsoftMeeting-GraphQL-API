using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirsoftMeetingGraphQL.Entities;
using AirsoftMeetingGraphQL.GraphQL.Images;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace AirsoftMeetingGraphQL.Services
{
    public interface IImageService
    {
        Task<(string, string)> Process(ImageUploadModel image, string desc, AirsoftDbContext context, long imageCreatorId);
        Task SaveImage(Image image, string name, int resizedWidth);
    }

    public class ImageService : IImageService
    {
        private readonly int _fullscreenWidth = 1296;
        private readonly int _thumbnailWidth = 540;

        public async Task<(string, string)> Process(ImageUploadModel image, string desc, AirsoftDbContext context, long imageCreatorId)
        {

            var totalImages = await context
                .Images
                .CountAsync();
            
            try
            {
                // Storing images in different directories is used to improve system's directories indexing
                var folder = $"{totalImages % 1000}";
                var path = $"../uploaded/images/{folder}/";
                var name = $"image_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.jpeg";

                // Using multithreading to improve images process time
                await Task.Run(async () =>
                {
                    using var imageResult = await Image.LoadAsync(image.Content);
                    
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    
                    await SaveImage(imageResult, Path.Combine(path, $"original_{name}"), imageResult.Width);
                    await SaveImage(imageResult, Path.Combine(path, $"fullscreen_{name}"), _fullscreenWidth);
                    await SaveImage(imageResult, Path.Combine(path, $"thumbnail_{name}"), _thumbnailWidth);
                    
                    if (desc.Equals("profile"))
                    {
                        var profImg = context.Images.FirstOrDefault(i => i.CreatorId == imageCreatorId && i.Description.Equals("profile"));
                        if (profImg is not null)
                        {
                            context.Images.Remove(profImg);
                        }
                    }

                    context.Images.Add(new AirsoftMeetingGraphQL.Entities.Image
                    {
                        CreatorId = imageCreatorId,
                        Folder = folder,
                        Url = name,
                        Description = desc
                    });

                    await context.SaveChangesAsync();
                });

                return (name, folder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return (null, null);
            }
        }

        public async Task SaveImage(Image image, string path, int resizedWidth)
        {
            var width = image.Width;
            var height = image.Height;

            if (width > resizedWidth)
            {
                height = (int) ((double) resizedWidth / width * height);
                width = resizedWidth;
            }
            
            image.Mutate(i => i.Resize(new Size(width, height)));
            image.Mutate(i => i.AutoOrient());
            // Removing ExifProfile to avoid storing unsafe data
            image.Metadata.ExifProfile = null;
            
            await image.SaveAsJpegAsync(path, new JpegEncoder
            {
                Quality = 75
            });
        }
    }
}