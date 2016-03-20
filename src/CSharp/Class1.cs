using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Helpers
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            Console.WriteLine("Hello from C#!");
            return a + b;
        }
    }

    public static class GeoLocation
    {
        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class LatLongResult
        {
            public Location location { get; set; }
            public double accuracy { get; set; }
        }

        public static async Task<Tuple<double, double>> GetLongLat(string key)
        {
            var data = "{ \"considerIp\": \"true\" }";
            var wc = new System.Net.WebClient();
            var result = await wc.UploadStringTaskAsync(new Uri("https://www.googleapis.com/geolocation/v1/geolocate?key=" + key), data);
            var parsed = JsonConvert.DeserializeObject<LatLongResult>(result);
            return Tuple.Create(parsed.location.lat, parsed.location.lng);
        }
    }
}
