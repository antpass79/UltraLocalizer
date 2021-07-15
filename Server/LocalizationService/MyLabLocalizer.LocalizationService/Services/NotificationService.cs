using MyLabLocalizer.Shared.DTOs;
using MyLabLocalizer.Shared.Utilities;
using MyLabLocalizer.LocalizationService.Hubs;
using MyLabLocalizer.LocalizationService.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MyLabLocalizer.LocalizationService.Services
{
    public class NotificationService : IAsyncNotificationService
    {
        private readonly IHubContext<NotificationHub, IAsyncNotificationService> _notificationHub;

        public NotificationService(IHubContext<NotificationHub, IAsyncNotificationService> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public async Task JoblistChanged(string joblistName)
        {
            await _notificationHub.Clients.All.JoblistChanged(joblistName);
        }

        public async Task ConceptsChanged(NewConceptsResult result)
        {
            await _notificationHub.Clients.Group(SharedConstants.GROUP_MASTER_TRANSLATOR).ConceptsChanged(result);
        }
    }
}
