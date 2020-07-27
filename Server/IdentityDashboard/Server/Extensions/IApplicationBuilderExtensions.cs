using Globe.Identity.AdministrativeDashboard.Server.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        async public static Task ApplyMigrationsAsync(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
                await dbContext.Database.MigrateAsync();
        }
    }
}
