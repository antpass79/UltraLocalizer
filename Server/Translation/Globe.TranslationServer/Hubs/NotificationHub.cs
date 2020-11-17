using Globe.TranslationServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Hubs
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<IAsyncNotificationService>, IAsyncNotificationService
    {
        public async Task JoblistChanged(string joblistName)
        {
            await Clients.All.JoblistChanged(joblistName);           
        }

        public async Task ConceptsChanged(int count)
        {
            await Clients.All.ConceptsChanged(count);
        }
    }
}
