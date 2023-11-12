using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI6.Data
{
    public class FoodStoreContext : IdentityDbContext<ApplicationUser>
    {
        public FoodStoreContext(DbContextOptions<FoodStoreContext> opt) : base(opt)
        {

        }

        #region DbSet
        public DbSet<Food>? Foods { get; set; }
        #endregion
    }
}
