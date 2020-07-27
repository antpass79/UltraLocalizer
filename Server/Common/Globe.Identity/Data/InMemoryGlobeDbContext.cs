using Globe.Identity.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Data
{
    public class InMemoryGlobeDbContext : GlobeDbContext
    {
        public InMemoryGlobeDbContext(IOptions<DatabaseOptions> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(this.Options.Value.DefaultInMemoryConnection);
        }
    }
}
