using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;

namespace CPWorkout
{
    public interface ICosmosService
    {
        public Task<T?> AddItem<T>(T item, string partitionKey);

        public Task<T?> GetItem<T>(string id, string partitionKey);
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
                System.Console.WriteLine("Conflict Error - " + ex.Message);
                throw;
            }
            catch (CosmosException ex)
            {
                System.Console.WriteLine("Unknown Exception - " + ex.Message);
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
                System.Console.WriteLine("Unknown Exception - " + ex.Message);
                throw;
            }
        }
    }
}
