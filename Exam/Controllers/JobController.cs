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
    [AllowAnonymous]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JobController> _logger;

        public JobController(IJobService jobService, IHttpContextAccessor httpContextAccessor, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        
        //browse jobs
        // GET
        public IActionResult Index()
        {
            var jobs = _jobService.GetListOfJobs();
            return View(jobs);
        }
        
        public IActionResult ViewParticularJob(string jobId)
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
        
        
        [Authorize]
        public IActionResult ApplyForJob(string jobId)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _jobService.ApplyForJob(userId, jobId);
                return RedirectToAction("Index");
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