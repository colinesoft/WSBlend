using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using WSBlend.Models;
using WSBlend.Services.ExternalService;

namespace WSBlend.Controllers
{
    internal class Pokemon
    {
        public long Count { get; set; }
        public object Next { get; set; }
        public object Previous { get; set; }
        public List<Result> Results { get; set; }

        public partial class Result
        {
            public string Name { get; set; }
            public Uri Url { get; set; }
        }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            BaseExternalService _service = new BaseExternalService("https://localhost:7294/api/");
            var retorno = _service.GetJsonAsync("StatusGeral/GetAll").Result;
            var url = "https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0";
            List<Pokemon.Result> pkr = new List<Pokemon.Result>();

            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                var listPokemon = JsonConvert.DeserializeObject<Pokemon>(results);

                foreach (var item in listPokemon.Results)
                {
                    pkr.Add(item);
                }
            }
            var sample = GetStatusGeral();
            return View(pkr);
        }

        private List<StatusGeralDTO> GetStatusGeral()
        {
            string url = "https://localhost:7294/api/StatusGeral/GetAll";
            List<StatusGeralDTO> sg = new List<StatusGeralDTO>();

            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var lista = JsonConvert.DeserializeObject<List<StatusGeralDTO>>(result);

                foreach (var item in lista)
                {
                    sg.Add(item);
                }
            }
            return sg;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}