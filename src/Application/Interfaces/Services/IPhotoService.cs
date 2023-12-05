using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<string> AddPhotoAsync(IFormFile file, string publicId, int height, int width, string folder);
        Task DeletePhotoAsync(string publicId);
    }
}
