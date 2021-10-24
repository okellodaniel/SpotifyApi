using System.Threading.Tasks;
using ApiWrapper.SpotifyServiceClient;
using ApiWrapper.SpotifyServiceClient.Responses;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyApi.Controllers
{
    public class SpotifyController : Controller
    {
        private readonly ISpotifyClient _spotifyClient;

        public SpotifyController(ISpotifyClient spotifyClient)
        {
            _spotifyClient = spotifyClient;
        }
        // Post
        [HttpPost("AuthorizationRequest")]
        public async Task<ActionResult<Response<AuthorizationResponse>>> AuthorizationRequest()
        {
            var response = await _spotifyClient.AuthorizationAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}