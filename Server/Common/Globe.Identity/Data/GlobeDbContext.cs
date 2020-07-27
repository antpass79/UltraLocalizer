using Globe.Identity.Models;
using Globe.Identity.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Data
{
    public abstract class GlobeDbContext : IdentityDbContext<GlobeUser>
    {
        protected IOptions<DatabaseOptions> Options { get; }
        protected GlobeDbContext(IOptions<DatabaseOptions> options)
        {
            Options = options;
        }
    }
}
