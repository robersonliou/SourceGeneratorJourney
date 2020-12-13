
using NetConf2020;

namespace SourceGeneratorJourney.Entities
{
    [DataBuilder]
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        
    }
}