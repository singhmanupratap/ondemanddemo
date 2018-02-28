using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Utilities.Models;

namespace Common.Interfaces
{
    public interface ISolutionRepository
    {
        Task<List<Solution>> GetSolutionsAsync();
        Task<Solution> GetSolutionAsync(string id);
        Task<bool> AddSolutionAsync(Solution solution);
        Task AddBuildAsync(Build build);
        Task AddBuildInQueueAsync(QueueObject queueObject);
        Task<List<Build>> GetBuildsAsync();
        Task<Build> GetBuildAsync(string id);
        Task UpdateBuildAsync(Build build);
        Task<Build> GetBuildByVSTSBuildAsync(string buildId);
        bool ClearUserTokens(string user);
        UserTokenCache GetTokenByWebUserUniqueId(string user);
        List<UserTokenCache> GetTokensByWebUserUniqueId(string user);
        bool UpdateToken(UserTokenCache cache);
        Task<List<Subscription>> GetSubscriptionsByUser(string userId);
        Task<UserSession> GetUserSessionByIdAsync(string sessionId);
        Task<UserSession> UpdateUserSessionAsync(UserSession existingSession);
        Task<UserSession> AddUserSessionAsync(UserSession session);
        Task<bool> UpdateSubscriptionAsync(Subscription subscription);
        Task<Subscription> GetSubscriptionAsync(string id, string connectedBy);
        Task<bool> InsertSubscriptionAsync(Subscription subscription);
    }
}
