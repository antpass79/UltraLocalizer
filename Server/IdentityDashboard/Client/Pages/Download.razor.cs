using Globe.Identity.AdministrativeDashboard.Client.Components;
using Globe.Identity.AdministrativeDashboard.Client.Services;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Client.Pages
{
    public class DownloadDataModel : ComponentBase
    {
        [Inject]
        protected IApplicationService ApplicationService { get; set; }

        async protected Task Download()
        {
            await ApplicationService.Download();
        }
    }
}