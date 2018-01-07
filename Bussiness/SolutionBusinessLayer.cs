﻿using Common.Models;
using DataAccess;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Bussiness
{
    public class SolutionBusinessLayer : ISolutionBusinessLayer
    {
        public ISolutionRepository SolutionRepository { get; set; }

        public SolutionBusinessLayer(ISolutionRepository solutionRepository)
        {
            SolutionRepository = solutionRepository;
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
                Status = (int)BuildStatus.BuildQueued,
                AzureAccountOwnerName = newBuild.AzureAccountOwnerName,
                AzureSubscriptionId=newBuild.AzureSubscriptionId,
                AzureTenantId=newBuild.AzureTenantId,
                DeploymentName= newBuild.DeploymentName,
                PresetAzureDirectoryName=newBuild.PresetAzureDirectoryName,
                PresetAzureLocationName=newBuild.PresetAzureLocationName,
                PresetAzureSubscriptionName=newBuild.PresetAzureSubscriptionName,
                ServicePrincipalId=newBuild.ServicePrincipalId,
                ServicePrincipalPassword=newBuild.ServicePrincipalPassword,
                VmAdminPassword=newBuild.VmAdminPassword
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
                if (build.Status == (int)BuildStatus.BuildQueued)
                {
                    updateBuild.VSTSBuildId = build.VSTSBuildId;
                }
                if (build.Status == (int)BuildStatus.DeploymentQueued)
                {
                    updateBuild.TemplateParameterUri = build.TemplateParameterUri;
                    updateBuild.TemplateUri = build.TemplateUri;
                }
                updateBuild.PkgURL = build.PkgURL;
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
                updateBuild.TemplateParameterUri = build.TemplateParameterUri;
                updateBuild.TemplateUri = build.TemplateUri;
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
                    PresetAzureSubscriptionName = updateBuild.PresetAzureSubscriptionName,
                    TemplateParameterUri = updateBuild.TemplateParameterUri,
                    TemplateUri = updateBuild.TemplateUri
                };

                await SolutionRepository.AddBuildInQueueAsync(queueObject);
                return await SolutionRepository.GetBuildAsync(updateBuild.RowKey);
            }
            return updateBuild;
        }

        public Task<Build> DeployAsync(Build build, string id)
        {
            throw new NotImplementedException();
        }

        public Task<Build> DeployCompleteAsync(Build build, string id)
        {
            throw new NotImplementedException();
        }
    }
}