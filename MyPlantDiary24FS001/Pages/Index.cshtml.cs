using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantPlacesSpecimens;

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
                specimens = Specimen.FromJson(specimenJSON);
            }
            ViewData["Specimens"] = specimens;
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