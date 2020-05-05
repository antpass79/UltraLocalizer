using Globe.Identity.Models;
using Globe.Identity.Services;
using Globe.Tests.Web;
using Globe.Tests.Web.Extensions;
using Globe.TranslationServer.DTOs;
using Globe.TranslationServer.Tests.Mocks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Globe.TranslationServer.Tests.Controllers
{
    [Trait(nameof(JobListControllerTests), "Job List Controller")]
    public class JobListControllerTests
    {
        WebProxyBuilder<Startup> _webProxyBuilder = new WebProxyBuilder<Startup>();

        public JobListControllerTests()
        {
            _webProxyBuilder.JsonFile("appsettings.json");
            _webProxyBuilder.BaseAddress("https://localhost:9500/api/");
        }

        [Fact(DisplayName = "Get Job List for ISOCoding = " + MockConstants.ISO_CODING_EN + " - Check inside Roles => to change")]
        async public void GetJobListFilterByIsoCoding()
        {
            using var client = _webProxyBuilder.Build();

            var search = new JobListSearchDTO
            {
                UserName = MockConstants.USERNAME_MARCODELPIANO,
                coding = MockConstants.ISO_CODING_EN
            };

            var result = await client.SendAsync<JobListSearchDTO>(HttpMethod.Get, "JobList", search);
            var jobList = await result.GetValue<IEnumerable<JobListDTO>>();

            Assert.NotEmpty(jobList);
            Assert.Equal(4, jobList.Count());
        }
    }
}
