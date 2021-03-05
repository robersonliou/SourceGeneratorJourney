using SourceGeneratorJourney.Entities;
using twMVC;

namespace SourceGeneratorJourney.Models
{
    [Mapping(typeof(PersonEntity))]
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
    }
}