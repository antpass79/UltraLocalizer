using Globe.Shared.DTOs;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Hubs;
using Globe.TranslationServer.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
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
