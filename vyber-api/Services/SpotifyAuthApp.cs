using System;
using System.Threading.Tasks;
using System.Net;
using SpotifyAPI.Web;
using DotNetEnv;

namespace vyber_api.Services
{
    public class SpotifyAuthApp
    {
        private SpotifyClient spotify;
        private string accessToken;

        // Step 1: Direct user to Spotify authorization page to grant permissions
        public async Task AuthenticateWithUser()
        {
            Env.Load(".env");

            var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
            var redirectUri = Environment.GetEnvironmentVariable("SPOTIFY_REDIRECT_URI");

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(redirectUri))
            {
                throw new Exception("Environment variables not found. Please ensure .env is correctly configured.");
            }

            // Create authorization URL
            var loginRequest = new LoginRequest(
                new Uri(redirectUri),
                clientId,
                LoginRequest.ResponseType.Code
            )
            {
                Scope = new[] { Scopes.UserReadPrivate, Scopes.PlaylistReadPrivate } // Add any other scopes you need
            };

            var authUri = loginRequest.ToUri();
            Console.WriteLine($"Please visit the following URL to authorize the app: {authUri}");
        }

        // Step 2: Exchange authorization code for an access token
        public async Task AuthenticateWithUser(string authCode)
        {
            Env.Load(".env");

            var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
            var redirectUri = Environment.GetEnvironmentVariable("SPOTIFY_REDIRECT_URI");

            var config = SpotifyClientConfig.CreateDefault();
            var tokenRequest = new AuthorizationCodeTokenRequest(clientId, clientSecret, authCode, new Uri(redirectUri));
            var response = await new OAuthClient(config).RequestToken(tokenRequest);

            // Now you have an access token and refresh token
            spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            Console.WriteLine("User authenticated successfully!");


        }

        // public void Logout()
        // {
        //     // Clear the access token
        //     accessToken = null; // or set it to an empty string
        //     spotify = null; // Clear the SpotifyClient instance
        // }

        // Fetch user's Spotify profile information
         public async Task<PrivateUser> GetUserProfile()
        {
            if (spotify == null)
            {
                throw new InvalidOperationException("Spotify client is not authenticated.");
            }

            var userProfile = await spotify.UserProfile.Current();
            return userProfile;
        }

        // Fetch user's Spotify playlists
        public async Task GetUserPlaylists()
        {
            var playlists = await spotify.Playlists.CurrentUsers();
            Console.WriteLine("Your Playlists:");
            foreach (var playlist in playlists.Items)
            {
                Console.WriteLine(playlist.Name);
            }
        }

        // Step 3: Handle the OAuth callback and get the authorization code
        public async Task StartAuthenticationProcess()
        {
            // First, direct the user to the Spotify authorization page
            await AuthenticateWithUser();

            // Now, wait for the redirect with the authorization code
            Console.WriteLine("Waiting for authorization code...");

            // Use HttpListener to handle the callback from Spotify
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:5000/callback/");
                listener.Start();

                var context = await listener.GetContextAsync();
                var code = context.Request.QueryString["code"];
                
                if (string.IsNullOrEmpty(code))
                {
                    Console.WriteLine("Authorization failed. No code received.");
                    return;
                }

                // Exchange the authorization code for tokens
                await AuthenticateWithUser(code);

                listener.Stop();
            }
        }
    }
}
