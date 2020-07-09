using API_Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Serialization;

namespace Win_Weather_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string allCachePath = Assembly.GetExecutingAssembly().Location.Split('\\')
                .SkipLast(1).Skip(1)
                .Aggregate("C:", (w1, w2) => $"{w1}\\{w2}"); //This will get the path of the exe file and remove the exe file on the path.

        /// <summary>
        /// This will allow us to cache it (by serializing the data).
        /// </summary>
        public WeatherData Weather { get; set; }

        /// <summary>
        /// This will allo me to get the city name easily.
        /// </summary>
        public Coordinates Coordinates { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
        }

        /// <summary>
        /// This will check if the cache is emtpy and if an hour has passed. If an hour has passed, it will 
        /// request the api the weather and location. I chose an hour because I have a limit on the number of 
        /// calls per day.
        /// </summary>
        /// <returns></returns>
        private async Task LoadWeatherData()
        {

            string weatherCachePath = @$"{allCachePath}\WeatherCache.xml";
            string locationCachePath = @$"{allCachePath}\LocationCache.xml";

            try
            {
                Weather = Serilizer<WeatherData>.Deserialize(weatherCachePath); //This will load the weather data from the cache
                if (DateTime.Now.Subtract(Weather.TimeStamp).TotalHours > 1) throw new InvalidOperationException(); //To recalculate
                Coordinates = Serilizer<Coordinates>.Deserialize(locationCachePath); //This will load the coordinates cache
                
            }
            catch (InvalidOperationException)
            {
                Coordinates = await Coordinates.GetCurrentLocation();
                Weather = await WheatherProcessor.GetWeather(Coordinates);

                //The two below lines will refresh the cache.
                Serilizer<WeatherData>.Serilize(Weather, weatherCachePath);
                Serilizer<Coordinates>.Serilize(Coordinates, locationCachePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateScreen()
        {
            //Change labels
            cityName.Content = Coordinates.City;
            temp.Content = $"{Math.Round(Weather.Temp, 1)} °C";
            skyDescr.Content = Weather.CloudDescription;
            humidity.Content = $"Humidity: {Weather.Humidity} %";
            visibility.Content = $"Visibiliy: {Weather.Visibility} m";
            feelsLike.Content = $"Feels like: {Math.Round(Weather.FeelsLike, 1)} °C";
        }

        private void LoadBackGround()
        {
            if (Weather.CloudDescription.ToLower().Contains("overcast"))
                WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\OvercastClouds.bmp"));
            else if (Weather.CloudDescription.ToLower().Contains("broken"))
                WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\BrokenClouds.bmp"));
            else if (Weather.CloudDescription.ToLower().Contains("rain"))
                WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\Rain.bmp"));
            else if (Weather.CloudDescription.ToLower().Contains("scattered") || Weather.CloudDescription.ToLower().Contains("few"))
                WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\ScatteredClouds.bmp"));
            else if (Weather.CloudDescription.ToLower().Contains("clear"))
                WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\ClearSky.bmp"));
            else WeatherBg.Source = new BitmapImage(new Uri(@$"{allCachePath}\Resources\Unknown.bmp"));
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CalculateWeather();
        }

        /// <summary>
        /// This will calculate the weather and refresh the interface.
        /// </summary>
        /// <returns></returns>
        private async Task CalculateWeather()
        {
            try
            {
                await LoadWeatherData();
                UpdateScreen();
                LoadBackGround();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                await TimeIt();
            }
        }

        /// <summary>
        /// This is a timer.
        /// </summary>
        /// <returns></returns>
        private async Task TimeIt()
        {
            int counter = 60;
            await Task.Run(delegate
            {
                while (counter > 0)
                {
                    counter -= 1;
                    Thread.Sleep(60000); //It will wait for a minute
                }
            });
            await CalculateWeather(); //After 1 h, it will refresh the weather.
        }
    }
}
