using System.Collections.Generic;

namespace ApiWrapper.SpotifyServiceClient.Responses
{
    public class GetNewReleasesResponse
    {
        public Albums Albums { get; set; }
    }
    public class Item
    {
        
    }

    public class Albums
    {
        public string Href { get; set; }
        public List<Item> Items { get; set; }
        public int Limit { get; set; }
        public string Next { get; set; }
        public int Offset { get; set; }
        public string Previous { get; set; }
        public int Total { get; set; }
    }
    
}