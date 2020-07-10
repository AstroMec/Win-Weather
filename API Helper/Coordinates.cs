using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace API_Helper
{
    public class Coordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string City { get; set; }

        public Coordinates()
        {
        }

        public static async Task<Coordinates> GetCurrentLocation()
        {
            string ip = GetIpAddress();
            string url = $"http://api.ipstack.com/{ip}?access_key=2bb9aeb4b9c1587141bb21e3a991afae&format=1";
            
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(new Uri(url)))
            {
                if (!response.IsSuccessStatusCode) throw new HttpResponseException(response.ReasonPhrase);
                else return await response.Content.ReadAsAsync<Coordinates>();
            }
        }

        public static string GetIpAddress()
        {
            return new WebClient().DownloadString("http://icanhazip.com").Trim();
        }
    }
}