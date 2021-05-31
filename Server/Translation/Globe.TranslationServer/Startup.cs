using AutoMapper;
using FluentValidation.AspNetCore;
using Globe.BusinessLogic.Repositories;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Globe.Infrastructure.EFCore.Repositories;
using Globe.Shared.Services;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Hubs;
using Globe.TranslationServer.Repositories;
using Globe.TranslationServer.Services;
using Globe.TranslationServer.Services.PortingAdapters;
using Globe.TranslationServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Globe.TranslationServer
{
    public class Startup
    {
        IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            services
                .AddOptions()
                .Configure<JwtAuthenticationOptions>(options => _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options))
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            // Database
            services
                .AddDbContext<LocalizationContext>(options =>
                {
                    if (!options.IsConfigured)
                    {
                        options.UseSqlServer(
                            _configuration.GetConnectionString(Constants.DEFAULT_CONNECTION_STRING),
                            options => options.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds))
                        .EnableSensitiveDataLogging();
                    }
                });

            // Services
            services
                .AddSingleton<IAsyncLocalizableStringService, FakeLocalizableStringService>();
            services
                .AddScoped<LocalizableStringRepository, LocalizableStringRepository>();

            // Notifications
            services
                .AddSingleton<IAsyncNotificationService, NotificationService>();

            // Repositories
            services
                .AddScoped<IReadRepository<LocLanguage>, GenericRepository<LocalizationContext, LocLanguage>>()
                .AddScoped<IReadRepository<VLocalization>, GenericRepository<LocalizationContext, VLocalization>>()
                .AddScoped<IReadRepository<VConceptStringToContext>, GenericRepository<LocalizationContext, VConceptStringToContext>>()
                .AddScoped<IReadRepository<VStringsToContext>, GenericRepository<LocalizationContext, VStringsToContext>>()
                .AddScoped<IReadRepository<VString>, GenericRepository<LocalizationContext, VString>>()
                .AddScoped<IReadRepository<VJobListConcept>, GenericRepository<LocalizationContext, VJobListConcept>>()
                .AddScoped<IReadRepository<VTranslatedConcept>, GenericRepository<LocalizationContext, VTranslatedConcept>>()
                .AddScoped<IAsyncReadRepository<LocConceptsTable>, AsyncGenericRepository<LocalizationContext, LocConceptsTable>>()
                .AddScoped<IAsyncReadRepository<LocJobList>, AsyncGenericRepository<LocalizationContext, LocJobList>>();

            // Services
            services
                .AddScoped<IXmlToDBService, Services.NewServices.XmlToDBService>()
                .AddScoped<IXmlToDbMergeService, XmlToDbMergeService>()               
                .AddScoped<ILocalizationResourceBuilder, Services.NewServices.ScopedLocalizationResourceBuilder>()
                .AddScoped<IAsyncLanguageService, Services.NewServices.LanguageService>()
                .AddScoped<IComponentNamespaceService, Services.NewServices.ComponentNamespaceService>()
                .AddScoped<IAsyncInternalNamespaceService, Services.NewServices.InternalNamespaceService>()
                .AddScoped<IAsyncConceptTranslatedComponentNamespaceService, Services.NewServices.ConceptTranslatedComponentNamespaceService>()
                .AddScoped<IAsyncConceptTranslatedInternalNamespaceService, Services.NewServices.ConceptTranslatedInternalNamespaceService>()
                .AddScoped<ITranslatedConceptService, Services.NewServices.ConceptTranslatedService>()
                .AddScoped<IAsyncJobItemService, Services.NewServices.JobItemService>()
                .AddScoped<IDBToXmlService, Services.NewServices.DBToXmlService>()

                .AddScoped<IAsyncConceptSearchService, ConceptSearchService>()
                .AddScoped<IAsyncContextService, ContextService>()
                .AddScoped<IAsyncStringTypeService, StringTypeService>()
                .AddScoped<IAsyncStringViewProxyService, StringViewProxyService>()
                .AddScoped<IJobListService, Services.NewServices.JobListService>()
                .AddScoped<IAsyncXmlGroupedStringEntityService, Services.NewServices.XmlGroupedStringEntityService>()
                .AddScoped<IAsyncXmlDefinitionReaderService, XmlDefinitionReaderService>()
                .AddScoped<IAsyncConceptDetailsService, ConceptDetailsAdapterService>()
                .AddScoped<IAsyncConceptService, ConceptService>()
                .AddScoped<IAsyncNotTranslatedConceptViewService, Services.NewServices.NotTranslatedConceptViewService>()
                .AddScoped<IComponentNamespaceGroupService, Services.NewServices.ComponentNamespaceGroupService>()
                .AddScoped<IAsyncJobListService, JobListService>()
                .AddScoped<IAsyncXmlZipService, XmlZipService>()
                .AddScoped<IExportDbFilterService, Services.NewServices.ExportDbFilterService>()

                .AddScoped<ILogService, ConsoleLogService>();

            // Security
            services
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>()
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {                  
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/NotificationHub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAutoMapper(typeof(Startup));

            services
                .AddControllers()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/NotificationHub");
                endpoints.MapControllers();
            });
        }
    }
}
