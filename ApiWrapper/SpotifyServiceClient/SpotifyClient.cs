using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApiWrapper.SpotifyServiceClient.Requests;
using ApiWrapper.SpotifyServiceClient.Responses;
using ApiWrapper.SpotifyServiceClient.Utilities;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
            
            var baseUrl = _configuration.GetValue<string>("Spotify:BaseUrl");

            var result = await baseUrl
                .AllowAnyHttpStatus()
                .AppendPathSegment(EndPoints.AuthorizationRequest)
                .WithHeader("Authorization", $"Basic {authorization}")
                .WithHeader("Content-Type","application/x-www-form-urlencoded")
                .PostUrlEncodedAsync(new
                {
                    grant_type = "client_credentials"
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