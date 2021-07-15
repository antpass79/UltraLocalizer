using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Extensions;
using MyLabLocalizer.LocalizationService.Services;
using MyLabLocalizer.LocalizationService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub<IAsyncNotificationService>, IAsyncNotificationService
    {
        public async override Task OnConnectedAsync()
        {
            var isInAdministratorGroup = Context.User.IsInAdministratorGroup();
            if (isInAdministratorGroup)
                await Groups.AddToGroupAsync(Context.ConnectionId, SharedConstants.GROUP_MASTER_TRANSLATOR);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var isInAdministratorGroup = Context.User.IsInAdministratorGroup();
            if (isInAdministratorGroup)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, SharedConstants.GROUP_MASTER_TRANSLATOR);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoblistChanged(string joblistName)
        {
            await Clients.All.JoblistChanged(joblistName);           
        }

        public async Task ConceptsChanged(NewConceptsResult result)
        {
            await Clients.Group(SharedConstants.GROUP_MASTER_TRANSLATOR).ConceptsChanged(result);
        }
    }
}
