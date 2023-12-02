using aspnetcore_web_api_cosmosdb.Constants;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace aspnetcore_web_api_cosmosdb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Container _container;

        public UserRepository(CosmosClient cosmosClient,
            IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDbConnectionSettings:DatabaseName"];
            var userContainerName = ContainerNames.Users;
            _container = cosmosClient.GetContainer(databaseName, userContainerName);
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            var response = await _container.CreateItemAsync(user);

            return response.Resource;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _container.DeleteItemAsync<Models.User>(userId, new PartitionKey(userId));
        }

        public async Task<Models.User?> GetUserByIdAsync(string userId)
        {
            var query = _container.GetItemLinqQueryable<Models.User>()
                .Where(x => x.Id == userId)
                .ToFeedIterator();

            var response = await query.ReadNextAsync();

            return response.SingleOrDefault();
        }

        public async Task<List<Models.User>> GetUsersAsync()
        {
            var query = _container.GetItemLinqQueryable<Models.User>()
                .ToFeedIterator();

            var users = new List<Models.User>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                users.AddRange(response);
            }

            return users;
        }

        public async Task<Models.User> UpdateUserAsync(Models.User user)
        {
            var response = await _container.ReplaceItemAsync(user, user.Id);

            return response.Resource;
        }
    }
}
