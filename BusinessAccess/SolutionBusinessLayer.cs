using Common.Interfaces;
using Common.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SolutionBusinessLayer : ISolutionBusinessLayer
    {
        public ISolutionRepository SolutionRepository { get; set; }

        public SolutionBusinessLayer()
        {
            SolutionRepository = new SolutionRepository();
        }

        public async Task<string> BuildSolution(string id)
        {
            var newTaskId = Guid.NewGuid().ToString();
           

            var solution = await SolutionRepository.GetSolution(id);


            await SolutionRepository.AddInSolutionQueue(solution, newTaskId);

            var task = Task.Run<string>(() => { return newTaskId; });

            return await task;
        }

        public Task<Solution> GetSolution(string id)
        {
            return SolutionRepository.GetSolution(id);
        }

        public async Task<List<Solution>> GetSolutions()
        {
            return await SolutionRepository.GetSolutions();
        }

        public async Task<string> GetBuildStatus(string id)
        {
           return await SolutionRepository.GetBuildStatus(id);
        }

        public async Task<bool> AddSolution(Solution solution)
        {
            return await SolutionRepository.AddSolution(solution);
        }
    }
}
