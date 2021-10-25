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
        private Response<AuthorizationResponse> _bearerTokenResponse;
        public SpotifyClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _bearerTokenResponse = AuthorizationAsync().Result;
        }
        public async Task<Response<AuthorizationResponse>> AuthorizationAsync(CancellationToken cancellationToken = default)
        {
            var client_id = _configuration.GetValue<string>("Spotify:ClientId");
            var client_secret = _configuration.GetValue<string>("Spotify:ClientSecret");

            var authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{client_id}:{client_secret}"));
            
            var authUrl = _configuration.GetValue<string>("Spotify:AuthUrl");

            var result = await authUrl
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

        public async Task<Response<GetNewReleasesResponse>> GetNewReleasesAsync(CancellationToken cancellationToken = default)
        {
            if (_bearerTokenResponse.Error != null)
            {
                return new Response<GetNewReleasesResponse>()
                {
                    StatusCode = _bearerTokenResponse.StatusCode,
                    Error = _bearerTokenResponse.Error
                };
            }

            var bearerToken = _bearerTokenResponse.Data.Access_token;
            var baseUrl = _configuration.GetValue<string>("Spotify:BaseUrl");

            var result = await baseUrl
                .AllowAnyHttpStatus()
                .WithHeader("Content-Type", "application/json")
                .AppendPathSegment(EndPoints.GetNewReleasesRequest)
                .WithOAuthBearerToken(bearerToken)
                .GetAsync();

            if (result.StatusCode >= 300)
            {
                var error = await result.GetJsonAsync<ErrorResponse>();
                return new Response<GetNewReleasesResponse>()
                {
                    StatusCode = result.StatusCode,
                    Error = error
                };
            }

            var data = await result.GetJsonAsync<GetNewReleasesResponse>();

            return new Response<GetNewReleasesResponse>()
            {
                StatusCode = result.StatusCode,
                Data = data
            };
        }
    }
}