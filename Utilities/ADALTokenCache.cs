using Common.Interfaces;
using DataAccess;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Data;
using System.Linq;
using Utilities.Models;

namespace Utilities
{
    public class ADALTokenCache : TokenCache
    {
        private ISolutionRepository repository;// db = new OnDemandDataContext();
        string User;
        UserTokenCache Cache;

        public ISolutionRepository Db { get => repository; set => repository = value; }

        // constructor
        public ADALTokenCache(string user)
        {
            Db = new SolutionRepository();
            // associate the cache to the current user of the web app
            User = user;
            AfterAccess = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            BeforeWrite = BeforeWriteNotification;
            // look up the entry in the DB
            Cache = Db.GetTokenByWebUserUniqueId(User);
            // place the entry in memory
            Deserialize((Cache == null) ? null : Cache.CacheBits);
        }

        // clean up the DB
        public override void Clear()
        {
            base.Clear();
            Db.ClearUserTokens(User);
        }

        // Notification raised before ADAL accesses the cache.
        // This is your chance to update the in-memory copy from the DB, if the in-memory version is stale
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            if (Cache == null)
            {
                // first time access
                Cache = Cache = new UserTokenCache
                {
                    WebUserUniqueId = User,
                };
            }
            else
            {   // retrieve last write from the DB
                var tokens = Db.GetTokensByWebUserUniqueId(User);
                var status = from token in tokens
                             select new 
                {
                     token.LastWrite
                };
                
                // if the in-memory copy is older than the persistent copy
                if (status.Count() > 0 && status.First().LastWrite > Cache.LastWrite)
                //// read from from storage, update in-memory copy
                {
                    Cache = Db.GetTokenByWebUserUniqueId(User);
                }
            }
            Deserialize((Cache == null) ? null : Cache.CacheBits);
        }

        internal bool GetToken()
        {
            throw new NotImplementedException();
        }

        // Notification raised after ADAL accessed the cache.
        // If the HasStateChanged flag is set, ADAL changed the content of the cache
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if state changed
            if (HasStateChanged)
            {
                // check for an existing entry
                Cache = Db.GetTokenByWebUserUniqueId(User);
                if (Cache == null)
                {
                    // if no existing entry for that user, create a new one
                    Cache = new UserTokenCache
                    {
                        WebUserUniqueId = User,
                    };
                }

                // update the cache contents and the last write timestamp
                Cache.CacheBits = this.Serialize();
                Cache.LastWrite = DateTime.Now;

                // update the DB with modification or new entry
               bool status = Db.UpdateToken(Cache);
               HasStateChanged = false;
            }
        }
        void BeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // if you want to ensure that no concurrent write take place, use this notification to place a lock on the entry
        }
    }
}