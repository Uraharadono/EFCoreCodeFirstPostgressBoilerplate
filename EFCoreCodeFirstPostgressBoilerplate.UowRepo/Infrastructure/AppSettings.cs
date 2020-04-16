using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Infrastructure
{
    public interface ISettings
    {
        string AdminBaseUrl { get; }
        string AuthBaseUrl { get; }
        string AuthApiName { get; }

        string ClientBaseUrl { get; }
    }

    public class AppSettings : ISettings
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public AppSettings(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public string AdminBaseUrl => ReadString("AdminBaseUrl");
        public string AuthBaseUrl => ReadString("AuthBaseUrl");
        public string AuthApiName => ReadString("AuthApiName");
        public string ClientBaseUrl => ReadString("ClientBaseUrl");

        private string ReadString(string key)
        {
            var settings = _config.GetSection("AppSettings");
            return settings[key];
        }
    }
}
