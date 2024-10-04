using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Web;
using vyber_api.Services;

namespace vyber_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyControllers : ControllerBase
    {
        private readonly SpotifyAuthApp _spotifyAuthApp;

        public SpotifyControllers(SpotifyAuthApp spotifyAuthApp)
        {
            _spotifyAuthApp = spotifyAuthApp;
        }
           // Endpoint to start the Spotify authentication process
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            // Start the Spotify authentication process
            await _spotifyAuthApp.StartAuthenticationProcess();
            return Ok("Authentication process started. Check the console for further instructions.");
        }

        // Endpoint to get the authenticated user's profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userProfile = await _spotifyAuthApp.GetUserProfile();
                return Ok(userProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // [HttpPost("logout")]
        // public IActionResult Logout()
        // {
        //     _spotifyAuthApp.Logout();
        //      HttpContext.Session.Clear(); 
        //     return Ok("Successfully logged out from Spotify.");
        // }


        // Fetch user's Spotify playlists
        // [HttpGet]
        // public async Task GetUserPlaylists()
        // {
        //     var playlists = await spotify.Playlists.CurrentUsers();
        //     Console.WriteLine("Your Playlists:");
        //     foreach (var playlist in playlists.Items)
        //     {
        //         Console.WriteLine(playlist.Name);
        //     }
        // }
    }
}