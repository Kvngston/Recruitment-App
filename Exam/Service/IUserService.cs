using System;
using System.Collections.Generic;
using Exam.Areas.Identity.Pages.Account;
using Exam.Domains;

namespace Exam.Service
{
    public interface IUserService
    {
        void CreateUser(RegisterModel.InputModel viewModel, string userId);
        void AddUserApplication(User user, Application application);
        
        Tuple<List<Application>, User, List<WorkExperience>> ReturnUserDetails(string userId);
    }
}