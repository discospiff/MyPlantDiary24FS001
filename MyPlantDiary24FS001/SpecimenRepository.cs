using PlantPlacesSpecimens;

namespace MyPlantDiary24FS001
{
    public class SpecimenRepository
    {
        static SpecimenRepository()
        {
            allSpecimens = new List<Specimen>();
        }
        public static IList<Specimen> allSpecimens { get; set; }

    }
}
