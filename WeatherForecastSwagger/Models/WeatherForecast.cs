using System;

namespace WeatherForecastSwagger.Models
{
    public class WeatherForecast
    {
        public string CityName { get; set; }
        public DateTime Date { get; set; }

        public double TemperatureC { get; set; }

        public double WindSpeed { get; set; }

        public int PercentCloudiness { get; set; }

        //convert Unix UTC to ISO format
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
