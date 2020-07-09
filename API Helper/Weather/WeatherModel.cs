using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Helper
{
    public class WeatherModel
    {
        public WeatherMainModel Main { get; set; }

        public WeatherWeatherModel[] Weather { get; set; }

        public int Visibility { get; set; }
    }

    /// <summary>
    /// The way the API returns the data is kind of messy, so this class will make it more organised. 
    /// </summary>
    public class WeatherData
    {
        public string CloudDescription { get; set; }

        public int Visibility { get; set; }

        public double FeelsLike { get; set; }

        public double Temp { get; set; }

        public int Humidity { get; set; }

        /// <summary>
        /// Because I don't want to get a request unless at least 1 hour has passed.
        /// Even when the person has closed the app, because I have an access rate limit.
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public WeatherData()
        {
        }

        public WeatherData(WeatherModel weatherModel)
        {
            CloudDescription = weatherModel.Weather[0].Description;
            Visibility = weatherModel.Visibility;
            FeelsLike = weatherModel.Main.Feels_like;
            Temp = weatherModel.Main.Temp;
            Humidity = weatherModel.Main.Humidity;
        }
    }
}
