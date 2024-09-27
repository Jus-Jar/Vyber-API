using System;
using vyber_api.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace vyber_api.Data
{
    public class CosmosDbContainers
    {
        public ContainerSettings Songs { get; set; }
    }
}
