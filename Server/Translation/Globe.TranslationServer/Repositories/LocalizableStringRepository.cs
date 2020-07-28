using Globe.Infrastructure.EFCore.Repositories;
using Globe.TranslationServer.Entities;

namespace Globe.TranslationServer.Repositories
{
    public class LocalizableStringRepository : AsyncGenericRepository<LocalizationContext, LocalizableStringRepository>
    {
        public LocalizableStringRepository(LocalizationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
