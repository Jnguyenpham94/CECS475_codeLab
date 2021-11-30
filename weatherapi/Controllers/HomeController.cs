using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using call_json_api.Models;
using Polly;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace call_json_api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
                
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {            
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        

        private async Task<object> GetWeatherForecasts()
        {
            // Get an instance of HttpClient from the factpry that we registered
            // in Startup.cs
            var client = _httpClientFactory.CreateClient("API Client");
            // string s = "sacramento";
            string s = "los angeles";
            String apiUrl = "/api/location/search/?query=" + s;
            int loc = 0;
            //  List<Location> li=null;
             var result1 = await client.GetAsync(apiUrl);
            if (result1.IsSuccessStatusCode)
            {
                // Read all of the response and deserialise it into an instace of
                // WeatherForecast class
                var content1 = await result1.Content.ReadAsStringAsync();
                List<Location> model1 = JsonConvert.DeserializeObject<List<Location>>(content1);
              //  Location m1 = model1.consolidated_location[0];
                //Location m2 = m1.consolidated_location[3];
                loc = model1[0].woeid;
              //  loc=model1[0].consolidated_location[3].woeid;
                Debug.WriteLine(loc);
              //  foreach(LocationArray i in model1)
              //  {
              //      loc = i.consolidated_location[3].woeid;                    }
                   // i.consolidated_location.woeid;
              //  }
               // loc = model1.consolidated_location[0].woeid;

                
           }
            Debug.WriteLine(loc);
            string url = "/api/location/" + loc + "/";
            var result = await client.GetAsync(url);
           if (result.IsSuccessStatusCode)
            {
                // Read all of the response and deserialise it into an instace of
                // WeatherForecast class
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherForecast>(content);
            }
            return null;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetWeatherForecasts();
            // Pass the data into the View
            return View(model);
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
