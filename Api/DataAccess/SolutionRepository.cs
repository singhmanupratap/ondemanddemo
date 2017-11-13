using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Interfaces;
using System;
using Newtonsoft.Json;
using System.Linq;
using Api.Models;

namespace DataAccess
{
    public class SolutionRepository : ISolutionRepository
    {
        public SolutionRepository()
        {
        }


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
    }
}
