using System.Threading.Tasks;
using Exam.Domains;
using Exam.Models;

namespace Exam.Service
{
    public interface IAdvertService
    {
        void CreateAdvert(AdvertisementViewModel viewModel);

        Advert GetRandomAdvert();
    }
}