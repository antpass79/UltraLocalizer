using Globe.Infrastructure.EFCore.Repositories;
using MyLabLocalizer.LocalizationService.Entities;

namespace MyLabLocalizer.LocalizationService.Repositories
{
    public class LocalizableStringRepository : AsyncGenericRepository<LocalizationContext, LocalizableStringRepository>
    {
        public LocalizableStringRepository(LocalizationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
