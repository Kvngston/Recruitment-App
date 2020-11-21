using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Exam.Data;
using Exam.Domains;
using Exam.Models;
using Exam.Service;

namespace Exam.Repository
{
    public class AdvertRepository : IAdvertService
    {
        public ICloudinaryInitialization CloudinaryInitialization { get; }
        public ApplicationDbContext DbContext { get; set; }

        public AdvertRepository(ICloudinaryInitialization cloudinaryInitialization, ApplicationDbContext dbContext)
        {
            CloudinaryInitialization = cloudinaryInitialization;
            DbContext = dbContext;
        }
        
        public void CreateAdvert(AdvertisementViewModel viewModel)
        {
            var cloudinary = CloudinaryInitialization.Initialize();

            if (viewModel.AdvertImage.Name == null)
            {
                throw new Exception("Image not found, Please add an image");
            }
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(viewModel.AdvertImage.FileName, viewModel.AdvertImage.OpenReadStream()),
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }
            
            var advert = new Advert()
            {
                AdvertName = viewModel.AdvertName,
                AdvertCompany = viewModel.AdvertCompanyOrBrand,
                AdvertLink = viewModel.AdvertLink,
                AdvertImageLocation = uploadResult.SecureUrl.AbsoluteUri,
            };

            DbContext.Adverts.Add(advert);
            DbContext.SaveChanges();
        }

        public Advert GetRandomAdvert()
        {
            var advertCount = DbContext.Adverts.ToList().Count;
            var randomNumber = new Random().Next(advertCount);
            
            // return DbContext.Adverts.FirstOrDefault(x => x.Id == randomNumber);

            if (advertCount > 0)
            {
                return DbContext.Adverts.ToList()[randomNumber];
            }

            return null;
        }
    }
}