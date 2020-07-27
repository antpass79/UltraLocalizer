using Globe.Identity.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Globe.Identity.Data
{
    public class SqlServerGlobeDbContext : GlobeDbContext
    {
        public SqlServerGlobeDbContext(IOptions<DatabaseOptions> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.Options.Value.DefaultSqlServerConnection);
        }
    }
}
