using System;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [AllowAnonymous]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertService _advertService;
        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(IAdvertService advertService, ILogger<AdvertisementController> logger)
        {
            _advertService = advertService;
            _logger = logger;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAdvert(AdvertisementViewModel viewModel)
        {
            try
            {
                _advertService.CreateAdvert(viewModel);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return View("Index");
            }
        }
    }
}