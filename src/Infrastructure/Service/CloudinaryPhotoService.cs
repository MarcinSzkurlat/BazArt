using Application.Exceptions;
using Application.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Service
{
    public class CloudinaryPhotoService : IPhotoService
    {
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;

        public CloudinaryPhotoService(IConfiguration config)
        {
            _config = config;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
            {
                _cloudinary = null;
            }
            else
            {
                _cloudinary = new Cloudinary(new Account()
                {
                    Cloud = _config["Cloudinary:CloudName"],
                    ApiSecret = _config["Cloudinary:ApiSecret"],
                    ApiKey = _config["Cloudinary:ApiKey"]
                });
            }
        }

        public async Task<string> AddPhotoAsync(IFormFile file, string publicId, int height, int width, string folder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
                return ("Photo service is temporary unavailable");

            await using var stream = file.OpenReadStream();

            var photoToUpload = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"BazArt/{folder}",
                PublicId = publicId,
                Transformation = new Transformation()
                    .Height(height)
                    .Width(width)
                    .Crop("fill")
            };

            var result = await _cloudinary.UploadAsync(photoToUpload);

            if (result.Error != null) throw new BadRequestException(result.Error.Message);

            return result.SecureUrl.ToString();
        }

        public async Task DeletePhotoAsync(string publicId)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
                return;

            var photoToDelete = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(photoToDelete);
        }
    }
}
