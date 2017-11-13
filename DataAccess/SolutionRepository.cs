using Common.Interfaces;
using System;
using Common.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading;

namespace DataAccess
{
    public class SolutionRepository : ISolutionRepository
    {
        public SolutionRepository()
        {
        }

        public async Task AddInSolutionQueue(Solution solution, string newTaskId)
        {
            var queue = await CreateCloudQueueClientAsync("SolutionQueue");

            // Create a message and add it to the queue.
            var message = new CloudQueueMessage(string.Format("{0}#{1}", solution.Name, newTaskId));
            await queue.AddMessageAsync(message);

            // Create a new SolutionQueue entity.
            var solutionQueue = new SolutionQueue
            {
                Id = newTaskId,
                SolutionId = solution.Id,
                BuildStatus = BuildStatus.InProcess
            };

            // Create the TableOperation object that inserts the customer entity.
            var insertOperation = TableOperation.Insert(solutionQueue);

            var table = await SolutionRepository.CreateCloudTableAsync("SolutionQueue");
            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
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

        public async Task<List<Solution>> GetSolutions()
        {
            var result = await GetTableResult("Solution");
            return result;
        }

        private static async Task<List<Solution>> GetTableResult(string tableName)
        {
            var table = await SolutionRepository.CreateCloudTableAsync(tableName);
                        TableContinuationToken continuationToken = null;
            var tableQuery = new TableQuery<Solution>();

            TableQuerySegment<Solution> tableQueryResult =
                await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

            return tableQueryResult.Results;
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            //var operation = TableOperation.Retrieve<Solution>(string.Empty, string.Empty);
            //var result = await table.ExecuteAsync(operation);
            //return result;
        }

        public async Task<Solution> GetSolution(string id)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Solution");
            var retrieveOperation = TableOperation.Retrieve<Solution>("", id);
            var retrieveResult = await table.ExecuteAsync(retrieveOperation);
            var task = Task<Solution>.Run(() => { return (Solution)retrieveResult.Result; });
            return await task;
        }

        public async Task<string> GetBuildStatus(string id)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("SolutionQueue");
            var retrieveOperation = TableOperation.Retrieve<SolutionQueue>("", id);
            var retrieveResult = await table.ExecuteAsync(retrieveOperation);
            var task = Task<SolutionQueue>.Run(() => { return ((SolutionQueue)retrieveResult.Result).BuildStatus.ToString(); });
            return await task;
        }

        public async Task<bool> AddSolution(Solution solution)
        {
            var table = await SolutionRepository.CreateCloudTableAsync("Solution");





            var retrieveOperation = TableOperation.Insert(solution);
            var retrieveResult = await table.ExecuteAsync(retrieveOperation);
            var task = Task<Solution>.Run(() => { return (bool)retrieveResult.Result; });
            return await task;
        }


    }
}
