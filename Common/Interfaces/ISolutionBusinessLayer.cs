using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ISolutionBusinessLayer
    {
// ISolutionRepository SolutionRepository { get; set; }
        Task<List<Solution>> GetSolutionsAsync();
        Task<Solution> GetSolutionAsync(string id);
        Task<bool> AddSolutionAsync(Solution solution);
        Task<List<Build>> GetBuildsAsync();
        Task<Build> GetBuildAsync(string id);
        Task<Build> BuildSolutionAsync(Build build);
        Task<Build> UpdateBuildAsync(Build build, string id);
        Task<Build> CompleteBuildAsync(Build build, string buildId);
        Task<Build> DeployAsync(Build build, string id);
        Task<Build> DeployCompleteAsync(Build build, string id);
    }
}