using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PlantPlacesPlants;
using PlantPlacesSpecimens;
using WeatherFeed;

namespace MyPlantDiary24FS001.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        HttpClient _httpClient = new HttpClient();


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Grab specimen data.
            Task<HttpResponseMessage> task = _httpClient.GetAsync("https://raw.githubusercontent.com/discospiff/data/refs/heads/main/specimens.json");
            HttpResponseMessage result = task.Result;

            List<Specimen> specimens = new List<Specimen>();
            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string specimenJSON = readString.Result;

                // validate incoming JSON
                // read our schema file.
                JSchema jSchema = JSchema.Parse(System.IO.File.ReadAllText("specimenschema.json"));
                JArray specimenArray = JArray.Parse(specimenJSON);
                // create a collection to hold errors.
                IList<string> validationEvents = new List<String>();

                if(specimenArray.IsValid(jSchema, out validationEvents))
                {
                    specimens = Specimen.FromJson(specimenJSON);
                } else
                {
                    foreach(string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                }

                
            }
            

            Task<HttpResponseMessage> plantTask = _httpClient.GetAsync("https://raw.githubusercontent.com/discospiff/data/refs/heads/main/thirstyplants.json");
            HttpResponseMessage plantResult = plantTask.Result;

            Task<string> plantStringTask = plantResult.Content.ReadAsStringAsync();
            string plantJSON = plantStringTask.Result;
            List<Plant> plants = Plant.FromJson(plantJSON);

            IDictionary<long, Plant> waterLovingPlants = new Dictionary<long, Plant>();
            foreach (var plant in plants)
            {
                waterLovingPlants[plant.Id] = plant;
            }

            List<Specimen> waterLovingSpecimens = new List<Specimen>();
            foreach(Specimen specimen in specimens)
            {
                if(waterLovingPlants.ContainsKey(specimen.PlantId))
                {
                    waterLovingSpecimens.Add(specimen);
                }
            }

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            string weatherApiKey = config["WeatherApiKey"];


            Task<HttpResponseMessage> weatherTask = _httpClient.GetAsync("https://api.weatherbit.io/v2.0/current?city=Cincinnati,OH&key=" + weatherApiKey);
            HttpResponseMessage weatherResult = weatherTask.Result;

            Task<string> weatherStringTask = weatherResult.Content.ReadAsStringAsync();
            string weatherJSON = weatherStringTask.Result;

            Weather weather = Weather.FromJson(weatherJSON);
            List<Datum> data = weather.Data;
            long precip = 0;
            foreach (Datum datum in data) {
                precip = datum.Precip;
            }

            if (precip < 1) {
                ViewData["WeatherMessage"] = "It's dry!  Water these plants.";
            }
            else {
                ViewData["WeatherMessage"] = "Rain Expected.   No need to water.";
            }


            ViewData["Specimens"] = waterLovingSpecimens;

            string brand = "My Plant Diary";
            string inBrand = Request.Query["Brand"];
            if (inBrand != null && inBrand.Length > 0) 
            {
                brand = inBrand;
            }
            ViewData["Brand"] = brand;
        }
    }
}