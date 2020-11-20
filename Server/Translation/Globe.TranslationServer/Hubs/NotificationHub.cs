using Globe.TranslationServer.Extensions;
using Globe.TranslationServer.Services;
using Globe.TranslationServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<IAsyncNotificationService>, IAsyncNotificationService
    {
        public async override Task OnConnectedAsync()
        {
            var masterTranslator = Context.User.IsMasterTranslator();
            if (masterTranslator)
                await Groups.AddToGroupAsync(Context.ConnectionId, Constants.GROUP_MASTER_TRANSLATOR);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var masterTranslator = Context.User.IsMasterTranslator();
            if (masterTranslator)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, Constants.GROUP_MASTER_TRANSLATOR);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoblistChanged(string joblistName)
        {
            await Clients.All.JoblistChanged(joblistName);           
        }

        public async Task ConceptsChanged(int count)
        {
            await Clients.Group(Constants.GROUP_MASTER_TRANSLATOR).ConceptsChanged(count);
        }
    }
}
