using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using StockAPI.Models;

namespace StockAPI.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Stock item)
        {
            await this._container.CreateItemAsync<Stock>(item, new PartitionKey(item.ID));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Stock>(id, new PartitionKey(id));
        }

        public async Task<Stock> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Stock> response = await this._container.ReadItemAsync<Stock>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Stock>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Stock>(new QueryDefinition(queryString));
            List<Stock> results = new List<Stock>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Stock item)
        {
            await this._container.UpsertItemAsync<Stock>(item, new PartitionKey(id));
        }
    }
}
