using MyLabLocalizer.IdentityDashboard.Client.Components;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Handlers
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
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

            try
            {
                _spinnerService.Show();
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _spinnerService.Hide();
            }

            return response;
        }
    }
}
