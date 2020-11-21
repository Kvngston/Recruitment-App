using System;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IJobService JobService { get; }
        public IResumeService ResumeService { get; }
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, IJobService jobService, IResumeService resumeService)
        {
            JobService = jobService;
            ResumeService = resumeService;
            _logger = logger;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddNewJob()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewJob(JobViewModel viewModel)
        {
            try
            {
                JobService.CreateJob(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                ModelState.AddModelError(string.Empty, e.Message);
                return View();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ViewAllJobs()
        {
            return View(JobService.GetListOfJobs());
        }

        public IActionResult ViewParticularJob(string jobId)
        {
            try
            {
                var job = JobService.ViewParticularJob(jobId);
                return View(job);
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

        public IActionResult ViewJobApplicants(string jobId)
        {
            try
            {
                var applicants = JobService.ViewJobApplicants(jobId);
                return View(applicants);
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

        public IActionResult ViewApplicant(int id, string jobId)
        {
            try
            {
                var applicant = JobService.ViewApplicant(id,jobId);
                return View(applicant);
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

        public IActionResult RejectApplicant(int id, string jobId)
        {
            try
            {
                JobService.RejectApplicant(id,jobId);
                return RedirectToAction("ViewAllJobs");
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
        
        public IActionResult AcceptApplicant(int id, string jobId)
        {
            try
            {
                JobService.AcceptApplicant(id,jobId);
                return RedirectToAction("ViewAllJobs");
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


        public IActionResult DownloadResume(string userId)
        {
            try
            {
                ResumeService.GetUploadedResume(userId);
                return RedirectToAction("ViewAllJobs");
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