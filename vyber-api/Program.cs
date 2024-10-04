using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using DotNetEnv;
using vyber_api.Models;
using SpotifyAPI.Web;
using vyber_api.Data;
namespace vyber_api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            Env.Load(".env"); // adjust the path if necessary

            // Test Spotify connection
            // var spotifyAuthApp = new Data.SpotifyAuthApp();
            // await spotifyAuthApp.StartAuthenticationProcess(); // Call the Spotify authentication method


            // Create the host and run the application
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Load environment variables
                var uri = Environment.GetEnvironmentVariable("URI");
                var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
                var primaryKey = Environment.GetEnvironmentVariable("PRIMARY_KEY");
                var primaryConnectionString = Environment.GetEnvironmentVariable("PRIMARY_CONNECTION_STRING");

                var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
                var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");

                if (string.IsNullOrEmpty(uri) && string.IsNullOrEmpty(databaseName) && string.IsNullOrEmpty(primaryKey) && string.IsNullOrEmpty(primaryConnectionString) )
                {
                    throw new Exception("Environment Variables not found. Ensure the .env file is correctly configured and placed in the root directory.");
                }

                // Add the connection string to configuration
                var settings = new Dictionary<string, string>
                {
                    { "CosmosDb:Account", uri },                       // Add URI
                    { "CosmosDb:DatabaseName", databaseName },      // Add Database Name
                    { "CosmosDb:Key", primaryKey },          // Add Primary Key
                    { "CosmosDb:PrimaryConnectionString", primaryConnectionString }  // Add Primary Connection String

                };

                config.AddInMemoryCollection(settings);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
        });
    }
}