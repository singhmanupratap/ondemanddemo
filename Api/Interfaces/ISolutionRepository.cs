using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Api.Models;

namespace Interfaces
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
