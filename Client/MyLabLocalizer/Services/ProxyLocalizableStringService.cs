using MyLabLocalizer.Models;
using Globe.Client.Platofrm.Events;
using MyLabLocalizer.Core.Services;
using MyLabLocalizer.Core.Services.Notifications;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLabLocalizer.Services
{
    public interface IProxyLocalizableStringService : IAsyncLocalizableStringService
    {}

    public class ProxyLocalizableStringService : IProxyLocalizableStringService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ICheckConnectionService _checkConnectionService;
        private readonly IAsyncLocalizableStringService _fileSystemLocalizableStringService;
        private readonly IAsyncLocalizableStringService _httpLocalizableStringService;
        private readonly ILocalizationAppService _localizationAppService;
        private readonly INotificationService _notificationService;

        public ProxyLocalizableStringService(
            IEventAggregator eventAggregator,
            ICheckConnectionService checkConnectionService,
            IFileSystemLocalizableStringService fileSystemLocalizableStringService,
            IHttpLocalizableStringService httpLocalizableStringService,
            ILocalizationAppService localizationAppService,
            INotificationService notificationService)
        {
            _eventAggregator = eventAggregator;
            _checkConnectionService = checkConnectionService;
            _fileSystemLocalizableStringService = fileSystemLocalizableStringService;
            _httpLocalizableStringService = httpLocalizableStringService;
            _localizationAppService = localizationAppService;
            _notificationService = notificationService;
        }

        async public Task<IEnumerable<LocalizableString>> GetAllAsync()
        {
            _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);

            IEnumerable<LocalizableString> strings = new List<LocalizableString>();

            try
            {
                if (_checkConnectionService.IsConnectionAvailable())
                {
                    try
                    {
                        strings = await _httpLocalizableStringService.GetAllAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await _notificationService.NotifyAsync(new Notification
                        {
                            Title = _localizationAppService.Resolve("Error"),
                            Message = _localizationAppService.Resolve("Error_during_server_communication"),
                            Level = NotificationLevel.Error
                        });

                        try
                        {
                            strings = await _fileSystemLocalizableStringService.GetAllAsync();

                            await _notificationService.NotifyAsync(new Notification
                            {
                                Title = _localizationAppService.Resolve("Warning"),
                                Message = _localizationAppService.Resolve("Strings_from_file_system"),
                                Level = NotificationLevel.Warning
                            });
                        }
                        catch(Exception innerException)
                        {
                            Console.WriteLine(innerException.Message);
                            await _notificationService.NotifyAsync(new Notification
                            {
                                Title = _localizationAppService.Resolve("Error"),
                                Message = _localizationAppService.Resolve("Impossible_to_retrieve_strings"),
                                Level = NotificationLevel.Error
                            });
                        }
                    }
                }
                else
                    strings = await _fileSystemLocalizableStringService.GetAllAsync();
            }
            finally
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
            }

            return strings;
        }

        async public Task SaveAsync(IEnumerable<LocalizableString> strings)
        {
            _eventAggregator.GetEvent<BusyChangedEvent>().Publish(true);
            try
            {
                if (_checkConnectionService.IsConnectionAvailable())
                {
                    try
                    {
                        await _httpLocalizableStringService.SaveAsync(strings);
                        await _fileSystemLocalizableStringService.SaveAsync(strings);

                        await _notificationService.NotifyAsync(new Notification
                        {
                            Title = _localizationAppService.Resolve("Information"),
                            Message = _localizationAppService.Resolve("Operation_successfully_completed"),
                            Level = NotificationLevel.Info
                        });
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await _notificationService.NotifyAsync(new Notification
                        {
                            Title = _localizationAppService.Resolve("Error"),
                            Message = _localizationAppService.Resolve("Error_during_server_communication"),
                            Level = NotificationLevel.Error
                        });

                        try
                        {
                            await _fileSystemLocalizableStringService.SaveAsync(strings);
                           
                            await _notificationService.NotifyAsync(new Notification
                            {
                                Title = _localizationAppService.Resolve("Warning"),
                                Message = _localizationAppService.Resolve("Strings_saved"),
                                Level = NotificationLevel.Warning
                            });
                        }
                        catch(Exception innerException)
                        {
                            Console.WriteLine(innerException.Message);
                            await _notificationService.NotifyAsync(new Notification
                            {
                                Title = _localizationAppService.Resolve("Error"),
                                Message = _localizationAppService.Resolve("Impossible_to_save_strings"),
                                Level = NotificationLevel.Error
                            });
                        }
                    }
                }
                else
                {
                    await _fileSystemLocalizableStringService.SaveAsync(strings);

                    await _notificationService.NotifyAsync(new Notification
                    {
                        Title = _localizationAppService.Resolve("Warning"),
                        Message = _localizationAppService.Resolve("Strings_saved"),
                        Level = NotificationLevel.Warning
                    });
                }
            }
            finally
            {
                _eventAggregator.GetEvent<BusyChangedEvent>().Publish(false);
            }
        }
    }
}
