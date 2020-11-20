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

        public async Task ConceptsChanged(int count)
        {
            await _notificationHub.Clients.Group(Constants.GROUP_MASTER_TRANSLATOR).ConceptsChanged(count);
        }
    }
}
