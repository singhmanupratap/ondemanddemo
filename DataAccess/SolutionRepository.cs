using Common.Interfaces;
using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Models;

namespace DataAccess
{
    public class SolutionRepository : ISolutionRepository
    {
        public SolutionRepository()
        {
        }

        #region Table and Queue operations
        private static async Task<CloudTable> CreateCloudTableAsync(string tableName)
        {

            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            var task = await table.CreateIfNotExistsAsync();

            return table;
        }

        private static CloudTable CreateCloudTable(string tableName)
        {

            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            var tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            var task = table.CreateIfNotExists();

            return table;
        }

        private static async Task<CloudQueue> CreateCloudQueueClientAsync(string queue)
        {
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            var queueClient = storageAccount.CreateCloudQueueClient();


            // Retrieve a reference to a queue.
            var cloudQueueClient = queueClient.GetQueueReference(queue);

            // Create the queue if it doesn't already exist.
            await cloudQueueClient.CreateIfNotExistsAsync();

            return cloudQueueClient;
        } 
        #endregion

        #region Solution
        public async Task<List<Solution>> GetSolutionsAsync()
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Solution");
            TableContinuationToken continuationToken = null;
            var tableQuery = new TableQuery<Solution>();

            TableQuerySegment<Solution> tableQueryResult =
                await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

            return tableQueryResult.Results;
        }

        public async Task<Solution> GetSolutionAsync(string id)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Solution");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<Solution>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id));
            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        }

        public async Task<bool> AddSolutionAsync(Solution solution)
        {
            solution.RowKey = Guid.NewGuid().ToString();
            var table = await SolutionRepository.CreateCloudTableAsync("Solution");
            var operation = TableOperation.Insert(solution);
            var retrieveResult = await table.ExecuteAsync(operation);
            return true;
        } 
        #endregion

        #region Build

        public async Task AddBuildInQueueAsync(QueueObject queueObject)
        {
            var queue = await CreateCloudQueueClientAsync("solution-build-queue");
            string serializedMessage = JsonConvert.SerializeObject(queueObject);
            var cloudQueueMessage = new CloudQueueMessage(serializedMessage);
            await queue.AddMessageAsync(cloudQueueMessage);
        }
        public async Task AddBuildAsync(Build build)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Build");
            var operation = TableOperation.Insert(build);
            var result = await table.ExecuteAsync(operation);
        }

        public async Task<List<Build>> GetBuildsAsync()
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Build");
            TableContinuationToken continuationToken = null;
            var tableQuery = new TableQuery<Build>();

            TableQuerySegment<Build> tableQueryResult =
                await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

            return tableQueryResult.Results;
        }

        public async Task<Build> GetBuildAsync(string id)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Build");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<Build>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id));
            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        }

        public async Task UpdateBuildAsync(Build build)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Build");
            var tableOperation = TableOperation.Replace(build);
            await table.ExecuteAsync(tableOperation);
        }

        public async Task<Build> GetBuildByVSTSBuildAsync(string buildId)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Build");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<Build>().Where(TableQuery.GenerateFilterCondition("VSTSBuildId", QueryComparisons.Equal, buildId));
            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        } 
        #endregion

        #region User Tokens
        public bool ClearUserTokens(string user)
        {
            var table = CreateCloudTable("UserTokenCache");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<UserTokenCache>().Where(TableQuery.GenerateFilterCondition("WebUserUniqueId", QueryComparisons.Equal, user));
            var tableQueryResult = table.ExecuteQuerySegmented(buildQuery, continuationToken);
            foreach (var item in tableQueryResult.Results)
            {
                var tableOperation = TableOperation.Delete(item);
                table.Execute(tableOperation);
            }
            return true;
        }

        public UserTokenCache GetTokenByWebUserUniqueId(string webUserUniqueId)
        {
            var table = CreateCloudTable("UserTokenCache");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<UserTokenCache>().Where(TableQuery.GenerateFilterCondition("WebUserUniqueId", QueryComparisons.Equal, webUserUniqueId));
            var tableQueryResult = table.ExecuteQuerySegmented(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        }

        public List<UserTokenCache> GetTokensByWebUserUniqueId(string webUserUniqueId)
        {
            var table = CreateCloudTable("UserTokenCache");
            TableContinuationToken continuationToken = null;
            var tableQuery = new TableQuery<UserTokenCache>().Where(TableQuery.GenerateFilterCondition("WebUserUniqueId", QueryComparisons.Equal, webUserUniqueId));

            TableQuerySegment<UserTokenCache> tableQueryResult =
                table.ExecuteQuerySegmented(tableQuery, continuationToken);

            return tableQueryResult.Results;
        }

        public bool UpdateToken(UserTokenCache token)
        {
            token.RowKey = Guid.NewGuid().ToString();
            token.PartitionKey = token.WebUserUniqueId;
            var table = CreateCloudTable("UserTokenCache");
            var operation = TableOperation.Insert(token);
            var result = table.Execute(operation);
            return true;
        } 
        #endregion

        #region User Session
        public async Task<UserSession> GetUserSessionByIdAsync(string sessionId)
        {
            var table = await CreateCloudTableAsync("UserSession");
            TableContinuationToken continuationToken = null;
            var buildQuery = new TableQuery<UserSession>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, sessionId));
            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        }

        public async Task<UserSession> UpdateUserSessionAsync(UserSession session)
        {
            var table = await CreateCloudTableAsync("UserSession");
            var tableOperation = TableOperation.Replace(session);
            await table.ExecuteAsync(tableOperation);
            return await GetUserSessionByIdAsync(session.RowKey);
        }

        public async Task<UserSession> AddUserSessionAsync(UserSession session)
        {
            session.RowKey = Guid.NewGuid().ToString();
            session.PartitionKey = session.RowKey;
            var table = await CreateCloudTableAsync("UserSession");
            var operation = TableOperation.Insert(session);
            var retrieveResult = await table.ExecuteAsync(operation);
            return await GetUserSessionByIdAsync(session.RowKey);
        }

        public async Task<Subscription> GetSubscriptionAsync(string id, string connectedBy)
        {
            var table = await CreateCloudTableAsync("Subscription");
            TableContinuationToken continuationToken = null;
            var idFilter = TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, id);
            var connectedByFilter = TableQuery.GenerateFilterCondition("ConnectedBy", QueryComparisons.Equal, connectedBy);
            string combinedRowKeyFilter = TableQuery.CombineFilters(idFilter, TableOperators.And, connectedByFilter);
            var buildQuery = new TableQuery<Subscription>().Where(combinedRowKeyFilter);
            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(buildQuery, continuationToken);
            return tableQueryResult.Results.FirstOrDefault();
        }
        #endregion

        #region Subscriptions
        public Task<List<Subscription>> GetSubscriptionsByUser(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateSubscriptionAsync(Subscription subscription)
        {
            var table = await CreateCloudTableAsync("Subscription");
            var tableOperation = TableOperation.Replace(subscription);
            var res = await table.ExecuteAsync(tableOperation);
            return true;
            //return await GetUserSessionByIdAsync(subscription.Id, subscription.ConnectedBy);
        }

        public async Task<bool> InsertSubscriptionAsync(Subscription subscription)
        {
            var table = await CreateCloudTableAsync("Subscription");
            var tableOperation = TableOperation.Insert(subscription);
            var res = await table.ExecuteAsync(tableOperation);
            return true;
            //return await GetUserSessionByIdAsync(subscription.Id, subscription.ConnectedBy);
        }
        #endregion
    }
}