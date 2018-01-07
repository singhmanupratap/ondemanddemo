using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Models;

namespace Common.Interfaces
{
    public interface ISolutionBusinessLayer2
    {
        ISolutionRepository SolutionRepository { get; set; }

        Task<string> BuildSolution(string id);
        Task<List<Solution>> GetSolutions();
        Task<Solution> GetSolution(string id);
        Task<bool> AddSolution(Solution solution);
    }
}