using System;

namespace vyber_api.Data
{
    public class CosmosDbSettings
    {
        public string Account { get; set; }
        public string DatabaseName { get; set; }
        public string Key { get; set; }
        public CosmosDbContainers Containers { get; set; }
    }
}
