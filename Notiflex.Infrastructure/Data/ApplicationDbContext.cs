using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notiflex.Infrastructure.Data.Models.UserModels;

namespace Notiflex.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<NotiflexUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}