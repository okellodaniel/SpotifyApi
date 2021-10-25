namespace ApiWrapper.SpotifyServiceClient.Responses
{
    public class ErrorResponse
    {
        public Error Error { get; set; }
    }
    public class Error
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

}