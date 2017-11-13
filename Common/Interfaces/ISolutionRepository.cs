using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Interfaces
{
    public interface ISolutionRepository
    {
        Task AddInSolutionQueue(Solution solution, string newTaskId);
        Task<List<Solution>> GetSolutions();
        Task<Solution> GetSolution(string id);
        Task<string> GetBuildStatus(string id);
        Task<bool> AddSolution(Solution solution);
    }
}
