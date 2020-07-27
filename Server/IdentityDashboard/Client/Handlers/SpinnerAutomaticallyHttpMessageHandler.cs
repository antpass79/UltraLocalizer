using Globe.Identity.AdministrativeDashboard.Client.Components;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Handlers
{
    public class AutoSpinnerHttpMessageHandler : DelegatingHandler
    {
        private readonly SpinnerService _spinnerService;
        public AutoSpinnerHttpMessageHandler(SpinnerService spinnerService)
        {
            _spinnerService = spinnerService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Send Async");

            _spinnerService.Show();
            var response = await base.SendAsync(request, cancellationToken);
            _spinnerService.Hide();

            return response;
        }
    }
}
