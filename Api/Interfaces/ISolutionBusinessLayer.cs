using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Models;

namespace Interfaces
{
    public interface ISolutionBusinessLayer
    {
        ISolutionRepository SolutionRepository { get; set; }

        
        Task<List<Solution>> GetSolutionsAsync();
        Task<Solution> GetSolutionAsync(string id);
        Task<bool> AddSolutionAsync(Solution solution);
        Task<List<Build>> GetBuildsAsync();
        Task<Build> GetBuildAsync(string id);

        Task<Build> BuildSolutionAsync(Build build);
        Task<Build> UpdateBuildAsync(Build build, string id);
        Task<Build> CompleteBuildAsync(Build build, string buildId);
        
    }
}