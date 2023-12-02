using aspnetcore_web_api_cosmosdb.Repositories;
using Microsoft.Azure.Cosmos;

namespace aspnetcore_web_api_cosmosdb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddSingleton((provider) =>
            {
                var endpointUri = configuration["CosmosDbConnectionSettings:EndpointUri"];
                var primaryKey = configuration["CosmosDbConnectionSettings:PrimaryKey"];
                var databaseName = configuration["CosmosDbConnectionSettings:DatabaseName"];

                var cosmosClientOptions = new CosmosClientOptions
                {
                    ApplicationName = databaseName,
                    ConnectionMode = ConnectionMode.Direct
                };

                var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);

                return cosmosClient;
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}