using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyPlantDiary24FS001.Pages
{
    public class AutoCompletePlantsModel : PageModel
    {
        private IList<string> plantNames = new List<string>();

        public JsonResult OnGet()
        {
            string term = Request.Query["Term"];


            IList<string> filteredPlantNames = new List<string>();


            plantNames.Add("Redbud");
            plantNames.Add("Red Maple");
            plantNames.Add("Red Oak");
            plantNames.Add("Red Rose");
            plantNames.Add("Red Lilly");

            foreach (string plantName in plantNames)
            {
                if (plantName.Contains(term))
                {
                    filteredPlantNames.Add(plantName);
                }
            }

            return new JsonResult(filteredPlantNames);
        }
    }
}
