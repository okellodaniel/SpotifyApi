using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApiWrapper.SpotifyServiceClient.Requests;
using ApiWrapper.SpotifyServiceClient.Responses;
using ApiWrapper.SpotifyServiceClient.Utilities;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace ApiWrapper.SpotifyServiceClient
{
    public class SpotifyClient : ISpotifyClient
    {
        private readonly IConfiguration _configuration;

        public SpotifyClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Response<AuthorizationResponse>> AuthorizationAsync(CancellationToken cancellationToken = default)
        {
            var client_id = _configuration.GetValue<string>("Spotify:ClientId");
            var client_secret = _configuration.GetValue<string>("Spotify:ClientSecret");

            var authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{client_id}:{client_secret}"));

            var request = new AuthorizationRequest()
            {
                Grant_type = "client_credentials"
            };

            var baseUrl = _configuration.GetValue<string>("Spotify:BaseUrl");

            var result = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(EndPoints.AuthorizationRequest)
                .WithHeader("Basic", $"{authorization}")
                .PostUrlEncodedAsync(new
                {
                    grant_type = request.Grant_type
                });

            if (result.StatusCode >= 300)
            {
                var error = await result.GetJsonAsync<ErrorResponse>();

                return new Response<AuthorizationResponse>()
                {
                    StatusCode = result.StatusCode,
                    Error = error
                };
            }

            var data = await result.GetJsonAsync<AuthorizationResponse>();

            return new Response<AuthorizationResponse>()
            {
                StatusCode = result.StatusCode,
                Data = data
            };
        }
    }
}