

using SourceGeneratorJourney.Entities;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestDataBuilderGenerator
    {
        public static void Run()
        {
            var dateBuilder = new PersonEntityBuilder();
            var person = dateBuilder
                .WithId(1)
                .WithName("John")
                .WithGender("Male")
                .WithCountry("Taiwan").Build();
        }
    }
    
}