using CloudinaryDotNet;
using Exam.Service;
using Microsoft.Extensions.Configuration;

namespace Exam.Repository
{
    public class CloudinaryInitializationImpl : ICloudinaryInitialization
    {
        public IConfiguration Configuration { get; }

        public CloudinaryInitializationImpl(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Cloudinary Initialize()
        {
            Account account = new Account()
            {
                Cloud = Configuration.GetValue<string>("CloudinaryConfigurationStrings:cloudName"),
                ApiKey = Configuration.GetValue<string>("CloudinaryConfigurationStrings:apiKey"),
                ApiSecret = Configuration.GetValue<string>("CloudinaryConfigurationStrings:apiSecret")
            };

            return new Cloudinary(account);
        }
    }
}