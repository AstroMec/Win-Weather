using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_Helper
{
	public class WheatherProcessor
	{
		public async static Task<WeatherData> GetWeather(Coordinates coordinates)
        {
			string url = $"https://api.openweathermap.org/data/2.5/weather?lat={coordinates.Latitude}&lon={coordinates.Longitude}" +
				$"&appid=fa3e3b93a26195fbcda92de01c6295c4";

			using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
				if (!response.IsSuccessStatusCode) throw new HttpResponseException(response.ReasonPhrase);
				else return new WeatherData(await response.Content.ReadAsAsync<WeatherModel>());
            } 
        }
	}
}
