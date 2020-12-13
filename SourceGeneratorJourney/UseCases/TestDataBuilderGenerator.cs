

using SourceGeneratorJourney.Entities;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestDataBuilderGenerator
    {
        public static void Run()
        {
            var personEntityBuilder = new PersonEntityBuilder();
            var personEntity = personEntityBuilder.WithCountry("asd").Build();
        }
    }
    
}