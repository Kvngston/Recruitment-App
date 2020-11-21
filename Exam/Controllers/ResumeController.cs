using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly ILogger<ResumeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IResumeService ResumeService { get; }

        public ResumeController(IResumeService resumeService, ILogger<ResumeController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            ResumeService = resumeService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateResume(ResumeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //get Logged In User Id
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    //check for file extension to be pdf or docx file 
                    var permittedTypes = new[] {".docx", ".pdf"};
                    var extensionType = Path.GetExtension(viewModel.ResumePdf.FileName)?.ToLowerInvariant();
                    if (string.IsNullOrEmpty(extensionType) || !permittedTypes.Contains(extensionType))
                    {
                        ModelState.AddModelError(string.Empty, "Only Pdf files are permitted");
                    }

                    ResumeService.CreateResume(viewModel, userId);
                }
                catch (Exception e)
                {
                    //log errors with the logger
                    _logger.LogError(e.Message);
                    _logger.LogError(e.StackTrace);
                    ModelState.AddModelError(string.Empty, e.Message);
                }

                return RedirectToAction("Index", "UserDashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid details");

            return View("Index");
        }

        public IActionResult EditResume()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            //check this logic to redirect to add resume if user doesn't have a resume yet

            try
            {
                return View("EditResume", ResumeService.MapResumeToEditResumeViewModel(userId));
            }
            catch (Exception e)
            {
                if (e.Message == "User Resume not found, Redirecting user to create new Resume")
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    _logger.LogError(e.Message);
                    return RedirectToAction("Index");
                }

                if (e.Message == "User not found")
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    _logger.LogError(e.Message);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditResume(EditResumeViewModel viewModel)
        {
            //edit logic here
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                ResumeService.EditResume(viewModel, userId);
                return RedirectToAction("Index", "UserDashboard");
            }
            catch (Exception e)
            {
                if (e.Message == "User Resume not found, Redirecting user to create new Resume")
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    _logger.LogError(e.Message);
                    return RedirectToAction("Index");
                }

                if (e.Message == "User not found")
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    _logger.LogError(e.Message);
                }
            }

            return View();
        }

        public IActionResult AddWorkExperience()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWorkExperience(WorkExperienceViewModel viewModel)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                ResumeService.AddWorkExperience(viewModel, userId);
                return RedirectToAction("Index", "UserDashboard");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return View();
            }
        }
    }
}