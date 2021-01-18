using Globe.BusinessLogic.Repositories;
using Globe.Shared.Services;
using Globe.Shared.Utilities;
using Globe.TranslationServer.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.TranslationServer.Services.NewServices
{
    public class DBToXmlService : IDBToXmlService
    {
        private readonly IReadRepository<VLocalization> _localizationViewRepository;
        private readonly IReadRepository<LocLanguage> _languageRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogService _logService;

        public DBToXmlService(
            IReadRepository<VLocalization> localizationViewRepository,
            IReadRepository<LocLanguage> languageRepository,
            IServiceScopeFactory serviceScopeFactory,
            ILogService logService)
        {
            _localizationViewRepository = localizationViewRepository;
            _languageRepository = languageRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _logService = logService;
        }

        async public Task Generate(string outputFolder, bool debugMode = true)
        {
            var components = _localizationViewRepository
                .Get()
                .GroupBy(item => item.ConceptComponentNamespace)
                .Select(item => item.First())
                .OrderBy(item => item.ConceptComponentNamespace);

            var languages = _languageRepository.Get();

            Parallel.ForEach(components, (component) =>
            {
                try
                {
                    if (
                    component.ConceptComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_ALL &&
                    component.ConceptComponentNamespace != SharedConstants.COMPONENT_NAMESPACE_OLD)
                    {
                        Parallel.ForEach(languages, (language) =>
                        {
                            try
                            {
                                using (var scope = _serviceScopeFactory.CreateScope())
                                {
                                    var localizationResource = scope.ServiceProvider.GetRequiredService<ILocalizationResourceBuilder>()
                                        .Component(component)
                                        .Language(language)
                                        .DebugMode(debugMode)
                                        .Build();

                                    localizationResource.Save(Path.Combine(outputFolder, $"{localizationResource.ComponentNamespace}.{localizationResource.Language}.xml"));
                                }
                            }
                            catch (Exception innerException)
                            {
                                _logService.Exception(innerException);
                            }
                        });
                    }
                }
                catch (Exception outerException)
                {
                    _logService.Exception(outerException);
                }
            });

            await Task.CompletedTask;
        }
    }
}
