using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace CPWorkout
{
    public interface ICosmosService
    {
        public Task<T?> AddItem<T>(T item, string partitionKey);

        public Task<T?> GetItem<T>(string id, string partitionKey);

        public Task<T?> UpdateItem<T>(T item, string partitionKey);

        public Task<T?> DeleteItem<T>(string id, string partitionKey);
    }
    public class CosmosService : ICosmosService
    {
        private readonly CosmosClient _client;
        private AppConfig Configuration { get; set; }

        private Container _container => _client.GetDatabase(Configuration.database).GetContainer(Configuration.container);

        public CosmosService(IOptions<AppConfig> options)
        {
            Configuration = options.Value;
            _client = new CosmosClient(connectionString: Configuration.connectionString);
        }

        public async Task<T?> AddItem<T>(T item, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await _container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
                HttpStatusCode status = response.StatusCode;
                if (status == HttpStatusCode.Created)
                {
                    var createdItem = response.Resource;
                    return createdItem;
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Conflict Error - " + ex.Message);
                throw;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine("Unknown Exception - " + ex.Message);
                throw;
            }
            return default(T);
        }

        public async Task<T?> GetItem<T>(string id, string partitionKey)
        {
            try
            {
                var fetchedItem = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                if (fetchedItem != null)
                {
                    return fetchedItem;
                }
                else
                {
                    throw new InvalidOperationException("Given key is invalid");
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine("Unknown Exception - " + ex.Message);
                throw;
            }
        }

        public async Task<T?> UpdateItem<T>(T item, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await _container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey));
                HttpStatusCode status = response.StatusCode;
                if (status == HttpStatusCode.OK)
                {
                    var createdItem = response.Resource;
                    return createdItem;
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                Console.WriteLine("Conflict Error - " + ex.Message);
                throw;
            }
            catch (CosmosException ex)
            {
                Console.WriteLine("Unknown Exception - " + ex.Message);
                throw;
            }
            return default(T);
        }

        public async Task<T?> DeleteItem<T>(string id, string partitionKey)
        {
            try
            {
                var deletededItem = await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
                if (deletededItem != null && deletededItem.StatusCode == HttpStatusCode.NoContent)
                {
                    return deletededItem.Resource;
                }
                else
                {
                    throw new InvalidOperationException("Given key is invalid");
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine("Unknown Exception - " + ex.Message);
                throw;
            }
        }
    }
}
