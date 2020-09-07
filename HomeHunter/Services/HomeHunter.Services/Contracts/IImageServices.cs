using HomeHunter.Services.Models.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services.Contracts
{
    public interface IImageServices
    {
        Task<bool> AddImageAsync(string publikKey, string url, string estateId, int sequence);

        ImageLoadServiceModel LoadImagesAsync(string offerId);

        Task<ImageUploadEditServiceModel> GetImageDetailsAsync(string offerId);

        int ImagesCount(string id);

        Task<bool> EditImageAsync(string publicKey, string url, string estateId, int sequence);

        Task<int> RemoveImages(string estateId);

        Task <IEnumerable<string>> GetImageNames(string realEstateId);

        Task<string> ProcessPhotoAsync(IFormFile photo);

        Task<bool> DeleteImageFile(string fileName);
    }
}
