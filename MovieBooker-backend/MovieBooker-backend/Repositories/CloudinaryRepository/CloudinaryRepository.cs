using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace MovieBooker_backend.Repositories.CloudinaryRepository
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _folder;

        public CloudinaryRepository(Cloudinary cloudinary, IOptions<CloudinarySettings> config)
        {
            _cloudinary = cloudinary;
            var settings = config.Value;
            _folder = settings.Folder;
        }

        public async Task<UploadResult> UploadImageAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                Folder = _folder
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }
    }
}
