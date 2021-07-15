using Microsoft.EntityFrameworkCore;
using System;

namespace MyLabLocalizer.LocalizationService.Data
{
    public class LocalizationDbContext : DbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
            : base(options)
        {
        }
    }
}
