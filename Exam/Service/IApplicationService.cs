using System.Collections.Generic;
using Exam.Domains;

namespace Exam.Service
{
    public interface IApplicationService
    {
        Application CreateApplication(User user, Job job);
        List<Application> GetUserApplications(string userId);

        Application GetUserSpecificApplication(string userId, string jobId);
    }
}