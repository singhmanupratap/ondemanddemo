using System.Collections.Generic;
using System.Data.Entity;
using WebApp.Models;

namespace WebApp
{
    public class DataAccess : DbContext
    {
        public DataAccess() : base("DataAccess") { }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<PerUserTokenCache> PerUserTokenCacheList { get; set; }
        //public IEnumerable<Subscription> Subscriptions { get; set; }
    }
    public class DataAccessInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DataAccess>
    {

    }
}