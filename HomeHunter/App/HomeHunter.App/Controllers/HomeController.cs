﻿using AutoMapper;
using HomeHunter.App.MLPricePrediction;
using HomeHunter.Common;
using HomeHunter.Domain;
using HomeHunter.Models.ViewModels.Offer;
using HomeHunter.Services.Contracts;
using HomeHunter.Services.EmailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using System.Threading.Tasks;

namespace HomeHunter.App.Controllers
{
    public class HomeController : Controller
    {
        private const string SuccessfullySentQuestionMessage = "Успешно изпратихте запитване към служителите ни!";

        private readonly IUserServices usersService;
        private readonly UserManager<HomeHunterUser> userManager;
        private readonly IApplicationEmailSender emailSender;
        private readonly IVisitorSessionServices visitorSessionServices;
        private readonly PredictionEnginePool<InputModel, OutputModel> predictionEngine;
        private readonly IHttpContextAccessor accessor;

        public HomeController(IUserServices usersService,
            UserManager<HomeHunterUser> userManager,
            IApplicationEmailSender emailSender,
            IVisitorSessionServices visitorSessionServices,
            PredictionEnginePool<InputModel, OutputModel> predictionEngine,
            IHttpContextAccessor accessor)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.visitorSessionServices = visitorSessionServices;
            this.predictionEngine = predictionEngine;
            this.accessor = accessor;
        }
        public IActionResult Index()
        {
            var ip = this.accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            string visitorId = HttpContext.Request.Cookies["ai_user"];

            this.visitorSessionServices.AddSessionInTheDb(ip, visitorId);

            if (this.User.IsInRole("User"))
            {
                return RedirectToAction("AuthenticatedIndex", "Home");
            }
            else if (this.User.IsInRole("Admin"))
            {
                return LocalRedirect("~/Administration");
            }

            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult AuthenticatedIndex()
        {
            var userId = this.userManager.GetUserId(this.User);
            var isUserEmailAuthenticated = this.usersService.IsUserEmailAuthenticated(userId)
                && this.User.Identity.IsAuthenticated;

            return View(isUserEmailAuthenticated);
        }

        
        [HttpGet]
        public IActionResult PredictPrice()
        {
            LoadDropdownMenusData();
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PredictPrice(InputModel model)
        {
            var output = this.predictionEngine.Predict("RegressionAnalysisModel", model);
            LoadDropdownMenusData();
            model.Price = output.Score;

            return this.View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(OfferDetailsGuestViewModel returnModel)
        {
            var model = returnModel.ContactFormBindingModel;

            if (!this.ModelState.IsValid)
            {
                return new RedirectToActionResult("Offer", "Details", model.OfferId);
            }


            //If the control checkbox is marked it is considered that the form is not filled by a person.
            //In that case we return a successful mesage but the contact form is not actualy sent. This is a bot trap.
            if (model.ControlCheckbox)
            {
                this.TempData["SuccessfullSubmition"] = SuccessfullySentQuestionMessage;

                return Redirect($"/Offer/Details/{model.OfferId}");
            }

            await this.emailSender.SendContactFormEmailAsync(model.Email, model.Name + " " + model.ReferenceNumber, model.Message);

            this.TempData["SuccessfullSubmition"] = SuccessfullySentQuestionMessage;

            return Redirect($"/Offer/Details/{model.OfferId}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [NonAction]
        private void LoadDropdownMenusData()
        {
            this.ViewData["AppartmentTypes"] = GlobalConstants.ImotBgAppartmentTypes;
            this.ViewData["Districts"] = GlobalConstants.ImotBgSofiaDistricts;
            this.ViewData["BuildingTypes"] = GlobalConstants.ImotBgBuildingTypes;
        }
    }
}
