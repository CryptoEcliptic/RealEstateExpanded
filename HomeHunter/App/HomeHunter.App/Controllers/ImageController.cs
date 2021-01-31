using AutoMapper;
using HomeHunter.Common;
using HomeHunter.Models.BindingModels.Image;
using HomeHunter.Services.CloudinaryServices;
using HomeHunter.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeHunter.App.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private const string InvalidFormatImageMessage = @"Системата каза НЕ! Част от файловете са с невалидно разширение. Можете да качвате само файлове със следните разширения: .jpg .jpeg .png .bmp .gif";
        private const string ImageReplacementwarningMesssage = @"Внимание, добавянето на нови снимки ще изтрие вече записаните такива!";

        private readonly ICloudinaryService cloudinaryService;
        private readonly IImageServices imageServices;
        private readonly IRealEstateServices realEstateServices;
        private readonly IMapper mapper;

        public ImageController(ICloudinaryService cloudinaryService,
            IImageServices imageServices,
            IRealEstateServices realEstateServices,
            IMapper mapper
            )
        {
            this.cloudinaryService = cloudinaryService;
            this.imageServices = imageServices;
            this.realEstateServices = realEstateServices;
            this.mapper = mapper;
        }

        [HttpGet("/Image/Upload/{id}")]
        public IActionResult Upload(string id)
        {
            return View();
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string id, ImageUploadBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["ErrorMessage"] = InvalidFormatImageMessage;
                return View(model ?? new ImageUploadBindingModel());
            }

            if (model.Images.Count > 0)
            {
                if (this.imageServices.ImagesCount(id) < GlobalConstants.ImageUploadLimit)
                {
                    int sequence = 1;
                    foreach (var image in model.Images)
                    {
                        var imageId = Guid.NewGuid().ToString();

                        //var imageUrl = await this.cloudinaryService.UploadPictureAsync(image, imageId);
                        //The code above is commented since Cloudinary is no longer used for saving the images.

                        //Images are saved in the file system. The below method returns the image name, the whole url is no longer needed since the views use the relative path "~/images/imagename". Now in the Db in column Url we save the image name instead of the url. The url would be needed only when using Cloudinary.
                        var imageName = await this.imageServices.ProcessPhotoAsync(image);
                        var isImageAddedInDb = await this.imageServices.AddImageAsync(imageId, imageName, id, sequence);

                        sequence++;
                    }
                }
            }

            RedirectToActionResult redirectResult = new RedirectToActionResult("Create", "Offer", new { @Id = $"{id}" });
            return redirectResult;
        }

        [HttpGet("/Image/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var imageUploadEditServiceModel = await this.imageServices.GetImageDetailsAsync(id);
            var imageUploadEditBindingModel = this.mapper.Map<ImageUploadEditBindingModel>(imageUploadEditServiceModel);
            this.ViewData["ImageReplacementwarningMesssage"] = ImageReplacementwarningMesssage;
            return View(imageUploadEditBindingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ImageUploadEditBindingModel model)
        {

            var realEstateId = await this.realEstateServices.GetRealEstateIdByOfferId(id);

            if (model.Images.Count != 0)
            {
                if (!this.ModelState.IsValid)
                {
                    var imageUploadEditServiceModel = await this.imageServices.GetImageDetailsAsync(id);
                    var imageUploadEditBindingModel = this.mapper.Map<ImageUploadEditBindingModel>(imageUploadEditServiceModel);
                    this.ViewData["ImageReplacementwarningMesssage"] = ImageReplacementwarningMesssage;
                    this.ViewData["ErrorMessage"] = InvalidFormatImageMessage;
                    return this.View(imageUploadEditBindingModel);
                }

                var imageNamesToDelete = await this.imageServices.GetImageNames(realEstateId);

                foreach (var imageName in imageNamesToDelete)
                {
                    await this.imageServices.DeleteImageFile(imageName);
                }

                var hasRemovedImagesFromTheDb = await this.imageServices.RemoveImages(realEstateId);

                int sequence = 1;
                foreach (var image in model.Images)
                {
                    var imageId = Guid.NewGuid().ToString();

                    var imageName = await this.imageServices.ProcessPhotoAsync(image);
                    var isImageAddedInDb = await this.imageServices.AddImageAsync(imageId, imageName, realEstateId, sequence);

                    sequence++;
                }
            }

            RedirectToActionResult redirectResult = new RedirectToActionResult("Edit", "RealEstates", new { @Id = $"{realEstateId}" });
            return redirectResult;
        }
    }
}