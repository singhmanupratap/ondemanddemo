using System.Data.Entity;
using Utilities.Models;

namespace DataAccess
{
    public class OnDemandDataContext : DbContext
    {
        public OnDemandDataContext() : base("DataAccess") { }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserTokenCache> PerUserTokenCacheList { get; set; }
    }
    public class DataAccessInitializer : DropCreateDatabaseIfModelChanges<OnDemandDataContext>
    {

    }
}