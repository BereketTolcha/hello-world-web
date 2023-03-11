using hello_world_web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace hello_world_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<WeatherForcastModel> reservationList = new List<WeatherForcastModel>();
            using (var httpClient = new HttpClient())
            {
                var BackendURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("BackendConfiguration")["URL"];

                using (var response = await httpClient.GetAsync(BackendURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if(apiResponse != null && apiResponse.Trim() != string.Empty)
                    {
                        reservationList = JsonConvert.DeserializeObject<List<WeatherForcastModel>>(apiResponse);
                    }
                }
            }

            return View(reservationList);
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