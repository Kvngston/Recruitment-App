using System;
using System.Collections.Generic;
using System.Security.Claims;
using Exam.Domains;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserDashboardController> _logger;
        private readonly IUserService _userService;
        private readonly IJobService _jobService;

        public UserDashboardController(IApplicationService applicationService, IHttpContextAccessor httpContextAccessor, ILogger<UserDashboardController> logger, IUserService userService, IJobService jobService)
        {
            _applicationService = applicationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userService = userService;
            _jobService = jobService;
        }
        
        // GET
        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _userService.ReturnUserDetails(userId);
            return View(userDetails);
        }

        public IActionResult ViewJobDetails(string jobId)
        {
            try
            {
                return View(_jobService.ViewParticularJob(jobId));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return View("Error", new ErrorViewModel()
                {
                    RequestId = e.Message
                });
            }
        }
    }
}