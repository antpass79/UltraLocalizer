using Globe.TranslationServer.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services
{
    public class FileWatcherService : IHostedService, IDisposable
    {
        private FileSystemWatcher _fileSystemWatcher;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FileWatcherService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;          
        }

        public void Dispose()
        {
            _fileSystemWatcher?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
        
            InitializeFileWatcher();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _fileSystemWatcher.Changed -= OnChanged;
            _fileSystemWatcher.Created -= OnChanged;

            _fileSystemWatcher.Dispose();

            return Task.CompletedTask;
        }

        private void InitializeFileWatcher()
        {
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), Constants.XML_FOLDER),
                NotifyFilter =
                    NotifyFilters.LastWrite |
                    NotifyFilters.CreationTime,
                Filter = "*.xml",
                EnableRaisingEvents = true
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Created += OnChanged;
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var conceptService = scope.ServiceProvider.GetRequiredService<IAsyncConceptService>();
                await conceptService.CheckNewConceptsAsync();
            }
        }
    }
}
