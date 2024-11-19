using Microsoft.AspNetCore.Mvc;
using PlantPlacesSpecimens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyPlantDiary24FS001
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Specimen> Get()
        {
            return SpecimenRepository.allSpecimens;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public Specimen Get(int id)
        {
            return SpecimenRepository.allSpecimens[id];
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] Specimen specimen)
        {
            Console.WriteLine(specimen);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Specimen specimen)
        {
            Console.WriteLine(specimen);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine(id);
        }
    }
}
