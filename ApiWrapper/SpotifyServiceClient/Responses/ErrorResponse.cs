namespace ApiWrapper.SpotifyServiceClient.Responses
{
    public class ErrorResponse
    {
        public Error error { get; set; }
    }
    public class Error
    {
        public int status { get; set; }
        public string message { get; set; }
    }

}