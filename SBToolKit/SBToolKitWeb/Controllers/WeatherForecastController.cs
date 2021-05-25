using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwitchServiceLibrary;
using SwitchServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBToolKitWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private ISwitchDataService _courseData;

        public WeatherForecastController(ISwitchDataService dataService)
        {
            _courseData = dataService;
        }

        [HttpPost]
        public async Task<IEnumerable<Course>> Post(string coursenumber)
        {
            Console.WriteLine(coursenumber);
            List<string> course = new() { coursenumber };
            return await _courseData.GetData(course);
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> Get()
        {
            List<string> course = new() { "60055" };
            return await _courseData.GetData(course);
        }
    }
}
