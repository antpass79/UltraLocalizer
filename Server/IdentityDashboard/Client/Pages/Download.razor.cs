using MyLabLocalizer.IdentityDashboard.Client.Components;
using MyLabLocalizer.IdentityDashboard.Client.Services;
using MyLabLocalizer.IdentityDashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLabLocalizer.IdentityDashboard.Client.Pages
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