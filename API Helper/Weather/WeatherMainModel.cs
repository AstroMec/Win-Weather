namespace API_Helper
{
    public class WeatherMainModel
    {
        double temp;
        double feelsLike;

        public double Temp { get => temp; set => temp = value - 273.15; } //This will automatically convert the temperature to celcius.

        public double Feels_like { get => feelsLike; set => feelsLike = value - 273.15; }

        public int Humidity { get; set; }
    }
}