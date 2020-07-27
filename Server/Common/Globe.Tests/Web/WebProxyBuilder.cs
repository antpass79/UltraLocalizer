using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;

namespace Globe.Tests.Web
{
    public class WebProxyBuilder<TStartup>
        where TStartup : class
    {
        string _jsonFile = String.Empty;
        IConfigurationRoot _configurationRoot;
        string _baseAddress;

        public WebProxyBuilder()
        {
        }

        public WebProxyBuilder<TStartup> JsonFile(string jsonFile)
        {
            _jsonFile = jsonFile;
            return this;
        }

        public WebProxyBuilder<TStartup> ConfigurationRoot(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
            return this;
        }

        public WebProxyBuilder<TStartup> BaseAddress(string baseAddress)
        {
            _baseAddress = baseAddress;
            return this;
        }

        public WebProxyTester Build()
        {
            if (string.IsNullOrWhiteSpace(_jsonFile) && _configurationRoot == null)
                throw new ArgumentException("jsonFile or configurationRoot must be set");
            if (!string.IsNullOrWhiteSpace(_jsonFile) && _configurationRoot != null)
                throw new ArgumentException("Only jsonFile or configurationRoot must be set");

            var configurationRoot = _configurationRoot != null ? _configurationRoot : new ConfigurationBuilder().AddJsonFile(_jsonFile).Build();

            var builder = new WebHostBuilder()
                .UseConfiguration(configurationRoot)
                .UseStartup<TStartup>();

            var server = new TestServer(builder);
            return new WebProxyTester(server, _baseAddress);
        }
    }
}
