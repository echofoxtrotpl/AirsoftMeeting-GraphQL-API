using System.IO;

namespace AirsoftMeetingGraphQL.GraphQL.Images
{
    public class ImageUploadModel
    {
        public string Name { get; set; }
        public Stream Content { get; set; }
    }
}