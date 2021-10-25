using System.Threading;
using System.Threading.Tasks;
using ApiWrapper.SpotifyServiceClient.Requests;
using ApiWrapper.SpotifyServiceClient.Responses;

namespace ApiWrapper.SpotifyServiceClient
{
    public interface ISpotifyClient
    {
        Task<Response<AuthorizationResponse>> AuthorizationAsync(CancellationToken cancellationToken = default);
        Task<Response<GetNewReleasesResponse>> GetNewReleasesAsync(CancellationToken cancellationToken = default);
    }
}