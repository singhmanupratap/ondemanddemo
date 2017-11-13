using Api.Models;
using Common.Models;
using DataAccess;
using Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
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

        public async Task<Build> BuildSolutionAsync(Build newBuild)
        {
            var buildId = Guid.NewGuid().ToString();
            var solution = await SolutionRepository.GetSolutionAsync(newBuild.SolutionId);
            var build = new Build
            {
                RowKey = buildId,
                Status = (int)BuildStatus.BuildQueued,
                SolutionId = solution.RowKey,
                PartitionKey = solution.Name,
                DeploymentName = newBuild.DeploymentName,
                PresetAzureLocationName = newBuild.PresetAzureLocationName,
                PresetAzureAccountName = newBuild.PresetAzureAccountName,
                PresetAzureSubscriptionName = newBuild.PresetAzureSubscriptionName,
                PresetAzureDirectoryName  = newBuild.PresetAzureDirectoryName
            };
            await SolutionRepository.AddBuildAsync(build);

            var queueObject = new QueueObject
            {
                Id = buildId,
                SolutionName = solution.Name,
                BuildTemplate = solution.BuildTemplate,
                Status = (int)BuildStatus.BuildQueued
            };

            await SolutionRepository.AddBuildInQueueAsync(queueObject);
            build = await SolutionRepository.GetBuildAsync(buildId);
            return build;
        }

        public Task<Solution> GetSolutionAsync(string id)
        {
            return SolutionRepository.GetSolutionAsync(id);
        }

        public async Task<List<Solution>> GetSolutionsAsync()
        {
            return await SolutionRepository.GetSolutionsAsync();
        }

        public async Task<bool> AddSolutionAsync(Solution solution)
        {
            return await SolutionRepository.AddSolutionAsync(solution);
        }

        public async Task<Build> GetBuildAsync(string id)
        {
            return await SolutionRepository.GetBuildAsync(id);
        }

        public async Task<List<Build>> GetBuildsAsync()
        {
            return await SolutionRepository.GetBuildsAsync();
        }

        public async Task<Build> UpdateBuildAsync(Build build, string id)
        {
            var updateBuild = await SolutionRepository.GetBuildAsync(id);
            if (updateBuild != null)
            {
                updateBuild.Timestamp = DateTime.Now;
                updateBuild.Status = build.Status;
                updateBuild.Description = build.Description;
                updateBuild.VSTSBuildId = build.VSTSBuildId;
                await SolutionRepository.UpdateBuildAsync(updateBuild);
                return await SolutionRepository.GetBuildAsync(id);
            }
            return updateBuild;
        }

        public async Task<Build> CompleteBuildAsync(Build build, string buildId)
        {
            var updateBuild = await SolutionRepository.GetBuildByVSTSBuildAsync(buildId);
            if (updateBuild != null)
            {
                var solution = await SolutionRepository.GetSolutionAsync(updateBuild.SolutionId);
                updateBuild.Timestamp = DateTime.Now;
                updateBuild.Status = build.Status;
                updateBuild.Description = build.Description;
                updateBuild.VSTSBuildId = build.VSTSBuildId;
                updateBuild.PkgURL = build.PkgURL;
                await SolutionRepository.UpdateBuildAsync(updateBuild);

                var queueObject = new QueueObject
                {
                    Id = updateBuild.RowKey,
                    SolutionName = solution.Name,
                    BuildTemplate = solution.BuildTemplate,
                    Status = (int)BuildStatus.BuildCompleted,
                    PkgURL = updateBuild.PkgURL,
                    DeploymentName = updateBuild.DeploymentName,
                    BuildId = buildId,
                    PresetAzureLocationName = updateBuild.PresetAzureLocationName,
                    PresetAzureAccountName = updateBuild.PresetAzureAccountName,
                    PresetAzureDirectoryName = updateBuild.PresetAzureDirectoryName,
                    PresetAzureSubscriptionName = updateBuild.PresetAzureSubscriptionName
                };

                await SolutionRepository.AddBuildInQueueAsync(queueObject);
                return await SolutionRepository.GetBuildAsync(updateBuild.RowKey);
            }
            return updateBuild;
        }
    }
}
