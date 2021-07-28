using System;
using System.IO;
using System.Threading.Tasks;
using MlsaBadgeMaker.Api.Data.InfluencerApi;
using MlsaBadgeMaker.Api.Properties;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace MlsaBadgeMaker.Api.Services
{
    public interface IAvatarGenerator
    {
        Task<Stream> GenerateAsync(Stream imageStream, string milestoneName);
    }

    public class ImageSharpAvatarGenerator : IAvatarGenerator
    {
        /// <inheritdoc />
        public async Task<Stream> GenerateAsync(Stream imageStream, string milestoneName)
        {
            using var image = await Image.LoadAsync(imageStream);

            var badgeBuffer = milestoneName switch
            {
                MlsaLevel.New => Resources.MSLearn_SA_Profile_Badge_Overlay_GENERIC,
                MlsaLevel.Alpha => Resources.MSLearn_SA_Profile_Badge_Overlay_ALPHA,
                MlsaLevel.Beta => Resources.MSLearn_SA_Profile_Badge_Overlay_BETA,
                MlsaLevel.Gold => Resources.MSLearn_SA_Profile_Badge_Overlay_GOLD,
                _ => throw new ArgumentOutOfRangeException()
            };

            using var badgeImage = await Image.LoadAsync(new MemoryStream(badgeBuffer));
            badgeImage.Mutate(x => x.Resize(image.Size()));

            image.Mutate(x => x.DrawImage(badgeImage, new Point(0), 1));

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, PngFormat.Instance);

            outputStream.Position = 0;
            return outputStream;
        }
    }
}