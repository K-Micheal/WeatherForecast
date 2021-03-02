using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherForecastSwagger.Models;

namespace WeatherForecastSwagger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/GetWeather")]// Get method for current weather
        public object Get()
        {
            string jsonResultWeather = String.Empty;
            WeatherForecast weather = new WeatherForecast();
            try
            {
                WebRequest request = WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?q=Dnipro&units=metric&appid=1e250eda137c9d35e05ea0e15709bd6e");
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {

                            jsonResultWeather = reader.ReadToEnd();
                            Root DeserializedClass = JsonConvert.DeserializeObject<Root>(jsonResultWeather);
                            weather.CityName = DeserializedClass.name;
                            weather.Date = weather.UnixTimeStampToDateTime(DeserializedClass.dt);
                            weather.TemperatureC = DeserializedClass.main.temp;
                            weather.WindSpeed = DeserializedClass.wind.speed;
                            weather.PercentCloudiness = DeserializedClass.clouds.all;

                        }
                    }
                }
                return weather;
            }


            catch (Exception ex)
            {
                // Get type of exception
                string exceptionMessage = ex.Message;
                return RedirectToRoute(new { controller = "Error", action = "Error", exceptionMessage = exceptionMessage });
            }

            
        }

        [HttpGet("/GetForecast")]// Get method for get forecast
        public object GetForecast()
        {
            WeatherForecast forecast = new WeatherForecast();
            List<WeatherForecast> listForecast = new List<WeatherForecast>();
            string jsonResultForecast = String.Empty;

            try
            {
                WebRequest request = WebRequest.Create("https://api.openweathermap.org/data/2.5/forecast?q=Dnipro&units=metric&appid=1e250eda137c9d35e05ea0e15709bd6e");
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {

                            jsonResultForecast = reader.ReadToEnd();
                            RootForecast DeserializedClass = JsonConvert.DeserializeObject<RootForecast>(jsonResultForecast);
                            foreach (List l in DeserializedClass.list)
                            {
                                WeatherForecast wf = new WeatherForecast();
                                int date1 = wf.UnixTimeStampToDateTime(l.dt).Hour;
                                if (wf.UnixTimeStampToDateTime(l.dt).Hour == 11)
                                {

                                    wf.Date = wf.UnixTimeStampToDateTime(l.dt);
                                    wf.CityName = DeserializedClass.city.name;
                                    wf.TemperatureC = l.main.temp;
                                    wf.WindSpeed = (int)l.wind.speed;
                                    wf.PercentCloudiness = l.clouds.all;
                                    listForecast.Add(wf);
                                }
                            }


                        }
                    }
                }
                return listForecast;
            }


            catch (Exception ex)
            {
                // Get type of exception
                
                string exceptionMessage = ex.Message;
                return RedirectToRoute(new { controller = "Error", action = "Error", exceptionMessage = exceptionMessage });
            }

            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
