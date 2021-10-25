namespace ApiWrapper.SpotifyServiceClient.Requests
{
    public class GetNewReleasesRequest
    {
        public string Country { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}