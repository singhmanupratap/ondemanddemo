using Common.Models;
using DataAccess;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Bussiness
{
    public class SolutionBusinessLayer : ISolutionBusinessLayer
    {
        public ISolutionRepository Repository { get; set; }

        public SolutionBusinessLayer(ISolutionRepository solutionRepository)
        {
            Repository = solutionRepository;
        }

        public async Task<Build> BuildSolutionAsync(Build newBuild)
        {
            var buildId = Guid.NewGuid().ToString();
            var solution = await Repository.GetSolutionAsync(newBuild.SolutionId);
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
                PresetAzureDirectoryName = newBuild.PresetAzureDirectoryName
            };
            await Repository.AddBuildAsync(build);

            var queueObject = new QueueObject
            {
                Id = buildId,
                SolutionName = solution.Name,
                BuildTemplate = solution.BuildTemplate,
                Status = (int)BuildStatus.BuildQueued,
                AzureAccountOwnerName = newBuild.AzureAccountOwnerName,
                AzureSubscriptionId = newBuild.AzureSubscriptionId,
                AzureTenantId = newBuild.AzureTenantId,
                DeploymentName = newBuild.DeploymentName,
                PresetAzureDirectoryName = newBuild.PresetAzureDirectoryName,
                PresetAzureLocationName = newBuild.PresetAzureLocationName,
                PresetAzureSubscriptionName = newBuild.PresetAzureSubscriptionName,
                ServicePrincipalId = ConfigurationManager.AppSettings["ClientID"],
                ServicePrincipalPassword = ConfigurationManager.AppSettings["Password"],
                VmAdminPassword = newBuild.VmAdminPassword,
                BuildDefinitionId = solution.BuildDefinitionId,
                BuildDefinitionUrl = solution.BuildDefinitionUrl,
                BuildDefinitionUserName = solution.BuildDefinitionUserName,
                BuildDefinitionPassword = solution.BuildDefinitionPassword
            };

            await Repository.AddBuildInQueueAsync(queueObject);
            build = await Repository.GetBuildAsync(buildId);
            await InitiateBuildAsync(queueObject).ConfigureAwait(false);
            return build;
        }

        public Task<Solution> GetSolutionAsync(string id)
        {
            return Repository.GetSolutionAsync(id);
        }

        public async Task<List<Solution>> GetSolutionsAsync()
        {
            return await Repository.GetSolutionsAsync();
        }

        public async Task<bool> AddSolutionAsync(Solution solution)
        {
            return await Repository.AddSolutionAsync(solution);
        }

        public async Task<Build> GetBuildAsync(string id)
        {
            return await Repository.GetBuildAsync(id);
        }

        public async Task<List<Build>> GetBuildsAsync()
        {
            return await Repository.GetBuildsAsync();
        }

        public async Task<Build> UpdateBuildAsync(Build build, string id)
        {
            var updateBuild = await Repository.GetBuildAsync(id);
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
                await Repository.UpdateBuildAsync(updateBuild);
                return await Repository.GetBuildAsync(id);
            }
            return updateBuild;
        }

        public async Task<Build> CompleteBuildAsync(Build build, string buildId)
        {
            var updateBuild = await Repository.GetBuildByVSTSBuildAsync(buildId);
            if (updateBuild != null)
            {
                var solution = await Repository.GetSolutionAsync(updateBuild.SolutionId);
                updateBuild.Timestamp = DateTime.Now;
                updateBuild.Status = build.Status;
                updateBuild.Description = build.Description;
                updateBuild.VSTSBuildId = build.VSTSBuildId;
                updateBuild.PkgURL = build.PkgURL;
                updateBuild.TemplateParameterUri = build.TemplateParameterUri;
                updateBuild.TemplateUri = build.TemplateUri;
                await Repository.UpdateBuildAsync(updateBuild);

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

                await Repository.AddBuildInQueueAsync(queueObject);
                return await Repository.GetBuildAsync(updateBuild.RowKey);
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

        public async Task<UserSession> UpdateUserSessionAsync(UserSession session)
        {
            var existingSession = await Repository.GetUserSessionByIdAsync(session.RowKey);
            if (existingSession != null)
            {
                if (!string.IsNullOrEmpty(session.SolutionId))
                {
                    existingSession.SolutionId = session.SolutionId;
                }

                if (!string.IsNullOrEmpty(session.UserId))
                {
                    existingSession.UserId = session.UserId;
                }

                if (!string.IsNullOrEmpty(session.SubscriptionId))
                {
                    existingSession.SubscriptionId = session.SubscriptionId;
                }

                if (!string.IsNullOrEmpty(session.TenantId))
                {
                    existingSession.TenantId = session.TenantId;
                }

                existingSession.ExpireTime = DateTime.Now.AddMinutes(70);
                session = await Repository.UpdateUserSessionAsync(existingSession);
            }
            else
            {
                session.ExpireTime = DateTime.Now.AddMinutes(70);
                session = await Repository.AddUserSessionAsync(session);
            }
            return session;
        }

        public async Task<List<Subscription>> GetSubscriptionsByUserAsync(string userId)
        {
            List<Subscription> connectedSubscriptions = await Repository.GetSubscriptionsByUser(userId);
            foreach (var connectedSubscription in connectedSubscriptions)
            {
                bool servicePrincipalHasReadAccessToSubscription = await AzureResourceManagerUtil.
                    DoesServicePrincipalHaveReadAccessToSubscription(connectedSubscription.Id, connectedSubscription.DirectoryId);
                connectedSubscription.AzureAccessNeedsToBeRepaired = !servicePrincipalHasReadAccessToSubscription;
            }
            return connectedSubscriptions;
        }

        public async Task<bool> UpdateSubscriptionAsync(Subscription subscription)
        {
            var existingSubscription = await Repository.GetSubscriptionAsync(subscription.Id, subscription.ConnectedBy);
            if (existingSubscription == null)
            {
                subscription.RowKey = Guid.NewGuid().ToString();
                subscription.PartitionKey = subscription.ConnectedBy;
                return await Repository.InsertSubscriptionAsync(subscription);
            }
            else
            {
                existingSubscription.DirectoryId = subscription.DirectoryId;
                existingSubscription.ConnectedOn = subscription.ConnectedOn;
                return await Repository.UpdateSubscriptionAsync(existingSubscription);
            }
        }

        public async Task<UserSession> GetUserSessionByIdAsync(string sessionId)
        {
            var session = await Repository.GetUserSessionByIdAsync(sessionId);
            return session;
        }

        private async Task<VSTSBuildResult> InitiateBuildAsync(QueueObject queueObject)
        {
            var TFSInstanceURL =  "https://vs-adityasingh.visualstudio.com";
            var ProjectCollection = "DefaultCollection";
            var TeamProject = "ondemanddemo";
            var Uri = TFSInstanceURL + "/" + ProjectCollection + "/" + TeamProject + "/_apis/build/builds?api-version=2.0";
            var User = queueObject.BuildDefinitionUserName ;// "";
            var Password = queueObject.BuildDefinitionPassword; // "mjreoriblyzpdiauv4hwct7oijadwqyzq7g5njv4dskz77y5ghza";
            var binaryData = new ASCIIEncoding().GetBytes(string.Format("{0}:{1}", User, Password));
            long arrayLength = (long)((4.0d / 3.0d) * binaryData.Length);
            if (arrayLength % 4 != 0)
            {
                arrayLength += 4 - arrayLength % 4;
            }
            char[] base64CharArray = new char[arrayLength];
            try
            {
                Convert.ToBase64CharArray(binaryData, 0, binaryData.Length, base64CharArray, 0);
                var token = new string(base64CharArray);
                var parameters = string.Empty;
                parameters = string.Format("'BuildRequestId':'{0}','AzureAccountOwnerName':'{1}','AzureSubscriptionId':'{2}','AzureTenantId':'{3}','DeploymentName':'{4}','PresetAzureDirectoryName':'{5}','PresetAzureLocationName':'{6}','PresetAzureSubscriptionName':'{7}','ServicePrincipalId':'{8}','ServicePrincipalPassword':'{9}','VmAdminPassword':'{10}'", queueObject.Id, queueObject.AzureAccountOwnerName, queueObject.AzureSubscriptionId, queueObject.AzureTenantId, queueObject.DeploymentName, queueObject.PresetAzureDirectoryName, queueObject.PresetAzureLocationName, queueObject.PresetAzureSubscriptionName, queueObject.ServicePrincipalId, queueObject.ServicePrincipalPassword, queueObject.VmAdminPassword);
                var build = new VSTSBuild
                {
                    definition = new PostDefinition
                    {
                        id = queueObject.BuildDefinitionId,
                    },
                    parameters = string.Concat("{", parameters , "}")
                };
                var headers = new Dictionary<string, string>();
                headers.Add("Authorization", string.Format("Basic {0}", token));
                return await HttpRequester.PostObject<VSTSBuild, VSTSBuildResult>(build, queueObject.BuildDefinitionUrl, headers);
                //$responseFromGet = Invoke - RestMethod - Method POST - Uri $Uri - ContentType $contentType - Headers $headers - Body $json;
            }
            catch
            {
            }

            return default(VSTSBuildResult);
        }
    }
}
