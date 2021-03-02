using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecastSwagger.Controllers
{
    public class ErrorController : Controller
    {
        public string Error(string exceptionMessage)
        {
            return exceptionMessage;
        }
    }
}
