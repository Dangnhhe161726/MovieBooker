using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Threading.Tasks;

namespace MovieBooker_backend.Repositories.CloudinaryRepository
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<UploadResult> UploadImageAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }
    }
}
