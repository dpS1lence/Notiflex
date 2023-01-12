using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notiflex.Infrastructure.Data.Models.UserModels;

namespace Notiflex.Infrastructure.Data
{
    public class NotiflexDbContext : IdentityDbContext<NotiflexUser>
    {
        public NotiflexDbContext(DbContextOptions<NotiflexDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}