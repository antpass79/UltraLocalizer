using Microsoft.EntityFrameworkCore;
using System;

namespace Globe.TranslationServer.Data
{
    public class LocalizationDbContext : DbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
            : base(options)
        {
        }
    }
}
