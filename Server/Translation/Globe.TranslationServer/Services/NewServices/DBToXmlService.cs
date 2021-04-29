﻿using Globe.BusinessLogic.Repositories;
using Globe.Shared.DTOs;
using Globe.Shared.Services;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class DBToXmlService : IDBToXmlService
    {
        private readonly IReadRepository<VLocalization> _localizationViewRepository;
        private readonly IReadRepository<LocLanguage> _languageRepository;
        private readonly IExportDbFilterService _exportDbFilterService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogService _logService;

        public DBToXmlService(
            IReadRepository<VLocalization> localizationViewRepository,
            IReadRepository<LocLanguage> languageRepository,
            IExportDbFilterService exportDbFilterService,
            IServiceScopeFactory serviceScopeFactory,
            ILogService logService)
        {
            _localizationViewRepository = localizationViewRepository;
            _languageRepository = languageRepository;
            _exportDbFilterService = exportDbFilterService;
            _serviceScopeFactory = serviceScopeFactory;
            _logService = logService;
        }

        async public Task Generate(string outputFolder, ExportDbFilters exportDbFilters, bool debugMode = true)
        {
            var componentNamespaceGroups = _exportDbFilterService.GetComponentNamespaceGroups(exportDbFilters);
            var languages = _exportDbFilterService.GetLanguages(exportDbFilters);

            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(componentNamespaceGroups, (componentNamespaceGroup) =>
            {
                try
                {
                    if (
                    componentNamespaceGroup.ComponentNamespace.Description != SharedConstants.COMPONENT_NAMESPACE_ALL &&
                    componentNamespaceGroup.ComponentNamespace.Description != SharedConstants.COMPONENT_NAMESPACE_OLD)
                    {
                        Parallel.ForEach(languages, (language) =>
                        {
                            try
                            {
                                using var scope = _serviceScopeFactory.CreateScope();
                                var localizationResource = scope.ServiceProvider.GetRequiredService<ILocalizationResourceBuilder>()
                                    .ComponentNamespaceGroup(componentNamespaceGroup)
                                    .Language(language)
                                    .DebugMode(debugMode)
                                    .Build();

                                localizationResource.Save(Path.Combine(outputFolder, $"{localizationResource.ComponentNamespace}.{localizationResource.Language}.xml"));
                            }
                            catch (Exception innerException)
                            {
                                _logService.Exception(innerException);
                                exceptions.Enqueue(innerException);
                            }
                        });
                    }
                }
                catch (Exception outerException)
                {
                    _logService.Exception(outerException);
                    exceptions.Enqueue(outerException);
                }
            });

            if (!exceptions.IsEmpty)
                throw new AggregateException(exceptions);

            await Task.CompletedTask;
        }
    }
}
