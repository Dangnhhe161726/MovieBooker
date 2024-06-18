using CloudinaryDotNet.Actions;

namespace MovieBooker_backend.Repositories.CloudinaryRepository
{
    public interface ICloudinaryRepository
    {
        public Task<UploadResult> UploadImageAsync(Stream imageStream, string fileName);
    }
}
