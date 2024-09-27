using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using vyber_api.Data;
using vyber_api.Models;
using vyber_api.Services;
using vyber_api.Controllers;
using Newtonsoft.Json;

namespace vyber_api.Services
{
    public class CosmosDbService
    {
         private readonly CosmosClient _cosmosClient;
        private readonly Database _database;


        public CosmosDbService(IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            var settings = cosmosDbSettings.Value;

            _cosmosClient = new CosmosClient(settings.Account, settings.Key);
            _database = _cosmosClient.GetDatabase(settings.DatabaseName);

            // if (string.IsNullOrEmpty(settings.Containers?.Customers?.ContainerName))
            // {
            //     throw new ArgumentException("Container names are not configured correctly.");
            // }

            // if (string.IsNullOrEmpty(settings.Containers?.Extensions?.ContainerName))
            // {
            //     throw new ArgumentException("Container names are not configured correctly.");
            // }
            // if (string.IsNullOrEmpty(settings.Containers?.Extensions?.ContainerName))
            // {
            //     throw new ArgumentException("Container names are not configured correctly.");
            // }

            // _customersContainer = _database.GetContainer(settings.Containers.Customers.ContainerName);
            // _extensionsContainer = _database.GetContainer(settings.Containers.Extensions.ContainerName);
            // _repoExtensionsContainer = _database.GetContainer(settings.Containers.RepoExtension.ContainerName);
        }
        }
}
