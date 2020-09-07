using AutoMapper;
using HomeHunter.Data;
using HomeHunter.Domain;
using HomeHunter.Services.Contracts;
using HomeHunter.Services.Models.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services
{
    public class ImageServices : IImageServices
    {
        private const string InvalidImageParamsMessage = "Null image parameters!";
        private const string InvalidRealEstateIdMessage = "Invalid real estate Id!";

        private readonly HomeHunterDbContext context;
        private readonly IMapper mapper;
        private readonly IRealEstateServices realEstateServices;
        private readonly IHostingEnvironment hostingEnvironment;

        public ImageServices(
            HomeHunterDbContext context, 
            IMapper mapper, 
            IRealEstateServices realEstateServices, 
            IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.mapper = mapper;
            this.realEstateServices = realEstateServices;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> AddImageAsync(string publicKey, string url, string realEstateId, int sequence)
        {
            List<Image> images = new List<Image>();

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(realEstateId) || string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(InvalidImageParamsMessage);
            }

            var image = new Image
            {
                Url = url,
                RealEstateId = realEstateId,
                Id = publicKey,
                Sequence = sequence,
            };

            images.Add(image);

            await this.context.Images.AddRangeAsync(images);
            await this.context.SaveChangesAsync();

            return true;
        }

        public ImageLoadServiceModel LoadImagesAsync(string realEstateId)
        {

            var images = this.context.Images
                .Where(x => x.RealEstateId == realEstateId)
                .ToList();

            var imageDelitableServiceModel = this.mapper.Map<List<ImageChangeableServiceModel>>(images);

            ImageLoadServiceModel imageLoadServiceModel = new ImageLoadServiceModel();

            foreach (var image in imageDelitableServiceModel)
            {
                imageLoadServiceModel.Images.Add(image);
            }

            return imageLoadServiceModel;
        }

        public async Task<ImageUploadEditServiceModel> GetImageDetailsAsync(string offerId)
        {
            var realEstateId = await this.realEstateServices.GetRealEstateIdByOfferId(offerId);

            var imagesCount = this.ImagesCount(realEstateId);

            var imageUploadEditServiceModel = new ImageUploadEditServiceModel
            {
                RealEstateId = realEstateId,
                AlreadyUploadedImagesCount = imagesCount,
            };

            return imageUploadEditServiceModel;
        }

        public int ImagesCount(string id)
        {
            if (!this.context.RealEstates.Any(x => x.Id == id))
            {
                throw new ArgumentException(InvalidRealEstateIdMessage);
            }

            var imageCount = this.context.Images
            .Where(x => x.RealEstateId == id)
            .Count();

            return imageCount;
        }

        public async Task<bool> EditImageAsync(string publicKey, string url, string estateId, int sequence)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(estateId) || string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentNullException(InvalidImageParamsMessage);
            }

            var image = new Image
            {
                Url = url,
                RealEstateId = estateId,
                Id = publicKey,
                Sequence = sequence
            };

            await this.context.Images.AddAsync(image);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<int> RemoveImages(string estateId)
        {
            if (string.IsNullOrEmpty(estateId))
            {
                throw new ArgumentNullException(InvalidRealEstateIdMessage);
            }

            var realEstateImages = this.context
                .Images
                .Where(x => x.RealEstateId == estateId)
                .ToList();

            int affectedRows = 0;
            if (realEstateImages.Count != 0)
            {
                try
                {
                    this.context.Images.RemoveRange(realEstateImages);
                    affectedRows = await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return 0;
                }
            }
            return affectedRows;
        }

        public async Task<IEnumerable<string>> GetImageNames(string realEstateId)
        {
            if (string.IsNullOrEmpty(realEstateId))
            {
                throw new ArgumentNullException(InvalidRealEstateIdMessage);
            }

            var imageNames = await this.context.Images
                .Where(x => x.RealEstateId == realEstateId)
                .Select(x => x.Url)
                .ToListAsync();
                
            return imageNames;
        }

        public async Task<string> ProcessPhotoAsync(IFormFile photo)
        {
            string uniqueFileName;
            // The image must be uploaded to the images folder in wwwroot
            // To get the path of the wwwroot folder we are using the inject
            // HostingEnvironment service provided by ASP.NET Core
            string uploadsFolder = await Task.Run(() => Path.Combine(hostingEnvironment.WebRootPath, "images"));
            // To make sure the file name is unique we are appending a new
            // GUID value and and an underscore to the file name
            uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            // Use CopyTo() method provided by IFormFile interface to
            // copy the file to wwwroot/images folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }

        public async Task<bool> DeleteImageFile(string fileName)
        {
            string uploadsFolder = await Task.Run(() => Path.Combine(hostingEnvironment.WebRootPath, "images"));

            string fileToBeDeleted = Path.Combine(uploadsFolder, fileName);

            if (System.IO.File.Exists(fileToBeDeleted))
            {
                System.IO.File.Delete(fileToBeDeleted);
                return true;
            }

            return false;
        }
    }
}
