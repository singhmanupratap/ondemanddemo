using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
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
    }
}
