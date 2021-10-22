using System.Threading;
using System.Threading.Tasks;
using ApiWrapper.SpotifyServiceClient.Responses;

namespace ApiWrapper.SpotifyServiceClient
{
    public interface ISpotifyClient
    {
        Task<Response<AuthorizationResponse>> AuthorizationAsync(CancellationToken cancellationToken = default);
    }
}